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
    public class EmpDeptController : ControllerBase
    {
        private readonly IUnitofWork _uow;
        public EmpDeptController(IUnitofWork uow) => _uow=uow;

        [HttpGet("AllEmpinDept")]
        public async Task<IActionResult> AllEmpinDept(bool? aval, string? eName, string? dName, string? edDate, string order = OrderingBy.Ascending, int orderCol = 0)
        {
            var empDeptList = aval.HasValue ? aval.Value ?
                await _uow.EmpDept.AvailableData()
                : await _uow.EmpDept.NotAvailableData()
                : await _uow.EmpDept.Search(new[] { "Employees", "Department" });

            if (!empDeptList.Any()) return BadRequest("There is no employee in any department");

            #region Search and Sort
            empDeptList = string.IsNullOrEmpty(eName) ? empDeptList
                : _uow.EmpDept.Search(empDeptList, ed => ed.Employees.EmpName.Contains(eName));
            empDeptList= string.IsNullOrEmpty(dName) ? empDeptList
                : _uow.EmpDept.Search(empDeptList, ed => ed.Department.DeptName.Contains(dName));
            empDeptList=string.IsNullOrEmpty(edDate) ? empDeptList
                : _uow.EmpDept.Search(empDeptList, ed => ed.StartDate.ToString().Contains(edDate));

            switch (orderCol)
            {
                case 1:
                    empDeptList=_uow.EmpDept.Search(empDeptList, ed => ed.Employees.EmpName, order);
                    break;
                case 2:
                    empDeptList=_uow.EmpDept.Search(empDeptList, ed => ed.Department.DeptName, order);
                    break;
                case 3:
                    empDeptList=_uow.EmpDept.Search(empDeptList, ed => ed.StartDate, order);
                    break;
                default:
                    empDeptList=empDeptList.OrderBy(ed => ed.DeptID);
                    break;
            }
            #endregion
            return Ok(empDeptList);
        }

        [HttpGet("EmpDeptByID")]
        public async Task<IActionResult> EmpDeptByID(int id)
        {
            var empDept = await _uow.EmpDept.GetByID(id);
            if (empDept==null) return BadRequest("There is no emp in this dept");
            var emp = await _uow.Employee.GetByID(empDept.EmpID);
            var dept = await _uow.Depts.GetByID(empDept.DeptID);
            empDept.Employees= emp!=null ? emp : new Employees();
            empDept.Department= dept!=null ? dept : new Department();
            return Ok(empDept);
        }

        [HttpPost("AddEmpinDept")]
        public async Task<IActionResult> AddEmpinDept(EmpDept empDept)
        {
            try
            {
                var repEmpDept = new EmpDept();
                if (_uow.EmpDept.Repeated(ed => ed.EmpID==empDept.EmpID && ed.DeptID==empDept.DeptID, out repEmpDept)) return Ok(repEmpDept);
                var edList = await _uow.EmpDept.Search(ed => ed.EmpID==empDept.EmpID && ed.DeptID==empDept.DeptID && !ed.EndDate.HasValue);
                if (!edList.Any())
                    foreach (var item in edList) await _uow.EmpDept.StopElement(item);

                await _uow.EmpDept.Add(empDept);
                _uow.Commit();
                return Ok(empDept);
            }
            catch (Exception ex) { return BadRequest("Error in adding emp in dept"+ex.Message); }
        }

        [HttpPut("UpdateEmpDept")]
        public async Task<IActionResult> UpdateEmpDept(EmpDept empDept)
        {
            try
            {
                var repEmpDept = new EmpDept();
                if (!(_uow.EmpDept.Repeated(ed => ed.EmpID==empDept.EmpID && ed.DeptID==empDept.DeptID, out repEmpDept)))
                {
                    await _uow.EmpDept.StopElement(repEmpDept);
                    await _uow.EmpDept.Add(empDept);
                    _uow.Commit();
                }
                return Ok(empDept);
            }
            catch (Exception ex) { return BadRequest("Error in updating the data"+ex.Message); }
        }

        [HttpDelete("DelEmpDept")]
        public async Task<IActionResult> DelEmpDept(int id)
        {
            var empDept = await _uow.EmpDept.GetByID(id);
            try
            {
                if (empDept==null) return BadRequest("There is no emp in this dept");
                if (_uow.EmpDept.Deletable(empDept))
                {
                    _uow.EmpDept.Delete(empDept);
                    _uow.Commit();
                    return StatusCode(StatusCodes.Status200OK);
                }
                return Ok("Couldn't delete the emp from dept");
            }
            catch (Exception ex) { return BadRequest("Error in deleting the emp form dept"); }
        }

        [HttpPut("StopRestore")]
        public async Task<IActionResult> StopRestore(EmpDept empDept)
        {
            try
            {
                var empDeptItem = empDept.EndDate.HasValue ?
                         await _uow.EmpDept.RestoreElement(empDept)
                         : await _uow.EmpDept.StopElement(empDept);
                return Ok(empDeptItem);
            }
            catch (Exception ex) { return BadRequest("Error in stopping/restoring certificate"+ex.Message); }
        }

    }
}
