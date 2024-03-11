using ElwadyFingerPrint.Core.Interfaces.Emp;
using EmpSchema;
using HrCodeFirstDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagment.EF.Repository;

namespace ElwadyFingerPrint.EF.Repositories.Emp
{
    public class EmpShiftRepo:UsingRepository<EmpShift>,IEmpShift
    {
        protected Hr_Db_CFContext _ctx;
        public EmpShiftRepo(Hr_Db_CFContext ctx):base(ctx) => _ctx = ctx;

        public async Task<IEnumerable<EmpShift>> AvailableData() =>
           await this.Search(c => !c.EndDate.HasValue || (c.EndDate.HasValue && c.EndDate<=DateTime.Now),
               new[] { "Employees", "Shifts" });
        public async Task<IEnumerable<EmpShift>> AvailableData(DateTime _date) =>
           await this.Search(c => !c.EndDate.HasValue || (c.EndDate.HasValue && c.EndDate.Value < _date),
               new[] { "Employees", "Shifts" });
        public IEnumerable<EmpShift> AvailableData(IEnumerable<EmpShift> _list) =>
            _list.Where(c => !c.EndDate.HasValue);
        public IEnumerable<EmpShift> AvailableData(IEnumerable<EmpShift> _list, DateTime _date) =>
            _list.Where(c => !c.EndDate.HasValue || (c.EndDate.HasValue && c.EndDate.Value<_date));

        public bool Deletable(EmpShift entity) =>
            !_ctx.Employees.Any(e => e.CertID==entity.EmpShiftID);

        public async Task<IEnumerable<EmpShift>> NotAvailableData() =>
           await this.Search(c => c.EndDate.HasValue || c.EndDate.Value>DateTime.Now,
               new[] { "Employees", "Shifts" });
        public async Task<IEnumerable<EmpShift>> NotAvailableData(DateTime _date) =>
           await this.Search(c => !c.EndDate.HasValue || (c.EndDate.HasValue && c.EndDate.Value < _date),
               new[] { "Employees", "Shifts" });

        public IEnumerable<EmpShift> NotAvailableData(IEnumerable<EmpShift> _list) =>
            _list.Where(c => c.EndDate.HasValue && c.EndDate.Value>=DateTime.Now);

        public IEnumerable<EmpShift> NotAvailableData(IEnumerable<EmpShift> _list, DateTime _date)
            => _list.Where(c => c.EndDate.HasValue && c.EndDate.Value>=_date);

        public async Task<EmpShift> RestoreElement(EmpShift entity)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=null;
                entity= await this.Update(entity, entity.EmpShiftID);
            }
            return entity;
        }

        public async Task<bool> RestoreElement(EmpShift entity, DateTime restoreDate)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=restoreDate;
                entity= await this.Update(entity, entity.EmpShiftID);
            }
            return !entity.EndDate.HasValue;
        }

        public async Task<EmpShift> StopElement(EmpShift entity)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=DateTime.Now;
                entity=await this.Update(entity, entity.EmpShiftID);
            }
            return entity;
        }

        public async Task<bool> StopElement(EmpShift entity, DateTime stopDate)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=stopDate;
                entity=await this.Update(entity, entity.EmpShiftID);
            }
            return entity.EndDate.HasValue;
        }
    }
}