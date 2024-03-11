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
    public class ShiftsController : ControllerBase
    {
        private readonly IUnitofWork _uow;
        public ShiftsController(IUnitofWork uow) => _uow=uow;

        [HttpGet("AllShifts")]
        public async Task<IActionResult> AllShifts(string? shName, string? shDate, bool? aval, string order = OrderingBy.Ascending, int orderCol = 0)
        {
            var shiftList = aval.HasValue ? aval.Value ?
                await _uow.Shift.AvailableData()
                : await _uow.Shift.NotAvailableData()
                : await _uow.Shift.GetAll();

            #region Search and Sort
            shiftList= string.IsNullOrEmpty(shName) ? shiftList
                : _uow.Shift.Search(shiftList, sh => sh.ShiftName.Contains(shName));
            shiftList=string.IsNullOrEmpty(shDate) ? shiftList
                : _uow.Shift.Search(shiftList, sh => sh.StartDate.ToString().Contains(shDate));

            switch (orderCol)
            {
                case 1:
                    shiftList=_uow.Shift.Search(shiftList, sh => sh.ShiftName, order); break;
                case 2:
                    shiftList=_uow.Shift.Search(shiftList, sh => sh.StartDate, order); break;
                default:
                    shiftList=shiftList.OrderBy(sh => sh.ShiftID); break;
            }
            #endregion
            return shiftList.Any() ?
                Ok(shiftList)
                : BadRequest("There is no shifts ");
        }

        [HttpGet("ShiftByID")]
        public async Task<IActionResult> ShiftByID(int id)
        {
            var shift = await _uow.Shift.GetByID(id);
            if (shift==null) return BadRequest("There is no shift ");
            return Ok(shift);
        }

        [HttpPost("NewShift")]
        public async Task<IActionResult> NewShift(Shifts shift)
        {
            try
            {
                if (!(await _uow.Shift.Repeated(s => s.ShiftName==shift.ShiftName)))
                {
                    await _uow.Shift.Add(shift);
                    _uow.Commit();
                }
                return Ok(shift);
            }
            catch (Exception ex) { return BadRequest("Error in adding new shift"+ex.Message); }
        }

        [HttpPut("UpdateShift")]
        public async Task<IActionResult> UpdateShift(Shifts shift)
        {
            try
            {
                if (!(await _uow.Shift.Repeated(s => s.ShiftName==shift.ShiftName)))
                {
                    _uow.Shift.Update(shift);
                    _uow.Commit();
                }
                return Ok(shift);
            }
            catch (Exception ex) { return BadRequest("Error in update shift" + ex.Message); }
        }

        [HttpDelete("DelShift")]
        public async Task<IActionResult> DelShift(int id)
        {
            var shift = await _uow.Shift.GetByID(id);
            try
            {
                if (_uow.Shift.Deletable(shift))
                {
                    _uow.Shift.Delete(shift);
                    _uow.Commit();
                    return StatusCode(StatusCodes.Status200OK);
                }
                return Ok("Shift willn't delete");
            }
            catch (Exception ex) { return BadRequest("Error in deleting shift"+ex.Message); }
        }


        [HttpPut("StopRestore")]
        public async Task<IActionResult> StopRestore(Shifts shift)
        {
            try
            {
                var shiftItem = shift.EndDate.HasValue ?
                         await _uow.Shift.RestoreElement(shift)
                         : await _uow.Shift.StopElement(shift);
                return Ok(shiftItem);
            }
            catch (Exception ex) { return BadRequest("Error in stopping/restoring certificate"+ex.Message); }
        }
    }
}
