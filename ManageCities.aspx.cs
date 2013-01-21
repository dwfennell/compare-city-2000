using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CityParser2000;
using CompareCity.Models;

public partial class ManageCities : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Event handler for city uploads.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_CityUpload(object sender, EventArgs e)
    {
        if (CityFileUpload.HasFile)
        {
            if (!CityValidator.validate(CityFileUpload.PostedFile.InputStream))
            {
                // Validation failed.
                CityUploadLabel.Text = "Invalid file type.";
                return;
            }

            // Save raw file data.
            // TODO: Store city files in subfolder based on account name? 
            String path = Server.MapPath("~/App_Data/CityFiles/");
            CityFileUpload.PostedFile.SaveAs(path + CityFileUpload.FileName);

            // Parse city.
            var parser = new CityParser();
            CityParser2000.City parserCity = parser.ParseCityFile(CityFileUpload.PostedFile.InputStream, true);
            
            // Store city data. 
            storeCity(parserCity, path);

            CityUploadLabel.Text = CityFileUpload.FileName + " uploaded!";
            return;
        }
        else
        {
            CityUploadLabel.Text = "No file selected.";
            return;
        }
    }

    private void storeCity(CityParser2000.City parserCity, string path)
    {
        var city = new CityInfo
        {
            CityName = parserCity.CityName,
            MayorName = parserCity.MayorName,
            CitySize = parserCity.GetMiscStatistic(CityParser2000.City.MiscStatistic.CitySize),
            FilePath = path
        };

        // TODO: Probably better to have some sort of context pool, or something.
        var context = new CityInfoContext();
        context.Cities.Add(city);
        context.SaveChanges();
    }

    public IQueryable<CityInfo> GetCities()
    {
        // TODO: Filter by username.

        var db = new CityInfoContext();
        IQueryable<CityInfo> query = db.Cities;
        return query;
    }

}