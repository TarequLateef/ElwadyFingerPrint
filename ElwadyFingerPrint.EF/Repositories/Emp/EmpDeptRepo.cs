using ElwadyFingerPrint.Core.Interfaces.Emp;
using EmpSchema;
using HrCodeFirstDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UserManagment.EF.Repository;

namespace ElwadyFingerPrint.EF.Repositories.Emp
{
    public class EmpDeptRepo : UsingRepository<EmpDept>, IEmpDept
    {
        protected Hr_Db_CFContext _ctx;
        public EmpDeptRepo(Hr_Db_CFContext ctx) : base(ctx) => _ctx=ctx;

        public async Task<IEnumerable<EmpDept>> AvailableData() =>
           await this.Search(e => !e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value<=DateTime.Now), new[] { "Employees", "Department" });

        public IEnumerable<EmpDept> AvailableData(IEnumerable<EmpDept> _list) =>
            _list.Where(e => e.Avaliable).ToList();

        public IEnumerable<EmpDept> AvailableData(IEnumerable<EmpDept> _list, DateTime _date) =>
            _list.Where(e => e.Avaliable || (e.EndDate.HasValue && e.EndDate.Value>=_date));

        public async Task<IEnumerable<EmpDept>> AvailableData(DateTime _date) =>
            await this.Search(e => !e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value>=_date), new[] { "Employees", "Department" });

        public bool Deletable(EmpDept dept) =>
            !_ctx.EmpDepts.Any(ed => ed.DeptID==dept.DeptID);
        public IEnumerable<EmpDept> NotAvailableData(IEnumerable<EmpDept> _list) =>
            _list.Where(e => e.Avaliable);
        public IEnumerable<EmpDept> NotAvailableData(IEnumerable<EmpDept> _list, DateTime _date) =>
            _list.Where(e => !e.Avaliable || (e.EndDate.HasValue && e.EndDate.Value<_date));

        public async Task<IEnumerable<EmpDept>> NotAvailableData() =>
            await this.Search(e => e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value<DateTime.Now), new[] { "Employees", "Department" });

        public async Task<IEnumerable<EmpDept>> NotAvailableData(DateTime _date) =>
            await this.Search(e => e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value<_date));

        public async Task<EmpDept> RestoreElement(EmpDept entity)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=null;
                entity= await this.Update(entity, entity.EmpDeptID);
            }
            return entity;
        }

        public async Task<bool> RestoreElement(EmpDept entity, DateTime restoreDate)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=restoreDate;
                entity= await this.Update(entity, entity.EmpDeptID);
            }
            return !entity.EndDate.HasValue;
        }

        public async Task<EmpDept> StopElement(EmpDept entity)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=DateTime.Now;
                entity=await this.Update(entity, entity.EmpDeptID);
            }
            return entity;
        }

        public async Task<bool> StopElement(EmpDept entity, DateTime stopDate)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=stopDate;
                entity=await this.Update(entity, entity.EmpDeptID);
            }
            return entity.EndDate.HasValue;
        }
    }
}