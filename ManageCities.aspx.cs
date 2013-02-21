using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CityParser2000;
using CompareCity.Model;
using CompareCity.Control;
using CompareCity.Util;

public partial class ManageCities : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Don't allow users without logins here.
        if (String.IsNullOrWhiteSpace(SiteControl.Username))
        {
            Response.Redirect(SiteControl.AnonRedirect);
        }
    }

    protected void UploadButton_Click(object sender, EventArgs e)
    {
        if (CityFileUpload.HasFile)
        {
            if (!CityValidator.validate(CityFileUpload.PostedFile.InputStream))
            {
                // Validation failed.
                CityUploadLabel.Text = "Invalid file type.";
                return;
            }
            
            CityControl.UploadCity(CityFileUpload.PostedFile.InputStream, CityFileUpload.PostedFile.FileName, Server.MapPath("~/"), SiteControl.Username);

            // Refresh cities list. 
            // TODO: Is there a better way to do this? 
            Response.Redirect(Request.RawUrl);

            CityUploadLabel.Text = CityFileUpload.FileName + " uploaded!";
        }
        else
        {
            CityUploadLabel.Text = "No file selected.";
        }
    }

    public void CitiesView_DeleteItem(int CityInfoId)
    {
        CityControl.DeleteCity(CityInfoId);
    }

    public IQueryable<CityInfo> GetCities()
    {
        return CityControl.GetCities(SiteControl.Username);
    }
}