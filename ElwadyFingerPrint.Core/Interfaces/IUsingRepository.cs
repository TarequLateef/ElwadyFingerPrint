using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserManagment.core.Interfaces
{
    public interface IUsingRepository<T> where T : class
    {
        #region Basic Operation
        Task<T> GetByID(int id);
        Task<T> GetBytID(string id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        void Update(T entity);
        Task<T> Update(T entity, int id);
        void Delete(T entity);

        #endregion

        #region Search Context
        Task<IEnumerable<T>> Search(Expression<Func<T, bool>> Criteria);
        Task<IEnumerable<T>> Search(string[] includes);
        Task<IEnumerable<T>> Search(int pageNo=1);
        Task<IEnumerable<T>> Search(Expression<Func<T, object>> orderByField, string orderBy = "ACS");
        Task<IEnumerable<T>> Search(Expression<Func<T, bool>> Criteria,string[] includes);
        Task<IEnumerable<T>> Search(Expression<Func<T, bool>> Criteria, string[] includes, int pageNo=1);
        Task<IEnumerable<T>> Search(Expression<Func<T, bool>> Criteria, string[] includes, Expression<Func<T, object>> orderByField, string orderBy = "ACS", int pageNo = 1);

        Task<bool> Repeated(Expression<Func<T, bool>> Criteria);
        bool Repeated(Expression<Func<T, bool>> Criteria,out T repeatedItem);

        #endregion


    }

    public class OrderingBy
    {
        public static string Ascending = "ASC";
        public static string Desending = "DESC";
    }
}
