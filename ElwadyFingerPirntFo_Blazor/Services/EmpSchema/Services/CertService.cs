using ElwadyFingerPirntFo_Blazor.Services.EmpSchema.Interfaces;
using ElwadyFingerPrint.Core.Interfaces.Emp;
using ElwadyFingerPrint.Core.Models.EmpSchema;
using EmpSchema;

namespace ElwadyFingerPirntFo_Blazor.Services.EmpSchema.Services
{
    public class CertService : ServiceRepository<CertificateTbl>,ICertService
    {
        public CertService() => ControllerUrl="Certificate/";


    }
}
