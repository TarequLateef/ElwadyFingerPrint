using ElwadyFingerPrint.Core.Interfaces.Emp;
using EmpSchema;
using HrCodeFirstDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserManagment.core.Interfaces;
using UserManagment.EF.Repository;

namespace ElwadyFingerPrint.EF.Repositories.Emp
{
    public class JobRepo : UsingRepository<Jobs>, IJobs
    {
        protected Hr_Db_CFContext _cfContex;
        public JobRepo(Hr_Db_CFContext cfContext):base(cfContext)=>_cfContex=cfContext;

        public async Task<IEnumerable<Jobs>> AvailableData() =>
           await this.Search(e => !e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value<=DateTime.Now));

        public IEnumerable<Jobs> AvailableData(IEnumerable<Jobs> _list) =>
            _list.Where(e => e.Avaliable).ToList();

        public IEnumerable<Jobs> AvailableData(IEnumerable<Jobs> _list, DateTime _date) =>
            _list.Where(e => e.Avaliable || (e.EndDate.HasValue && e.EndDate.Value>=_date));

        public async Task<IEnumerable<Jobs>> AvailableData(DateTime _date) =>
            await this.Search(e => !e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value>=_date));

        public bool Deletable(Jobs employees) =>
            !_ctx.EmpJobs.Any(ed => ed.JobID==employees.JobID);
        public IEnumerable<Jobs> NotAvailableData(IEnumerable<Jobs> _list) =>
            _list.Where(e => e.Avaliable);
        public IEnumerable<Jobs> NotAvailableData(IEnumerable<Jobs> _list, DateTime _date) =>
            _list.Where(e => !e.Avaliable || (e.EndDate.HasValue && e.EndDate.Value<_date));

        public async Task<IEnumerable<Jobs>> NotAvailableData() =>
            await this.Search(e => e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value<DateTime.Now));

        public async Task<IEnumerable<Jobs>> NotAvailableData(DateTime _date) =>
            await this.Search(e => e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value<_date));

        public async Task<Jobs> RestoreElement(Jobs entity)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=null;
                entity= await this.Update(entity, entity.JobID);
            }
            return entity;
        }

        public async Task<bool> RestoreElement(Jobs entity, DateTime restoreDate)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=restoreDate;
                entity= await this.Update(entity, entity.JobID);
            }
            return !entity.EndDate.HasValue;
        }

        public async Task<Jobs> StopElement(Jobs entity)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=DateTime.Now;
                entity=await this.Update(entity, entity.JobID);
            }
            return entity;
        }

        public async Task<bool> StopElement(Jobs entity, DateTime stopDate)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=stopDate;
                entity=await this.Update(entity, entity.JobID);
            }
            return entity.EndDate.HasValue;
        }
    }
}