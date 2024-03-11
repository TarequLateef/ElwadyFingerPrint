using ElwadyFingerPrint.EF.Repositories;
using HrCodeFirstDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using UserManagment.core.Interfaces;

namespace UserManagment.EF.Repository
{
    public class UsingRepository<T> :ListRepository<T>, IUsingRepository<T> where T : class
    {
        protected Hr_Db_CFContext _ctx;
        public UsingRepository(Hr_Db_CFContext context) => _ctx = context;
        #region Basic Operation
        public async Task<T> Add(T entity)
        {
            await _ctx.Set<T>().AddAsync(entity);
            return entity;
        }

        public void Update(T entity) =>
            _ctx.Entry(entity).State=EntityState.Modified;

        public async Task<T> Update(T entity, int id)
        {
            _ctx.Entry(entity).State=EntityState.Modified;
            return await this.GetByID(id);
        }

        public void Delete(T entity)
        {
            _ctx.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var result = await _ctx.Set<T>().ToListAsync();
            return result ==null || result.Count==0 ? null : result;
        }

        public async Task<T> GetByID(int id) => await _ctx.Set<T>().FindAsync(id);

        public async Task<T> GetBytID(string id) => await _ctx.Set<T>().FindAsync(id);

        #endregion Basci Operation

        #region Search Context
        public async Task<IEnumerable<T>> Search(Expression<Func<T, bool>> Criteria) =>
            await _ctx.Set<T>().Where(Criteria).ToListAsync();

        public async Task<bool> Repeated(Expression<Func<T, bool>> Criteria)
        {
            var list = await Search(Criteria);
            return list.Any();
        }

        public bool Repeated(Expression<Func<T, bool>> Criteria, out T repeatedItem)
        {
            var list = Search(Criteria).Result;
            repeatedItem=list.Any() ? list.FirstOrDefault() : null;
            return list.Any();
        }
        public async Task<IEnumerable<T>> Search(string[] includes)
        {
            IQueryable<T> query = _ctx.Set<T>();
            foreach (var item in includes) query=query.Include(item);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> Search(int pageNo = 1)
        {
            IQueryable<T> dataList = _ctx.Set<T>();
            dataList = pageNo!=1 ? dataList.Count() < pageNo * 10 ?
             dataList.Skip((pageNo - 1) * 10).Take(dataList.Count() - (pageNo - 1))
                     : dataList.Skip((pageNo - 1) * 10).Take(10)
                    : dataList.Take(10);
            return await dataList.ToListAsync();
        }

        public async Task<IEnumerable<T>> Search(Expression<Func<T, object>> orderByField, string orderBy = "ACS")
        {
            IQueryable<T> dataList = _ctx.Set<T>();
            dataList=orderBy==OrderingBy.Ascending ? dataList.OrderBy(orderByField)
                : dataList.OrderByDescending(orderByField);
            return await dataList.ToListAsync();
        }

        public async Task<IEnumerable<T>> Search(Expression<Func<T, bool>> Criteria, string[] includes)
        {
            IQueryable<T> dataList = _ctx.Set<T>().Where(Criteria);
            foreach (var item in includes) dataList=dataList.Include(item);
            return await dataList.ToListAsync();
        }

        public async Task<IEnumerable<T>> Search(Expression<Func<T, bool>> Criteria, string[] includes, int pageNo = 1)
        {
            IQueryable<T> dList = _ctx.Set<T>().Where(Criteria);
            foreach (var item in includes) dList=dList.Include(item);
            dList = pageNo!=1 ? dList.Count() < pageNo * 10 ?
             dList.Skip((pageNo - 1) * 10).Take(dList.Count() - (pageNo - 1))
                     : dList.Skip((pageNo - 1) * 10).Take(10)
                    : dList.Take(10);
            return await dList.ToListAsync();
        }

        public async Task<IEnumerable<T>> Search(Expression<Func<T, bool>> Criteria, string[] includes, Expression<Func<T, object>> orderByField, string orderBy = "ACS", int pageNo = 1)
        {
            IQueryable<T> dList = _ctx.Set<T>().Where(Criteria);
            foreach (var item in includes) dList=dList.Include(item);
            dList=orderBy==OrderingBy.Ascending ? dList.OrderBy(orderByField) 
                : dList.OrderByDescending(orderByField);
            dList = pageNo!=1 ? dList.Count() < pageNo * 10 ?
             dList.Skip((pageNo - 1) * 10).Take(dList.Count() - (pageNo - 1))
                     : dList.Skip((pageNo - 1) * 10).Take(10)
                    : dList.Take(10);
            return await dList.ToListAsync();
        }
        #endregion

    }
}
