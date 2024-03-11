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
    public class DeptRepo:UsingRepository<Department>,IDepts
    {
        protected Hr_Db_CFContext _ctx;
        public DeptRepo(Hr_Db_CFContext ctx):base(ctx)=> _ctx= ctx;

        public async Task<IEnumerable<Department>> AvailableData() =>
           await this.Search(e => !e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value<=DateTime.Now));

        public IEnumerable<Department> AvailableData(IEnumerable<Department> _list) =>
            _list.Where(e => e.Avaliable).ToList();

        public IEnumerable<Department> AvailableData(IEnumerable<Department> _list, DateTime _date) =>
            _list.Where(e => e.Avaliable || (e.EndDate.HasValue && e.EndDate.Value>=_date));

        public async Task<IEnumerable<Department>> AvailableData(DateTime _date) =>
            await this.Search(e => !e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value>=_date));

        public bool Deletable(Department dept) =>
            !_ctx.EmpDepts.Any(ed => ed.DeptID==dept.DeptID);
        public IEnumerable<Department> NotAvailableData(IEnumerable<Department> _list) =>
            _list.Where(e => e.Avaliable);
        public IEnumerable<Department> NotAvailableData(IEnumerable<Department> _list, DateTime _date) =>
            _list.Where(e => !e.Avaliable || (e.EndDate.HasValue && e.EndDate.Value<_date));

        public async Task<IEnumerable<Department>> NotAvailableData() =>
            await this.Search(e => e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value<DateTime.Now));

        public async Task<IEnumerable<Department>> NotAvailableData(DateTime _date) =>
            await this.Search(e => e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value<_date));
        public async Task<Department> RestoreElement(Department entity)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=null;
                entity= await this.Update(entity, entity.DeptID);
            }
            return entity;
        }

        public async Task<bool> RestoreElement(Department entity, DateTime restoreDate)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=restoreDate;
                entity= await this.Update(entity, entity.DeptID);
            }
            return !entity.EndDate.HasValue;
        }

        public async Task<Department> StopElement(Department entity)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=DateTime.Now;
                entity=await this.Update(entity, entity.DeptID);
            }
            return entity;
        }

        public async Task<bool> StopElement(Department entity, DateTime stopDate)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=stopDate;
                entity=await this.Update(entity, entity.DeptID);
            }
            return entity.EndDate.HasValue;
        }
    }
}