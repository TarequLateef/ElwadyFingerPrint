using ElwadyFingerPrint.Core.Interfaces.Emp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IunitWork
{
    public interface IUnitofWork:IDisposable
    {
        #region Emp Schema
        IEmployee Employee { get; }
        ICertified Certificate { get; }
        IDepts Depts { get; }
        IJobs Job { get; }
        IShift Shift { get; }
        IEmpDept EmpDept { get; }
        IEmpJob EmpJob { get; }
        IEmpShift EmpShift { get; }
        #endregion
        int Commit();

        void Dispose();

    }
}
