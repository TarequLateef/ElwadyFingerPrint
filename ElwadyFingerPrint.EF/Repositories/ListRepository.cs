using ElwadyFingerPrint.Core.Interfaces;
using ElwadySales.Core.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElwadyFingerPrint.EF.Repositories
{
    public class ListRepository<T>:IlistRepository<T> where T : class
    {
        #region DataList Operations
        public bool Repeated(IEnumerable<T> dataList, Expression<Func<T, bool>> Criteria)
        {
            IQueryable<T> dList = dataList.AsQueryable();
            return dList.Any(Criteria);
        }

        public bool Repeated(IEnumerable<T> dataList, Expression<Func<T, bool>> Criteria, out T repeatedItem)
        {
            IQueryable<T> dList = dataList.AsQueryable();
            repeatedItem = dList.Where(Criteria).FirstOrDefault();
            return dList.Any();
        }

        public IEnumerable<T> Search(IEnumerable<T> dataList, Expression<Func<T, bool>> Criteria)
        {
            IQueryable<T> dList = dataList.AsQueryable();
            dList = dList.Where(Criteria);
            return dList.ToList();
        }

        public IEnumerable<T> Search(IEnumerable<T> dataList, int pageNo = 1)
        {
            IQueryable<T> dList = dataList.AsQueryable();
            dList = pageNo!=1 ? dList.Count() < pageNo * 10 ?
                dList.Skip((pageNo - 1) * 10).Take(dList.Count() - (pageNo - 1))
                : dList.Skip((pageNo - 1) * 10).Take(10)
                : dList.Take(10);
            return dList;
        }

        public IEnumerable<T> Search(IEnumerable<T> dataList, Expression<Func<T, object>> orderByField, string orderBy = "ACS")
        {
            IQueryable<T> dList = dataList.AsQueryable();
            return orderBy==OrderingBy.Ascending ?
                dList.OrderBy(orderByField)
                : dList.OrderByDescending(orderByField);
        }


        public IEnumerable<T> Search(IEnumerable<T> dataList, Expression<Func<T, bool>> Criteria, int pageNo = 1)
        {
            IQueryable<T> dList = dataList.AsQueryable();
            dList= Search(dList, Criteria).AsQueryable();
            dList=Search(dList, pageNo).AsQueryable();
            return dList;
        }
        public IEnumerable<T> Search(IEnumerable<T> dataList, Expression<Func<T, bool>> Criteria, Expression<Func<T, object>> orderByField, string orderBy = "ACS", int pageNo = 1)
        {
            IQueryable<T> dList = dataList.AsQueryable();
            dList=Search(dList, Criteria, pageNo).AsQueryable();
            dList = Search(dList, orderByField, orderBy).AsQueryable();
            return dList;
        }

        #endregion

    }
}
