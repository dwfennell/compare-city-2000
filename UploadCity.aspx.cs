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
            if (CityValidator.validate(CityFileUpload.PostedFile.InputStream))
            {
                // Validation passed. Reset steam pointed to startoffile.
                CityFileUpload.PostedFile.InputStream.Position = 0;
            }
            else
            {
                // Validation failed.
                CityUploadLabel.Text = "Invalid file type.";
                return;
            }

            string cityFileName = CityFileUpload.FileName;
            CityFileUpload.PostedFile.SaveAs(path + cityFileName);
            CityUploadLabel.Text = cityFileName + " uploaded!";

            // TODO: parse city file.

        }
        else
        {
            CityUploadLabel.Text = "No file selected.";
            return;
        }
    }
}