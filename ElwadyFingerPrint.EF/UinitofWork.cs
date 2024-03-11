using ElwadyFingerPrint.Core.Interfaces.Emp;
using ElwadyFingerPrint.EF.Repositories.Emp;
using HrCodeFirstDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagment.core;
using UserManagment.core.Interfaces;
using UserManagment.EF.Repository;

namespace IunitWork
{
    public class UnitofWork : IUnitofWork
    {
        private readonly Hr_Db_CFContext _dbCtx;
        #region Emp Schema
        public IEmployee Employee { get; private set; }
        public ICertified Certificate { get; private set; }
        public IDepts Depts { get; private set; }
        public IJobs Job { get; private set; }
        public IShift Shift { get; private set; }
        public IEmpDept EmpDept { get; private set; }
        public IEmpJob EmpJob { get; private set; }
        public IEmpShift EmpShift { get; private set; }
        #endregion
        public UnitofWork(Hr_Db_CFContext dbContext)
        {
            _dbCtx = dbContext;
            #region Emp Schema
            this.Employee=new EmpRepo(_dbCtx);
            this.Certificate=new CertRepo(_dbCtx);
            this.Depts=new DeptRepo(_dbCtx);
            this.Job=new JobRepo(_dbCtx);
            this.Shift=new ShiftRepo(_dbCtx);
            this.EmpDept=new EmpDeptRepo(_dbCtx);
            this.EmpJob=new EmpJobRepo(_dbCtx);
            this.EmpShift=new EmpShiftRepo(_dbCtx);
            #endregion
        }

        public int Commit() => _dbCtx.SaveChanges();

        public void Dispose() => _dbCtx.Dispose();

    }
}
