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
    public class ShiftRepo:UsingRepository<Shifts>,IShift
    {
        protected Hr_Db_CFContext _ctx;
        public ShiftRepo(Hr_Db_CFContext ctx):base(ctx)=>_ctx= ctx;

        public async Task<IEnumerable<Shifts>> AvailableData() =>
           await this.Search(e => !e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value<=DateTime.Now));

        public IEnumerable<Shifts> AvailableData(IEnumerable<Shifts> _list) =>
            _list.Where(e => e.Avaliable).ToList();

        public IEnumerable<Shifts> AvailableData(IEnumerable<Shifts> _list, DateTime _date) =>
            _list.Where(e => e.Avaliable || (e.EndDate.HasValue && e.EndDate.Value>=_date));

        public async Task<IEnumerable<Shifts>> AvailableData(DateTime _date) =>
            await this.Search(e => !e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value>=_date));

        public bool Deletable(Shifts employees) =>
            !_ctx.EmpShifts.Any(ed => ed.ShiftID==employees.ShiftID);
        public IEnumerable<Shifts> NotAvailableData(IEnumerable<Shifts> _list) =>
            _list.Where(e => e.Avaliable);
        public IEnumerable<Shifts> NotAvailableData(IEnumerable<Shifts> _list, DateTime _date) =>
            _list.Where(e => !e.Avaliable || (e.EndDate.HasValue && e.EndDate.Value<_date));

        public async Task<IEnumerable<Shifts>> NotAvailableData() =>
            await this.Search(e => e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value<DateTime.Now));

        public async Task<IEnumerable<Shifts>> NotAvailableData(DateTime _date) =>
            await this.Search(e => e.EndDate.HasValue || (e.EndDate.HasValue && e.EndDate.Value<_date));

        public async Task<Shifts> RestoreElement(Shifts entity)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=null;
                entity= await this.Update(entity, entity.ShiftID);
            }
            return entity;
        }

        public async Task<bool> RestoreElement(Shifts entity, DateTime restoreDate)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=restoreDate;
                entity= await this.Update(entity, entity.ShiftID);
            }
            return !entity.EndDate.HasValue;
        }

        public async Task<Shifts> StopElement(Shifts entity)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=DateTime.Now;
                entity=await this.Update(entity, entity.ShiftID);
            }
            return entity;
        }

        public async Task<bool> StopElement(Shifts entity, DateTime stopDate)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=stopDate;
                entity=await this.Update(entity, entity.ShiftID);
            }
            return entity.EndDate.HasValue;
        }
    }
}