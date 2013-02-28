using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CompareCity.Control;

public partial class AnonDefault : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Don't allow logged-in users here.
        if (!String.IsNullOrWhiteSpace(SiteControl.Username))
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}