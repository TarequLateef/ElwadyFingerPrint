using ElwadySales.Core.Consts;
using EmpSchema;
using IunitWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.AccessControl;

namespace ElwadyFingerPring.API.Controllers.Emp
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpJobController : ControllerBase
    {
        private readonly IUnitofWork _uow;
        public EmpJobController(IUnitofWork uow) => _uow = uow;

        [HttpGet("AllEmpJobs")]
        public async Task<IActionResult> AllEmpJobs(string? eName, string? jName, string? ejDate, bool? aval, string order = OrderingBy.Ascending, int orderCol = 0)
        {
            var ejList = aval.HasValue ? aval.Value ?
                await _uow.EmpJob.AvailableData()
                : await _uow.EmpJob.NotAvailableData()
                : await _uow.EmpJob.Search(new[] { "Employees", "Jobs" });

            if (!ejList.Any()) return BadRequest("There is no Employee in jobs");

            #region Search and Sort
            ejList= string.IsNullOrEmpty(eName) ? ejList
                : _uow.EmpJob.Search(ejList, ej => ej.Employees.EmpName.Contains(eName));
            ejList= string.IsNullOrEmpty(jName) ? ejList
                : _uow.EmpJob.Search(ejList, ej => ej.Jobs.JobName.Contains(jName));
            ejList= string.IsNullOrEmpty(ejDate) ? ejList
               : _uow.EmpJob.Search(ejList, ej => ej.StartDate.ToString().Contains(ejDate));

            switch (orderCol)
            {
                case 1:
                    ejList=_uow.EmpJob.Search(ejList, ej => ej.Employees.EmpName, order); break;
                case 2:
                    ejList=_uow.EmpJob.Search(ejList, ej => ej.Jobs.JobName, order); break;
                case 3:
                    ejList=_uow.EmpJob.Search(ejList, ej => ej.StartDate, order); break;
                default:
                    ejList=ejList.OrderBy(ej => ej.JobID); break;
            }
            #endregion

            return Ok(ejList);
        }

        [HttpGet("EmpJobByID")]
        public async Task<IActionResult> EmpJobByID(int id)
        {
            var ej = await _uow.EmpJob.GetByID(id);
            if (ej==null) return BadRequest("There is no emp in this job");
            ej.Employees =await _uow.Employee.GetByID(ej.EmpID);
            ej.Jobs=await _uow.Job.GetByID(ej.JobID);
            return Ok(ej);
        }

        [HttpPost("AddNewEmpJob")]
        public async Task<IActionResult> AddNewEmpJob(EmpJob empJob)
        {
            try
            {
                var newEJ = new EmpJob();
                if (_uow.EmpJob.Repeated(ej => ej.EmpID==empJob.EmpID && ej.JobID==empJob.JobID && !ej.EndDate.HasValue, out newEJ)) return Ok(newEJ);

                await _uow.EmpJob.Add(empJob);
                var ejList = await _uow.EmpJob.Search(ej => ej.EmpID==empJob.EmpID && !ej.EndDate.HasValue);
                if (ejList.Any())
                    foreach (var item in ejList) await _uow.EmpJob.StopElement(item);
                _uow.Commit();
                return Ok(empJob);
            }
            catch (Exception ex) { return BadRequest("Error in adding employee to the job"+ ex.Message); }
        }

        [HttpPut("UpdateEmpJob")]
        public async Task<IActionResult> UpdateEmpJob(EmpJob empJob)
        {
            try
            {
                var ej = new EmpJob();
                if (!(_uow.EmpJob.Repeated(ej => ej.EmpID==empJob.EmpID && ej.JobID==empJob.JobID && !ej.EndDate.HasValue, out ej)))
                {
                    //in front end we'll update just the job not the employee
                    await _uow.EmpJob.StopElement(ej);
                    await _uow.EmpJob.Add(empJob);
                    _uow.Commit();
                }
                return Ok(empJob);
            }
            catch (Exception ex) { return BadRequest("Error in updating emp's job data"+ex.Message); }
        }

        [HttpDelete("DelEmpJob")]
        public async Task<IActionResult> DelEmpJob(int id)
        {
            var empJob = await _uow.EmpJob.GetByID(id);
            if (empJob==null) return BadRequest("There is no employee in this job");
            try
            {
                if (_uow.EmpJob.Deletable(empJob))
                {
                    _uow.EmpJob.Delete(empJob);
                    _uow.Commit();
                    return StatusCode(StatusCodes.Status200OK);
                }
                return Ok("Couldn't delete emp job");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("StopRestore")]
        public async Task<IActionResult> StopRestore(EmpJob empjob)
        {
            try
            {
                var ejItem = empjob.EndDate.HasValue ?
                         await _uow.EmpJob.RestoreElement(empjob)
                         : await _uow.EmpJob.StopElement(empjob);
                return Ok(ejItem);
            }
            catch (Exception ex) { return BadRequest("Error in stopping/restoring Employee in job"+ex.Message); }
        }

    }
}
