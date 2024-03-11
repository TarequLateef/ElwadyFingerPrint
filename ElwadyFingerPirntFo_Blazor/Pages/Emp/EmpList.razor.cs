using EmpSchema;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Newtonsoft.Json;
using TarequComponets.TableComp;
using IunitWork;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components.Forms;
using ElwadyFingerPirntFo_Blazor.Services.EmpSchema.Services;

namespace ElwadyFingerPirntFo_Blazor.Pages.Emp
{
    public partial class EmpList
    {

        private bool newEmp = false;
        private bool shwEmpData = false;
        private readonly EmplService _eServ = new EmplService();
        const string ActionUrl = "GetAllEmps";
        private int currPage = 1;
        protected override async Task OnInitializedAsync()
        {
            var fullUrl = _eServ.ControllerUrl+ ActionUrl;
            _eServ.dataList= await _eServ.GetDataList(fullUrl, true);
            _eServ.item= new Employees();
            _eServ.dumyList=_eServ.dataList;
            _eServ.dataList=_eServ.dataList.Take(10).ToList();
            _eServ.pagesCount=_eServ.dumyList.Count%10>0 ?
                  (_eServ.dumyList.Count/10)+1
                  : _eServ.dumyList.Count/10;
        }

        private void searchName(SearchTable st)
        {
            _eServ.ManageSearchArr(st);
            _eServ.Searching();
        }

        private void SearchWork(SearchTable st)
        {
            _eServ.ManageSearchArr(st);
            this._eServ.Searching();
        }

        private void SearchAddress(SearchTable st)
        {
            _eServ.ManageSearchArr(st);
            this._eServ.Searching();
        }

        private void SearchInsuranceNo(SearchTable st)
        {
            _eServ.ManageSearchArr(st);
            this._eServ.Searching();

        }

        private void SearchInsuranceDate(SearchTable st)
        {
            _eServ.ManageSearchArr(st);
            this._eServ.Searching();
        }

        private void SearchCert(SearchTable st)
        {
            _eServ.ManageSearchArr(st);
            this._eServ.Searching();
        }

        private void ShowNewComp()
        {
            newEmp = !newEmp;

        }

        private async void GetEmpData(string id)
        {
            shwEmpData=true;newEmp=false;
            _eServ.dataList=_eServ.dataList.Where(d => d.EmpID==Convert.ToInt32(id)).ToList();
            /*_eServ.item.EmpID=Convert.ToInt32(id);*/
            _eServ.item =await _eServ.GetData(_eServ.ControllerUrl+"GetEmpID", id);
        }

        private void GotoPage(int pg)
        {
            _eServ.dataList = _eServ.searching ?
                _eServ.searchList.Skip((pg-1)*10).Take(10).ToList()
                : _eServ.dumyList.Skip((pg-1)*10).Take(10).ToList();
            currPage=pg;
        }

        private void SwithWindows(bool showDet)
        {
            shwEmpData=showDet; newEmp=false;
            _eServ.dataList=_eServ.searching ? 
                _eServ.searchList.Take(10).ToList() 
                : _eServ.dumyList.Take(10).ToList();
            currPage=1;
        }

        private void AfterCreate(bool itemCreated)
        {
            shwEmpData=!itemCreated;
        }
        private void SetNewItem(Employees emp)
        {
            _eServ.item=emp;
            _eServ.dataList.Add(_eServ.item);
            _eServ.dumyList.Add(_eServ.item);
            _eServ.dataList=_eServ.dataList.Where(d => d.EmpID==emp.EmpID).ToList();
        }
    }
}
