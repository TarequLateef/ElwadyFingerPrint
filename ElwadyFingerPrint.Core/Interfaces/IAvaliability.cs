using ElwadyFingerPrint.Core.Interfaces;
using EmpSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagment.core.Interfaces;

namespace IunitWork
{
    public interface IAvaliability<T>:IUsingRepository<T>,IlistRepository<T> where T: class
    {
        Task<IEnumerable<T>> AvailableData();
        Task<IEnumerable<T>> AvailableData(DateTime _date);
        Task<IEnumerable<T>> NotAvailableData();
        Task<IEnumerable<T>> NotAvailableData(DateTime _date);

        IEnumerable<T> AvailableData(IEnumerable<T> _list);
        IEnumerable<T> AvailableData(IEnumerable<T> _list,DateTime _date);
        IEnumerable<T> NotAvailableData(IEnumerable<T> _list);
        IEnumerable<T> NotAvailableData(IEnumerable<T> _list, DateTime _date);

        bool Deletable(T entity);

        Task<T> StopElement(T entity);
        Task<bool> StopElement(T entity, DateTime stopDate);

        Task<T> RestoreElement(T entity);
        Task<bool> RestoreElement(T entity, DateTime restoreDate);
    }
}
