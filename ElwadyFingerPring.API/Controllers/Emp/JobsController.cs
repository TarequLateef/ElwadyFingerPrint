using ElwadySales.Core.Consts;
using EmpSchema;
using IunitWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ElwadyFingerPring.API.Controllers.Emp
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IUnitofWork _uow;
        public JobsController(IUnitofWork uow) => _uow=uow;

        [HttpGet("AllJobs")]
        public async Task<IActionResult> AllJobs(string? jName, string? jDate, bool? aval, string order = OrderingBy.Ascending, int colOrder = 1)
        {
            var jobList = aval.HasValue ? aval.Value ?
                await _uow.Job.AvailableData()
                : await _uow.Job.NotAvailableData()
                : await _uow.Job.GetAll();
            if (jobList.Any()) return BadRequest("There is no job");
            #region Search and Sort
            jobList=string.IsNullOrEmpty(jName) ? jobList
                : _uow.Job.Search(jobList, j => j.JobName.Contains(jName));
            jobList=string.IsNullOrEmpty(jDate) ? jobList
                : _uow.Job.Search(jobList, j => j.StartDate.ToString().Contains(jDate));

            switch (colOrder)
            {
                case 1:
                    jobList=_uow.Job.Search(jobList, j => j.JobName, order); break;
                default:
                    jobList=jobList.OrderBy(j => j.JobID); break;
            }
            #endregion
            return Ok(jobList);
        }

        [HttpGet("JobByID")]
        public async Task<IActionResult> JobsByID(int id)
        {
            var job = await _uow.Job.GetByID(id);
            if (job!=null) return Ok(job);
            return BadRequest("There is no job with this Data");
        }

        [HttpPost("NewJob")]
        public async Task<IActionResult> NewJob(Jobs job)
        {
            try
            {
                if (!(await _uow.Job.Repeated(j => j.JobName==job.JobName)))
                {
                    await _uow.Job.Add(job);
                    _uow.Commit();
                }
                return Ok(job);
            }
            catch (Exception ex) { return BadRequest("Error in adding a new job"+ex.Message); }
        }

        [HttpPut("UpdateJob")]
        public async Task<IActionResult> UpdateJob(Jobs job)
        {
            try
            {
                if (!(await _uow.Job.Repeated(j => j.JobName==job.JobName)))
                {
                    _uow.Job.Update(job);
                    _uow.Commit();
                }
                return Ok(job);
            }
            catch (Exception ex) { return BadRequest("Error in updating the job"+ex.Message); }
        }

        [HttpDelete("DelJob")]
        public async Task<IActionResult> DelJob(int id)
        {
            var job = await _uow.Job.GetByID(id);
            if (job==null) return BadRequest("There is no job to delete");
            try
            {
                if (_uow.Job.Deletable(job))
                {
                    _uow.Job.Delete(job);
                    _uow.Commit();
                    return StatusCode(StatusCodes.Status200OK);
                }
                return Ok("Couldn't delete this job");
            }
            catch (Exception ex) { return BadRequest("Error in deleting the jog"+ex.Message); }
        }

        [HttpPut("StopRestore")]
        public async Task<IActionResult> StopRestore(Jobs job)
        {
            try
            {
                var jobItem = job.EndDate.HasValue ?
                         await _uow.Job.RestoreElement(job)
                         : await _uow.Job.StopElement(job);
                return Ok(jobItem);
            }
            catch (Exception ex) { return BadRequest("Error in stopping/restoring certificate"+ex.Message); }
        }
  
    }
}
