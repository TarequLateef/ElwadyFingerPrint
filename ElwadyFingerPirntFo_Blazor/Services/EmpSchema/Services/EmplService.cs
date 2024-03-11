using ElwadyFingerPirntFo_Blazor.Pages.Emp;
using ElwadyFingerPirntFo_Blazor.Services.EmpSchema.Interfaces;
using ElwadyFingerPrint.Core.Interfaces.Emp;
using EmpSchema;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using TarequComponets.TableComp;
using static System.Net.WebRequestMethods;

namespace ElwadyFingerPirntFo_Blazor.Services.EmpSchema.Services
{
    public class EmplService : ServiceRepository<Employees>, IEmpService
    {
        public int pagesCount = 0; public bool searching = false;
        public EmplService() => this.ControllerUrl=_httpClient.BaseAddress+ "Employees/";


        public IEnumerable<Employees> Searching()
        {
            dataList= dumyList;
            searching=false;
            foreach (var item in SearchArr)
            {
                if (item.FieldName == "name")
                    dataList= string.IsNullOrEmpty(item.SearchValue) ? dataList
                    : item.Condition == Condition.Equal ?
                    dataList.Where(e => e.EmpName == item.SearchValue && e.Avaliable == _aval).ToList()
                        : dataList.Where(e => e.EmpName.Contains(item.SearchValue) && e.Avaliable == _aval).ToList();

                if (item.FieldName == "startDate")
                    dataList= string.IsNullOrEmpty(item.SearchValue) ?
                        dataList
                    : item.Condition == Condition.Equal ?
                        dataList.Where(e => e.StartDate.ToString("yyyy-MM-dd") == item.SearchValue && e.Avaliable == _aval).ToList()
                        : dataList.Where(e => e.StartDate.ToString().Contains(item.SearchValue) && e.Avaliable == _aval).ToList();
                if (item.FieldName == "address")
                    dataList= string.IsNullOrEmpty(item.SearchValue) ? dataList
                    : item.Condition == Condition.Equal ?
                        dataList.Where(e => !string.IsNullOrEmpty(e.Address) && e.Address == item.SearchValue && e.Avaliable == _aval).ToList()
                        : dataList.Where(e => !string.IsNullOrEmpty(e.Address) && e.Address.Contains(item.SearchValue) && e.Avaliable == _aval).ToList();
                if (item.FieldName == "insuranceNo")
                    dataList= string.IsNullOrEmpty(item.SearchValue) ? dataList
                    : item.Condition == Condition.Equal ?
                            dataList.Where(e => e.InsuranceNo == item.SearchValue && e.Avaliable == _aval).ToList()
                            : dataList.Where(e => e.InsuranceNo.Contains(item.SearchValue) && e.Avaliable == _aval).ToList();
                if (item.FieldName == "insuranceDate")
                    dataList= string.IsNullOrEmpty(item.SearchValue) ? dataList
                    : item.Condition == Condition.Equal ?
                            dataList.Where(e => e.InsuranceDate.HasValue && e.InsuranceDate.Value.ToString("yyyy-MM-dd") == item.SearchValue && e.Avaliable == _aval).ToList()
                        : dataList.Where(e => e.InsuranceDate.HasValue && e.InsuranceDate.Value.ToString().Contains(item.SearchValue) && e.Avaliable == _aval).ToList();
                if (item.FieldName == "cert")
                    dataList= string.IsNullOrEmpty(item.SearchValue) ? dataList
                    : item.Condition == Condition.Equal ?
                            dataList.Where(e => e.Cert.CertName == item.SearchValue && e.Avaliable == _aval).ToList()
                            : dataList.Where(e => e.Cert.CertName.Contains(item.SearchValue) && e.Avaliable == _aval).ToList();
                searching = !string.IsNullOrEmpty(item.SearchValue);
            }

            pagesCount=dataList.Count()%10>0 ?
                (dataList.Count()/10)+1
                : dataList.Count()/10;
            searchList=dataList;
            dataList = searchList.Take(10).ToList();

            return dataList;
        }


    }
}
