using ElwadyFingerPrint.Core.Models.EmpSchema;
using ElwadySales.Core.Consts;
using EmpSchema;
using IunitWork;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ElwadyFingerPring.API.Controllers.Emp
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly IUnitofWork _uow;
        public CertificateController(IUnitofWork uow) =>
            this._uow = uow;

        [HttpGet("AllCerts")]
        public async Task<IActionResult> AllCerts(string? cName, string? sDate, bool? aval, int colOrder = 0, string orderby = OrderingBy.Ascending)
        {
            var certList = aval.HasValue ? aval.Value ?
               await _uow.Certificate.AvailableData()
                : await _uow.Certificate.NotAvailableData()
                : await _uow.Certificate.GetAll();

            if (!certList.Any()) return BadRequest("There is no certificates");

            #region Searching and Ordering
            certList=string.IsNullOrEmpty(cName) ? certList
                : _uow.Certificate.Search(certList, c => c.CertName.Contains(cName));
            certList=string.IsNullOrEmpty(sDate) ? certList
                  : _uow.Certificate.Search(certList, c => c.StartDate.ToString().Contains(sDate));
            switch (colOrder)
            {
                case 1:
                    certList= _uow.Certificate.Search(certList, c => c.CertName, orderby);
                    break;
                case 2:
                    certList=_uow.Certificate.Search(certList, c => c.StartDate, orderby);
                    break;
                default:
                    certList= certList.OrderBy(c => c.CertID);
                    break;
            }
            #endregion
            return Ok(certList);
        }

        [HttpGet("CertByID")]
        public async Task<IActionResult> CertByID(int id)
        {
            var cert = await _uow.Certificate.GetByID(id);
            return cert==null ?
                BadRequest("There is no cert")
                : Ok(cert);
        }

        [HttpPost(Name = "AddCert")]
        public async Task<IActionResult> AddCert(CertificateTbl cert)
        {
            try
            {
                if (!(await _uow.Certificate.Repeated(c => c.CertName==cert.CertName)))
                {
                    await _uow.Certificate.Add(cert);
                    _uow.Commit();
                }
                return Ok(cert);
            }
            catch (Exception ex)
            {
                return BadRequest("Error in adding new certificates"+ex.Message);
            }
        }

        /*                [HttpPut("UpdateCert")]
                public async Task<IActionResult> UpdateCert(CertificateTbl certificates)
                {
                    var cert = await _uow.Certificate.GetByID(certificates.CertID);
                    try
                    {
                        if (cert==null) return BadRequest("There is no certificate with this id");
                        if (!(await _uow.Certificate.Repeated(c => c.CertName==certificates.CertName)))
                        {
                            _uow.Certificate.Update(cert);
                            _uow.Commit();
                        }
                        return Ok(cert);
                    }
                    catch (Exception ex) { return BadRequest("Error in updating the certificates" + ex.Message); }
                }
        */
        [HttpDelete("DeleteCert")]
        public async Task<IActionResult> DeleteCert(int id)
        {
            var cert = await _uow.Certificate.GetByID(id);
            try
            {
                if (cert==null) return BadRequest("There is no certificate with this ID");

                if (_uow.Certificate.Deletable(cert))
                {
                    _uow.Certificate.Delete(cert);
                    _uow.Commit();
                    return StatusCode(StatusCodes.Status200OK);
                }
                return Ok("Can't Delete this certificate");
            }
            catch (Exception ex) { return BadRequest("Error in deleting the cert" + ex.Message); }
        }

        /*        [HttpPut("StopRestore")]
                public async Task<IActionResult> StopRestore(CertificateTbl certificates)
                {
                    try
                    {
                        var cert = certificates.EndDate.HasValue ?
                                 await _uow.Certificate.RestoreElement(certificates)
                                 : await _uow.Certificate.StopElement(certificates);
                        return Ok(cert);
                    }
                    catch (Exception ex) { return BadRequest("Error in stopping/restoring certificate"+ex.Message); }
                }
        */
    }
}
