using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElwadyFingerPrint.Core.Interfaces
{
    public interface IlistRepository<T> where T : class
    {
        #region Search dataList
        IEnumerable<T> Search(IEnumerable<T> dataList, Expression<Func<T, bool>> Criteria);
        IEnumerable<T> Search(IEnumerable<T> dataList, int pageNo = 1);
        IEnumerable<T> Search(IEnumerable<T> dataList, Expression<Func<T, object>> orderByField, string orderBy = "ACS");
        IEnumerable<T> Search(IEnumerable<T> dataList, Expression<Func<T, bool>> Criteria, int pageNo = 1);

        IEnumerable<T> Search(IEnumerable<T> dataList, Expression<Func<T, bool>> Criteria, Expression<Func<T, object>> orderByField, string orderBy = "ACS", int pageNo = 1);

        bool Repeated(IEnumerable<T> dataList, Expression<Func<T, bool>> Criteria);
        bool Repeated(IEnumerable<T> dataList, Expression<Func<T, bool>> Criteria, out T repeatedItem);
        #endregion

    }
}
