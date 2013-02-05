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
    private ThreadSafeRandom random = new ThreadSafeRandom();

    // TODO: Use a global context-pool of some sort? This seems wasteful.
    private DatabaseContext db = new DatabaseContext();

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

            // Parse city file (quick-parse, does not fetch all information).
            var parser = new CityParser();
            City parserCity = parser.ParseCityFile(CityFileUpload.PostedFile.InputStream, true);

            storeCity(parserCity);

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
        CityInfo cityInfo = db.CityInfoes.First(i => i.CityInfoId == CityInfoId);
        db.CityInfoes.Remove(cityInfo);
        db.SaveChanges();

        // TODO: Also delete city files... or mark them as "orphan" somehow.
    }

    public IQueryable<CityInfo> GetCities()
    {
        string username = SiteControl.Username;
        
        IQueryable<CityInfo> query =
            from c in db.CityInfoes
            where c.User.Equals(username)
            select c;
        return query;
    }

    private void storeCity(City parserCity)
    {
        string username = SiteControl.Username;
        string filepath = generateCityFilepath(username);

        // Fetch relevant data from parserCity.
        var city = new CityInfo
        {
            CityName = parserCity.CityName,
            Mayor = parserCity.MayorName,
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

        db.CityInfoes.Add(city);
        db.SaveChanges();

        // TODO: Serialize and store parserCity.

        // Save .sc2 file on server.
        CityFileUpload.PostedFile.SaveAs(filepath);
    }

    private string generateCityFilepath(string username)
    {
        string filepath;

        if (!string.IsNullOrWhiteSpace(username))
        {
            // Logged-in user has uploaded the file.
            filepath = String.Format("{0}{1}-{2}-{3}", 
                Server.MapPath("~/App_Data/CityFiles/"), 
                username, 
                CityFileUpload.FileName, 
                random.Next());

            while (File.Exists(filepath))
            {
                // Random number collision! Try again.
                filepath = String.Format("{0}{1}-{2}-{3}",
                Server.MapPath("~/App_Data/CityFiles/"),
                username,
                CityFileUpload.FileName,
                random.Next());
            }
        }
        else
        {
            // Anonymous user has uploaded a file.
            
            // TODO: Temporary cookie auth here? Or just force users to register? 
            filepath = String.Format("{1}{2}-{3}", 
                Server.MapPath("~/App_Data/CityFiles/"), 
                CityFileUpload.FileName, 
                random.Next());
            
            while (File.Exists(filepath)) 
            {
                // Random number collision! Try again.
                filepath = String.Format("{1}{2}-{3}", 
                    Server.MapPath("~/App_Data/CityFiles/"), 
                    CityFileUpload.FileName, 
                    random.Next());
            }
        }

        return filepath;
    }
}