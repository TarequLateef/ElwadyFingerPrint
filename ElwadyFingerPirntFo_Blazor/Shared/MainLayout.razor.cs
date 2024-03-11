namespace ElwadyFingerPirntFo_Blazor.Shared
{
    public partial class MainLayout
    {
        /// <summary>
        /// page caption , page link
        /// </summary>
        private Dictionary<string, string> ListPages = new Dictionary<string, string>();
        private void EmpLoadList()
        {
            ListPages.Add("EmployeesList", "الموظفين");
            ListPages.Add("EmployeesNotAvalList", "الموظفين القدامى");
            ListPages.Add("AllEmployeesList", "جميع المسجلين");
        }
    }

}
