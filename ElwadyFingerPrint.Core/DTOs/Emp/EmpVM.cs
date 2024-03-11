using EmpSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElwadyFingerPrint.Core.DTOs.Emp
{
    public class EmpVM:Employees
    {
        public string CertName { get; set; } = string.Empty;
    }
}
