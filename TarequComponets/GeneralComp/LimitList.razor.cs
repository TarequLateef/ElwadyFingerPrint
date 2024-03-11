using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarequComponets.GeneralComp
{
    public partial class LimitList
    {
        [Parameter]
        public string[] ItemList { get; set; }

        [Parameter]
        public string? FieldCaption { get; set; }

        [Parameter]
        public EventCallback<string> SelectValue { get; set; }

        public string itemValue { get; set; }

        private void changeVal()
        {
            SelectValue.InvokeAsync(itemValue);
        }

    }
}
