using ElwadyFingerPrint.Core.Interfaces;
using ElwadyFingerPrint.Core.Interfaces.Emp;
using ElwadyFingerPrint.Core.Models.EmpSchema;
using EmpSchema;
using HrCodeFirstDB;
using IunitWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagment.EF.Repository;

namespace ElwadyFingerPrint.EF.Repositories.Emp
{
    public class CertRepo : UsingRepository<CertificateTbl>, ICertified
    {
        protected Hr_Db_CFContext _ctx;
        public CertRepo(Hr_Db_CFContext context) : base(context) =>
            _ctx=context;

        public async Task<IEnumerable<CertificateTbl>> AvailableData() =>
           await this.Search(c => !c.EndDate.HasValue || (c.EndDate.HasValue && c.EndDate<=DateTime.Now));
        public async Task<IEnumerable<CertificateTbl>> AvailableData(DateTime _date) =>
           await this.Search(c => !c.EndDate.HasValue || (c.EndDate.HasValue && c.EndDate.Value < _date));
        public IEnumerable<CertificateTbl> AvailableData(IEnumerable<CertificateTbl> _list) =>
            _list.Where(c => !c.EndDate.HasValue);
        public IEnumerable<CertificateTbl> AvailableData(IEnumerable<CertificateTbl> _list, DateTime _date) =>
            _list.Where(c => !c.EndDate.HasValue || (c.EndDate.HasValue && c.EndDate.Value<_date));

        public bool Deletable(CertificateTbl entity) =>
            !_ctx.Employees.Any(e => e.CertID==entity.CertID);
            
        public async Task<IEnumerable<CertificateTbl>> NotAvailableData() =>
           await this.Search(c => c.EndDate.HasValue || c.EndDate.Value>DateTime.Now);
        public async Task<IEnumerable<CertificateTbl>> NotAvailableData(DateTime _date) =>
           await this.Search(c => !c.EndDate.HasValue || (c.EndDate.HasValue && c.EndDate.Value < _date));

        public IEnumerable<CertificateTbl> NotAvailableData(IEnumerable<CertificateTbl> _list) =>
            _list.Where(c => c.EndDate.HasValue && c.EndDate.Value>=DateTime.Now);

        public IEnumerable<CertificateTbl> NotAvailableData(IEnumerable<CertificateTbl> _list, DateTime _date)
            => _list.Where(c => c.EndDate.HasValue && c.EndDate.Value>=_date);
 
        public async Task<CertificateTbl> RestoreElement(CertificateTbl entity)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=null;
                entity= await this.Update(entity, entity.CertID);
            }
            return entity;
        }

        public async Task<bool> RestoreElement(CertificateTbl entity, DateTime restoreDate)
        {
            if (entity.EndDate.HasValue)
            {
                entity.EndDate=restoreDate;
                entity= await this.Update(entity, entity.CertID);
            }
            return !entity.EndDate.HasValue;
        }

        public async Task<CertificateTbl> StopElement(CertificateTbl entity)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=DateTime.Now;
                entity=await this.Update(entity, entity.CertID);
            }
            return entity;
        }

        public async Task<bool> StopElement(CertificateTbl entity, DateTime stopDate)
        {
            if (!entity.EndDate.HasValue)
            {
                entity.EndDate=stopDate;
                entity=await this.Update(entity, entity.CertID);
            }
            return entity.EndDate.HasValue;
        }
    }
}