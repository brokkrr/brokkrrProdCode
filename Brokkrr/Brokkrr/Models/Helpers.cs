using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;

namespace BrokerMVC.Models
{
    public static class Helpers
    {
        public static MvcHtmlString SortDirection(this HtmlHelper helper,ref WebGrid grid,string columnname)
        {
        string html = "";
        if (grid.SortColumn == columnname && grid.SortDirection == System.Web.Helpers.SortDirection.Ascending)
            html = "A";
        else if (grid.SortColumn == columnname && grid.SortDirection == System.Web.Helpers.SortDirection.Descending)
            html = "D";
        else
            html="";

        return MvcHtmlString.Create(html);
        }
    }
}