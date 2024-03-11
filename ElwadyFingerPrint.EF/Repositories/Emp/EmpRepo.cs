using ElwadyFingerPrint.Core.DTOs.Emp;
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
    public class EmpRepo : UsingRepository<Employees>, IEmployee
    {
        protected Hr_Db_CFContext _ctx;
        public EmpRepo(Hr_Db_CFContext ctx) : base(ctx) =>
            _ctx=ctx;

        public async Task<IEnumerable<Employees>> AvailableData() =>
           await this.Search(e => !e.EndDate.HasValue, new[] { "Cert" });

        public IEnumerable<Employees> AvailableData(IEnumerable<Employees> _list) =>
            _list.Where(e => e.Avaliable).ToList();

        public IEnumerable<Employees> AvailableData(IEnumerable<Employees> _list, DateTime _date) =>
            _list.Where(e => e.Avaliable || (e.EndDate.HasValue && e.EndDate.Value>=_date));

        public async Task<IEnumerable<Employees>> AvailableData(DateTime _date) =>
            await this.Search(e => !e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value>=_date));

        public bool Deletable(Employees employees) =>
            !_ctx.EmpDepts.Any(ed => ed.Employees.EmpID==employees.EmpID);
        public IEnumerable<Employees> NotAvailableData(IEnumerable<Employees> _list) =>
            _list.Where(e => e.Avaliable);
        public IEnumerable<Employees> NotAvailableData(IEnumerable<Employees> _list, DateTime _date) =>
            _list.Where(e => !e.Avaliable || (e.EndDate.HasValue && e.EndDate.Value<_date));

        public async Task<IEnumerable<Employees>> NotAvailableData() =>
            await this.Search(e => e.EndDate.HasValue, new[] { "Cert" });

        public async Task<IEnumerable<Employees>> NotAvailableData(DateTime _date) =>
            await this.Search(e => e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value<_date));

        public async Task<Employees> RestoreElement(Employees entity)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=null;
                entity= await this.Update(entity, entity.EmpID);
            }
            return entity;
        }

        public async Task<bool> RestoreElement(Employees entity, DateTime restoreDate)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=restoreDate;
                entity= await this.Update(entity, entity.EmpID);
            }
            return !entity.EndDate.HasValue;
        }

        public async Task<Employees> StopElement(Employees entity)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=DateTime.Now;
                entity=await this.Update(entity, entity.EmpID);
            }
            return entity;
        }

        public async Task<bool> StopElement(Employees entity, DateTime stopDate)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=stopDate;
                entity=await this.Update(entity, entity.EmpID);
            }
            return entity.EndDate.HasValue;
        }
    }
}