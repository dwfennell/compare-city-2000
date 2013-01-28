using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace CompareCity.Control
{
    public static class SiteControl
    {
        public static string Username {
            get { return string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name) ? "" : HttpContext.Current.User.Identity.Name; }
            private set { }
        }



    }
}
