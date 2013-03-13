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
        public static string Username
        {
            get { return string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name) ? "" : HttpContext.Current.User.Identity.Name; }
            private set { }
        }

        private static string _anonRedirect = "~/Account/Login.aspx";
        public static string AnonRedirect
        {
            get { return _anonRedirect; }
            private set { }
        }

        private static string _cityFileDirectory = "App_Data/CityFiles/";
        public static string CityFileDirectory
        {
            get { return _cityFileDirectory; }
            private set { }
        }

        private static string _scoringIdentifierFilepath = "App_Code/Config/ScoringIdentifiers.xml";
        public static string ScoringIdentifierFilepath {
            get { return _scoringIdentifierFilepath; }
            private set { }
        }

        private static string _errorRedirect = "Error.html";
        public static string ErrorRedirect
        {
            get { return _errorRedirect; }
            private set { }
        }
    }
}
