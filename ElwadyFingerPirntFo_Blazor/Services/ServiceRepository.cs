using System.Data;
using System.Linq.Expressions;
using System.Net.Http.Json;
using TarequComponets.TableComp;

namespace ElwadyFingerPirntFo_Blazor.Services
{
    public class ServiceRepository<T> : IServiceRepository<T> where T : class
    {
        protected readonly HttpClient _httpClient;
        public IList<SearchTable> SearchArr = new List<SearchTable>();
        public IList<T> dataList = new List<T>();
        public IList<T> dumyList = new List<T>();
        public IList<T> searchList = new List<T>();
        public T item { get; set; }
        public bool _aval { get; set; } = true;
        public string ControllerUrl { get; set; }

        public ServiceRepository()
        {
            _httpClient=new HttpClient();
            _httpClient.BaseAddress=new Uri("https://localhost:7232/api/");
            /*https://24.24.24.252:903*/
        }

        public async Task<IList<T>> GetDataList(string actionUrl, bool? aval = null)
        {
            IList<T> list = aval is not null ?
                 await _httpClient.GetFromJsonAsync<List<T>>(actionUrl+"?aval="+aval.Value.ToString().ToLower())
                 : await _httpClient.GetFromJsonAsync<List<T>>(actionUrl);
            return list;
        }

        public void ManageSearchArr(SearchTable st)
        {
            bool done = false;
            if (!this.SearchArr.Any()) SearchArr.Add(st);
            else
            {
                while (!done)
                {
                    foreach (var item in SearchArr)
                    {
                        if (item.FieldName==st.FieldName)
                        {
                            if (string.IsNullOrEmpty(st.SearchValue))
                            {
                                SearchArr.Remove(item);
                                done=true; break;
                            }
                            item.SearchValue=st.SearchValue;
                            item.Condition=st.Condition;
                            done=true; break;
                        }
                    }
                    if (!done) { SearchArr.Add(st); done=true; }
                }
            }
        }

        public async Task<T> GetData(string actionUrl, string itemID)
        {
            return await _httpClient.GetFromJsonAsync<T>(actionUrl+"?id="+itemID);
        }

        public async Task<HttpResponseMessage> AddItem(string actionUrl, T item) =>
            await _httpClient.PostAsJsonAsync<T>(actionUrl, item);

        public async Task<HttpResponseMessage> UpdateItem(string actionUrl, T item) =>
            await _httpClient.PutAsJsonAsync<T>(actionUrl, item);


        public async Task<T?> DeleteItem(string actionUrl, T item) =>
            await _httpClient.DeleteFromJsonAsync<T>(actionUrl);

    }
}
