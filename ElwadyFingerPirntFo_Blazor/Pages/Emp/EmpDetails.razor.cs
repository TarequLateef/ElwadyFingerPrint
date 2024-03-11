using ElwadyFingerPirntFo_Blazor.Services.EmpSchema.Services;
using EmpSchema;
using Microsoft.AspNetCore.Components;

namespace ElwadyFingerPirntFo_Blazor.Pages.Emp
{
    public partial class EmpDetails
    {
        [Parameter]
        public bool ShowDetails { get; set; } = false;
        [Parameter]
        public Employees? empItem { get; set; } =new Employees();

        [Parameter]
        public EventCallback<bool> RetToTable { get; set; }

        private bool showMenu = false;
        /*private readonly EmplService _eServ = new EmplService();*/
        
        private void ShowMenu()
        {
            showMenu= !showMenu;
        }

        private void HideDetails() { ShowDetails=false; RetToTable.InvokeAsync(ShowDetails); }
        /*protected override async Task OnInitializedAsync()
        {
            string actionUrl = _eServ.ControllerUrl + "GetEmpID";
            if (empId.HasValue && empId.Value!=0)
                _eServ.item=await _eServ.GetData(actionUrl, empId.Value.ToString());
            else _eServ.item = new Employees();
        }*/
    }
}
