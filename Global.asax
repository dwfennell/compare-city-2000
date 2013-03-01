<%@ Application Language="C#" %>
<%@ Import Namespace="CompareCity" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Data.Entity" %>
<%@ Import Namespace="CompareCity.Model" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        BundleConfig.RegisterBundles(BundleTable.Bundles);
        AuthConfig.RegisterOpenAuth();
        
        // Initialize database if necessary.
        Database.SetInitializer(new DbInititializer(Server.MapPath("~/")));
        using (var db = new DatabaseContext())
        {
            db.Database.Initialize(false);
        }
    }
    
    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

        // TODO: Uncomment line below.
        //Response.Redirect(CompareCity.Control.SiteControl.ErrorRedirect);
    }

</script>
