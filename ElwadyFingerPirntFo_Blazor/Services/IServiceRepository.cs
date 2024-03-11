using System.Reflection.Emit;

namespace ElwadyFingerPirntFo_Blazor.Services
{
    public interface IServiceRepository<T> where T : class
    {
        Task<IList<T>> GetDataList(string actionUrl, bool? aval = null);

        Task<T> GetData(string actionUrl, string itemID);
        Task<HttpResponseMessage> AddItem(string actionUrl, T item);
        Task<HttpResponseMessage> UpdateItem(string actionUrl, T item);
        Task<T?> DeleteItem(string actionUrl,T item);

    }
}

