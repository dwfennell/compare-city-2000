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

            // Parse city file.
            var parser = new CityParser();
            City parserCity = parser.ParseCityFile(CityFileUpload.PostedFile.InputStream, true);

            storeCity(parserCity);

            // Refresh cities list. 
            // TODO: There must be a better way to refresh the list..
            Response.Redirect(Request.RawUrl);

            CityUploadLabel.Text = CityFileUpload.FileName + " uploaded!";
        }
        else
        {
            CityUploadLabel.Text = "No file selected.";
        }
    }

    public IQueryable<CityInfo> GetCities()
    {
        string username;
        if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
        {
            username = HttpContext.Current.User.Identity.Name;
        }
        else
        {
            username = "";
        }

        var db = new CityInfoContext();
        IQueryable<CityInfo> query =
            from c in db.Cities
            where c.User.Equals(username)
            select c;

        return query;
    }

    private void storeCity(City parserCity)
    {
        // Set filepath to raw file data, depending on login status.
        // TODO: Don't accept uploads from users who aren't logged in?
        string username;
        string filepath;
        if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
        {
            username = HttpContext.Current.User.Identity.Name;
            filepath = Server.MapPath("~/App_Data/CityFiles/" + username + "/") + CityFileUpload.FileName;
        }
        else
        {
            filepath = Server.MapPath("~/App_Data/CityFiles/") + CityFileUpload.FileName;
            username = "";
        }

        // TODO: Modify file name to prevent unintended overwrites.
        CityFileUpload.PostedFile.SaveAs(filepath);

        // TODO: Serialize and store parserCity.
        
        // Scrape relevant data from parserCity.
        var city = new CityInfo
        {
            CityName = parserCity.CityName,
            MayorName = parserCity.MayorName,
            CitySize = parserCity.GetMiscStatistic(City.MiscStatistic.CitySize),
            YearOfFounding = parserCity.GetMiscStatistic(City.MiscStatistic.YearOfFounding),
            DaysSinceFounding = parserCity.GetMiscStatistic(City.MiscStatistic.DaysSinceFounding),
            AvailableFunds = parserCity.GetMiscStatistic(City.MiscStatistic.AvailableFunds),
            LifeExpectancy = parserCity.GetMiscStatistic(City.MiscStatistic.LifeExpectancy),
            EducationQuotent = parserCity.GetMiscStatistic(City.MiscStatistic.EducationQuotent),
            User = username,
            FilePath = filepath,
            Uploaded = DateTime.Now
        };

        // TODO: Probably better to have some sort of context pool, or something.
        var context = new CityInfoContext();
        context.Cities.Add(city);
        context.SaveChanges();
    }
}