using ElwadyFingerPrint.Core.DTOs.Emp;
using EmpSchema;
using IunitWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagment.core.Interfaces;

namespace ElwadyFingerPrint.Core.Interfaces.Emp
{
    public interface IEmployee:IUsingRepository<Employees>,IAvaliability<Employees>
    {
    }
}
