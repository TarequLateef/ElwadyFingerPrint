using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarequComponets.TableComp
{
    public partial class BbtnDetails
    {
        [Parameter]
        public string ItemID { get; set; } = "";
        [Parameter]
        public EventCallback<string> GetItemID { get; set; }

        public void GetData()
        {
            if (string.IsNullOrEmpty(ItemID))
                GetItemID.InvokeAsync("0");
            else
                GetItemID.InvokeAsync(ItemID);
        }

    }
}
