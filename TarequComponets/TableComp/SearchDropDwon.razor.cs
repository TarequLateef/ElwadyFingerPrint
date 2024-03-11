using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TarequComponets.TableComp
{
    public partial class SearchDropDwon
    {
        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? FieldName { get; set; }

        [Parameter]
        public EventCallback<SearchTable> searching { get; set; }

        string checkAcion = "";
        private SearchTable Search = new SearchTable();

        private bool filterd = false;
        protected override void OnInitialized()
        {
            this.checkAcion = "Contains";
            this.Search = new SearchTable
            {
                SearchValue="",
                Condition=Condition.Contains,
                FieldName=this.FieldName
            };
        }
        private void SearchValue(ChangeEventArgs e)
        {
            Search.SearchValue = Convert.ToString(e.Value);
            if (!filterd) filterd=true;
            searching.InvokeAsync(Search);
        }

        private void ConditionEq()
        {
            Search.Condition=Condition.Equal;
            checkAcion="Equal";
            if (!filterd) filterd=true;
            searching.InvokeAsync(Search);
        }

        private void ConditionCont()
        {
            Search.Condition = Condition.Contains;
            checkAcion="Contains";
            if (!filterd) filterd=true;
            searching.InvokeAsync(Search);
        }

        private void BtnSearch()
        {
            if (!filterd) filterd=true;
            searching.InvokeAsync(Search);
        }

        private void BtnCancelSearch()
        {
            if (filterd) filterd=false;
            Search=new SearchTable
            {
                Condition=Condition.Contains,
                SearchValue="",
                FieldName=this.FieldName
            };
            searching.InvokeAsync(Search);
        }
    }

    public enum Condition 
    { Equal, GreaterThan, LessThan, Contains, EqualorGreaterThan, EqualorLessThan, NotEqual }

    public class SearchTable
    {
        public string? SearchValue { get; set; }
        public Condition Condition = Condition.Contains;
        public string? FieldName { get; set; }
    }
}
