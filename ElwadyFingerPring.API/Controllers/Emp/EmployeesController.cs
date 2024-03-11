using Azure;
using ElwadyFingerPrint.EF.Repositories;
using ElwadySales.Core.Consts;
using EmpSchema;
using HrCodeFirstDB;
using IunitWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ElwadyFingerPring.API.Controllers.Emp
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitofWork _unt;
        public EmployeesController(IUnitofWork unt) => _unt = unt;

        [HttpGet("GetAllEmps")]
        public async Task<IActionResult> GetAllEmps(bool? aval, string? name, string? nationalNo, string? certName, string? sDate, int sortCol = 0, string ordering = OrderingBy.Ascending, int pageNo = 1)
        {
            var empList = aval.HasValue ?
                aval.Value ? await _unt.Employee.AvailableData()
                : await _unt.Employee.NotAvailableData()
                : await _unt.Employee.Search(new[] { "Cert" });
            if (empList.Any())
            {
                #region Searching
                empList= string.IsNullOrEmpty(name) ? empList
                    : _unt.Employee.Search(empList, e => e.EmpName.Contains(name));
                empList=string.IsNullOrEmpty(nationalNo) ? empList
                    : _unt.Employee.Search(empList, e => e.NationalNo.Contains(nationalNo));
                empList=string.IsNullOrEmpty(certName) ? empList
                    : _unt.Employee.Search(empList, e => e.Cert.CertName.Contains(certName));
                empList=string.IsNullOrEmpty(sDate) ? empList
                    : _unt.Employee.Search(empList, e => e.StartDate.ToString().Contains(sDate));
                #endregion

                #region Sorting
                switch (sortCol)
                {
                    case 1:
                        empList=_unt.Employee.Search(empList, e => e.EmpName, ordering);
                        break;
                    case 2:
                        empList = _unt.Employee.Search(empList, e => e.StartDate, ordering);
                        break;
                    default:
                        empList=empList.OrderBy(e => e.EmpID);
                        break;
                }
                #endregion

            }
            /*ReturnedData<Employees> rd = new ReturnedData<Employees>(empList.ToList());*/
            return empList is null || !empList.Any() ?
                BadRequest("There is no Employees") :
                Ok(empList);
        }

        [HttpGet("GetEmpID")]
        public async Task<IActionResult> GetEmpID(int id)
        {
            try
            {
                var emp = await _unt.Employee.GetByID(id);
                if (emp is not null)
                    emp.Cert=await _unt.Certificate.GetByID(emp.CertID);
                return emp is null ?
                    BadRequest("There is no emp whith this id")
                    : Ok(emp);
            }
            catch (Exception ex) { return BadRequest("Error in getting the data"+ex.Message); }
        }

        [HttpPost("AddEmp")]
        public async Task<IActionResult> AddEmp(Employees newEmp)
        {
            try
            {
                if (!(await _unt.Employee.Repeated(e => e.NationalNo==newEmp.NationalNo)))
                {
                    await _unt.Employee.Add(newEmp);
                    _unt.Commit();
                }
                return Ok(newEmp);
            }
            catch (Exception ex) { return BadRequest("Error in adding emp"+ex.Message); }
        }

        [HttpPut("UpdateEmp/{employees}")]
        public async Task<IActionResult> UpdateEmp(Employees employees)
        {
            try
            {
                if (!(await _unt.Employee.Repeated(e => e.NationalNo==employees.NationalNo || e.InsuranceNo==employees.InsuranceNo)))
                {
                    employees= await _unt.Employee.Update(employees, employees.EmpID);
                    _unt.Commit();
                }
                return Ok(employees);
            }
            catch (Exception ex) { return BadRequest("Error in updating the emp data" + ex.Message); }
        }

        [HttpDelete("DelEmp")]
        public async Task<IActionResult> DelEmp(int empID)
        {
            try
            {
                var emp = await _unt.Employee.GetByID(empID);
                if (_unt.Employee.Deletable(emp))
                {
                    _unt.Employee.Delete(emp);
                    _unt.Commit();
                    return StatusCode(StatusCodes.Status200OK);
                }
                return Ok("Can't Deleted");
            }
            catch (Exception ex) { return BadRequest("Can't delete the emp " + ex.Message); }
        }

        [HttpPut("StopRestore")]
        public async Task<IActionResult> StopRestore(Employees emp)
        {
            try
            {
                var empItem = emp.EndDate.HasValue ?
                         await _unt.Employee.RestoreElement(emp)
                         : await _unt.Employee.StopElement(emp);
                return Ok(empItem);
            }
            catch (Exception ex) { return BadRequest("Error in stopping/restoring certificate"+ex.Message); }
        }
    }
}
