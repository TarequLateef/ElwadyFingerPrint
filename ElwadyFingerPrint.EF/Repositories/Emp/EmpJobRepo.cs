using ElwadyFingerPrint.Core.Interfaces.Emp;
using EmpSchema;
using HrCodeFirstDB;
using IunitWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagment.core.Interfaces;
using UserManagment.EF.Repository;

namespace ElwadyFingerPrint.EF.Repositories.Emp
{
    public class EmpJobRepo : UsingRepository<EmpJob>, IEmpJob
    {
        protected Hr_Db_CFContext _ctx;
        public EmpJobRepo(Hr_Db_CFContext ctx) : base(ctx) => _ctx = ctx;

        public async Task<IEnumerable<EmpJob>> AvailableData() =>
           await this.Search(c => !c.EndDate.HasValue || (c.EndDate.HasValue && c.EndDate<=DateTime.Now),
               new[] { "Employees", "Jobs" });
        public async Task<IEnumerable<EmpJob>> AvailableData(DateTime _date) =>
           await this.Search(c => !c.EndDate.HasValue || (c.EndDate.HasValue && c.EndDate.Value < _date),
               new[] { "Employees", "Jobs" });
        public IEnumerable<EmpJob> AvailableData(IEnumerable<EmpJob> _list) =>
            _list.Where(c => !c.EndDate.HasValue);
        public IEnumerable<EmpJob> AvailableData(IEnumerable<EmpJob> _list, DateTime _date) =>
            _list.Where(c => !c.EndDate.HasValue || (c.EndDate.HasValue && c.EndDate.Value<_date));

        public bool Deletable(EmpJob entity) =>
            !_ctx.Employees.Any(e => e.CertID==entity.EmpJobID);

        public async Task<IEnumerable<EmpJob>> NotAvailableData() =>
           await this.Search(c => c.EndDate.HasValue || c.EndDate.Value>DateTime.Now,
               new[] { "Employees", "Jobs" });
        public async Task<IEnumerable<EmpJob>> NotAvailableData(DateTime _date) =>
           await this.Search(c => !c.EndDate.HasValue || (c.EndDate.HasValue && c.EndDate.Value < _date),
               new[] { "Employees", "Jobs" });

        public IEnumerable<EmpJob> NotAvailableData(IEnumerable<EmpJob> _list) =>
            _list.Where(c => c.EndDate.HasValue && c.EndDate.Value>=DateTime.Now);

        public IEnumerable<EmpJob> NotAvailableData(IEnumerable<EmpJob> _list, DateTime _date)
            => _list.Where(c => c.EndDate.HasValue && c.EndDate.Value>=_date);

        public async Task<EmpJob> RestoreElement(EmpJob entity)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=null;
                entity= await this.Update(entity, entity.EmpJobID);
            }
            return entity;
        }

        public async Task<bool> RestoreElement(EmpJob entity, DateTime restoreDate)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=restoreDate;
                entity= await this.Update(entity, entity.EmpJobID);
            }
            return !entity.EndDate.HasValue;
        }

        public async Task<EmpJob> StopElement(EmpJob entity)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=DateTime.Now;
                entity=await this.Update(entity, entity.EmpJobID);
            }
            return entity;
        }

        public async Task<bool> StopElement(EmpJob entity, DateTime stopDate)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=stopDate;
                entity=await this.Update(entity, entity.EmpJobID);
            }
            return entity.EndDate.HasValue;
        }
    }
}