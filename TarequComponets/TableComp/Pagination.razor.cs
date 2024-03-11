using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarequComponets.TableComp
{
    public partial class Pagination
    {
        [Parameter]
        public int pagesCount { get; set; } = 1;
        [Parameter]
        public EventCallback<int> CurrPage { get; set; }


        bool avalFirst = false; bool avalPrev = false;
        bool avalNext = true; bool avalLast = true;
        private int pageGroup = 1; private int groupEnd = 10;
        private int firstPage = 1; private int currPage = 1;

        private void FirstGroup()
        {
            firstPage=1;
            pageGroup=1;
            groupEnd=pageGroup*10;
            avalLast=true; avalNext=true; avalFirst=false; avalPrev=false;
        }
        private void PrevGroup()
        {
            pageGroup--; firstPage=(pageGroup*10)-9;
            groupEnd=pageGroup*10;
            int maxGroup = pagesCount%10>0 ? (pagesCount/10)+1 : (pagesCount/10);
            if (pageGroup==maxGroup) { avalFirst=true; avalPrev=true; avalNext=false; avalLast=false; }
            else if (pageGroup==1) { avalLast=false; avalNext=false; avalFirst=true; avalPrev=true; }
            else { avalLast=true; avalNext=true; avalFirst=true; avalPrev=true; }

        }
        private void NextGroup()
        {
            firstPage+=10;
            pageGroup++; groupEnd=pageGroup*10;
            if (firstPage+10>pagesCount) { avalLast=false; avalNext=false; avalFirst=true; avalPrev=true; }
            int maxGroup = pagesCount%10>0 ? (pagesCount/10)+1 : (pagesCount/10);
            if (pageGroup==maxGroup) { avalFirst=true; avalPrev=true; avalNext=false; avalLast=false; }
            else if (pageGroup==1) { avalLast=false; avalNext=false; avalFirst=true; avalPrev=true; }
            else { avalLast=true; avalNext=true; avalFirst=true; avalPrev=true; }

        }
        private void LastGroup()
        {
            pageGroup=pagesCount%10>0 ?
                (pagesCount/10)+1
                : pagesCount/10;
            firstPage=(pageGroup*10)-9;
            groupEnd=pageGroup*10;
            avalLast=false; avalNext=false; avalFirst=true; avalPrev=true;
        }

    }
}
