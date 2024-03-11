using ElwadyFingerPirntFo_Blazor.Services.EmpSchema.Services;
using EmpSchema;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace ElwadyFingerPirntFo_Blazor.Pages.Emp
{
    public partial class NewEmployee
    {
        private readonly EmplService _eServ = new EmplService();
        private readonly CertService _certService = new CertService();
        private string insureCase = "غير مؤمن عليه";
        const string ActionUrl = "AddEmp";

        private string[] InsuranceList = new string[] { "مؤمن عليه", "غير مؤمن عليه" };

        [Parameter]
        public bool ShowComponent { get; set; }
        [Parameter]
        public EventCallback<bool> ShowNewItem { get; set; }
        [Parameter]
        public EventCallback<Employees> NewItem { get; set; }

        /*private Employees empItem { get; set; } = new Employees();*/
        public NewEmployee() => _eServ.item=new Employees();


        protected override async Task OnInitializedAsync()
        {
            _certService.dataList=
                await _certService.GetDataList(_certService.ControllerUrl + "AllCerts", true);
        }

        private void changeStatus(string e)
        {
            _eServ.item.InsuranceStatus=e;
        }

        protected async Task AddNewEmp()
        {
            var addEmp = await _eServ.AddItem(_eServ.ControllerUrl+ActionUrl, _eServ.item);
            if (addEmp.IsSuccessStatusCode)
            {
                ShowComponent=false;
                string data = await addEmp.Content.ReadAsStringAsync();
                _eServ.item=JsonConvert.DeserializeObject<Employees>(data);
                await ShowNewItem.InvokeAsync(true);
                await NewItem.InvokeAsync(_eServ.item);
            }
            _eServ.item=new Employees();
        }
    }
}
