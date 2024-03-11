using ElwadyFingerPrint.Core.Interfaces.Emp;
using ElwadySales.Core.Consts;
using EmpSchema;
using IunitWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ElwadyFingerPring.API.Controllers.Emp
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpShiftController : ControllerBase
    {
        private readonly IUnitofWork _uow;
        public EmpShiftController(IUnitofWork uow) => _uow = uow;

        [HttpGet("AllEmpShifts")]
        public async Task<IActionResult> AllEmpShifts(string? eName, string? shName, string? esDate, bool? aval, string order = OrderingBy.Ascending, int orderCol = 0)
        {
            var esList = aval.HasValue ? aval.Value ?
                await _uow.EmpShift.AvailableData()
                : await _uow.EmpShift.NotAvailableData()
                : await _uow.EmpShift.Search(new[] { "Employees", "Shifts" });

            #region Search and Sort
            esList= string.IsNullOrEmpty(eName) ? esList
                : _uow.EmpShift.Search(esList, es => es.Employees.EmpName.Contains(eName));
            esList= string.IsNullOrEmpty(shName) ? esList
                : _uow.EmpShift.Search(esList, es => es.Shifts.ShiftName.Contains(shName));
            esList= string.IsNullOrEmpty(esDate) ? esList
                : _uow.EmpShift.Search(esList, es => es.StartDate.ToString().Contains(esDate));

            switch (orderCol)
            {
                case 1:
                    esList=_uow.EmpShift.Search(esList, es => es.Employees.EmpName, order); break;
                case 2:
                    esList=_uow.EmpShift.Search(esList, es => es.Shifts.ShiftID, order); break;
                case 3:
                    esList=_uow.EmpShift.Search(esList, es => es.StartDate, order); break;
                default:
                    esList=esList.OrderBy(es => es.ShiftID); break;
            }

            #endregion
            return Ok(esList);
        }

        [HttpGet("EmpShiftDetails")]
        public async Task<IActionResult> EmpShiftDetails(int id)
        {
            var esItem = await _uow.EmpShift.GetByID(id);
            if (esItem==null) return BadRequest("There is no employee in this shift now");
            esItem.Employees=await _uow.Employee.GetByID(esItem.EmpID);
            esItem.Shifts=await _uow.Shift.GetByID(esItem.ShiftID);
            return Ok(esItem);
        }

        [HttpPost("AddEmpShift")]
        public async Task<IActionResult> AddEmpShift(EmpShift newES)
        {
            try
            {
                var esItem = new EmpShift();
                if (_uow.EmpShift.Repeated(es => es.EmpID==newES.EmpID && es.ShiftID==newES.ShiftID && !es.EndDate.HasValue, out esItem)) return Ok(esItem);
                await _uow.EmpShift.Add(newES);
                var esList = await _uow.EmpShift.Search(es => es.EmpID==newES.EmpID && !es.EndDate.HasValue);
                if (!esList.Any())
                    foreach (var item in esList) await _uow.EmpShift.StopElement(item);
                _uow.Commit();
                return Ok(esList);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("UpdateEmpShift")]
        public async Task<IActionResult> UpdateEmpShift(EmpShift editES)
        {
            try
            {

                var es = new EmpShift();
                if (!(_uow.EmpShift.Repeated(ej => ej.EmpID==editES.EmpID && ej.ShiftID==editES.ShiftID && !ej.EndDate.HasValue, out es)))
                {
                    //in front end we'll update just the job not the employee
                    await _uow.EmpShift.StopElement(es);
                    await _uow.EmpShift.Add(es);
                    _uow.Commit();
                }
                return Ok(editES);
            }
            catch (Exception ex) { return BadRequest("Error in updating emp's shift data"+ex.Message); }
        }

        [HttpDelete("DelEmpShift")]
        public async Task<IActionResult> DelEmpShift(int id)
        {
            var es = await _uow.EmpShift.GetByID(id);
            if (es==null) return BadRequest("There is no employee in this shift");
            try
            {
                if (_uow.EmpShift.Deletable(es))
                {
                    _uow.EmpShift.Delete(es);
                    _uow.Commit();
                    return StatusCode(StatusCodes.Status200OK);
                }
                return Ok("Couldn't delete emp job");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("StopRestore")]
        public async Task<IActionResult> StopRestore(EmpShift es)
        {
            try
            {
                var esItem = es.EndDate.HasValue ?
                         await _uow.EmpShift.RestoreElement(es)
                         : await _uow.EmpShift.StopElement(es);
                return Ok(esItem);
            }
            catch (Exception ex) { return BadRequest("Error in stopping/restoring Employee in shift"+ex.Message); }
        }


    }
}
