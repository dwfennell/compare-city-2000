using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CityParser2000;

public partial class CityUpload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_CityUpload(object sender, EventArgs e)
    {
        // TODO: Store city files in subfolder based on account name? 
        String path = Server.MapPath("~/App_Data/CityFiles/");

        if (CityFileUpload.HasFile)
        {
            HttpPostedFile cityFile = CityFileUpload.PostedFile;

            if (!CityValidator.validate(cityFile.InputStream))
            {
                // Validation failed.
                CityUploadLabel.Text = "Invalid file type.";
                return;
            }

            CityFileUpload.PostedFile.SaveAs(path + CityFileUpload.FileName);

            var parser = new CityParser();
            City city = parser.ParseCityFile(cityFile.InputStream);

            // TODO: Store City in database.

            CityUploadLabel.Text = CityFileUpload.FileName + " uploaded!";
            return;
        }
        else
        {
            CityUploadLabel.Text = "No file selected.";
            return;
        }
    }
}