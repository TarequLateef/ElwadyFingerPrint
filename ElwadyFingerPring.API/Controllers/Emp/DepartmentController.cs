using Azure.Core;
using ElwadySales.Core.Consts;
using EmpSchema;
using IunitWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ElwadyFingerPring.API.Controllers.Emp
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitofWork _uow;
        public DepartmentController(IUnitofWork uow) => _uow=uow;

        [HttpGet("AllDepts")]
        public async Task<IActionResult> AllDepts(string? dName, string? sDate, bool? aval, int orderCol = 0, string order = OrderingBy.Ascending)
        {
            var deptList = aval.HasValue ? aval.Value ?
                await _uow.Depts.AvailableData()
                : await _uow.Depts.NotAvailableData()
                : await _uow.Depts.GetAll();

            if (!deptList.Any()) return BadRequest("There is no departs");
            #region Search Sort
            deptList=string.IsNullOrEmpty(dName) ? deptList
                : _uow.Depts.Search(deptList, d => d.DeptName.Contains(dName));
            deptList=string.IsNullOrEmpty(sDate) ? deptList
                : _uow.Depts.Search(deptList, d => d.StartDate.ToString().Contains(sDate));
            switch (orderCol)
            {
                case 1:
                    deptList=_uow.Depts.Search(deptList, d => d.DeptName, order);
                    break;
                case 2:
                    deptList=_uow.Depts.Search(deptList, d => d.StartDate, order);
                    break;
                default:
                    deptList=deptList.OrderBy(d => d.DeptID);
                    break;
            }
            #endregion
            return Ok(deptList);
        }

        [HttpGet("DeptByID")]
        public async Task<IActionResult> DeptByID(int id)
        {
            var dept = await _uow.Depts.GetByID(id);
            return dept != null ? Ok(dept) : BadRequest("There is no department by this id");
        }

        [HttpPost("AddDept")]
        public async Task<IActionResult> AddDept(Department dept)
        {
            try
            {
                if (!(await _uow.Depts.Repeated(d => d.DeptName==dept.DeptName)))
                {
                    await _uow.Depts.Add(dept);
                    _uow.Commit();
                }
                return Ok(dept);
            }
            catch (Exception ex) { return BadRequest("Error in adding new depart"+ ex.Message); }
        }

        [HttpPut("UpdateDept")]
        public async Task<IActionResult> UpdateDept(Department dept)
        {
            try
            {
                if (!(await _uow.Depts.Repeated(d => d.DeptName==dept.DeptName)))
                {
                    _uow.Depts.Update(dept);
                    _uow.Commit();
                }
                return Ok(dept);
            }
            catch (Exception ex) { return BadRequest("Error in updating the department" + ex.Message); }
        }

        [HttpDelete("DelDept")]
        public async Task<IActionResult> DelDept(int id)
        {
            var dept = await _uow.Depts.GetByID(id);
            try
            {
                if (_uow.Depts.Deletable(dept))
                {
                    _uow.Depts.Delete(dept);
                    _uow.Commit();
                    return StatusCode(StatusCodes.Status200OK);
                }
                return Ok("This department willn't delete");
            }
            catch (Exception ex) { return BadRequest("Error in deleting this depart"+ex.Message); }
        }

        [HttpPut("StopRestore")]
        public async Task<IActionResult> StopRestore(Department dept)
        {
            try
            {
                var deptItem = dept.EndDate.HasValue ?
                         await _uow.Depts.RestoreElement(dept)
                         : await _uow.Depts.StopElement(dept);
                return Ok(deptItem);
            }
            catch (Exception ex) { return BadRequest("Error in stopping/restoring certificate"+ex.Message); }
        }

    }
}
