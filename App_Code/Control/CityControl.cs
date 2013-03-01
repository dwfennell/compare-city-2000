using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;

using CityParser2000;
using CompareCity.Model;
using CompareCity.Util;

namespace CompareCity.Control
{
    public class CityControl
    {
        // TODO: Context pool? 
        private static DatabaseContext db = new DatabaseContext();
        private static ThreadSafeRandom random = new ThreadSafeRandom();

        public static void UploadCity(Stream cityFileStream, string filename, string serverRoot, string username)
        {
            // Parse city file (quick-parse, does not fetch all information).
            var parser = new CityParser();
            City parserCity = parser.ParseCityFile(cityFileStream);

            storeCity(parserCity, username, generateCityFilepath(username, filename, serverRoot), cityFileStream);
        }

        public static void DeleteCity(int cityId)
        {
            CityInfo cityInfo = db.CityInfoes.First(i => i.CityInfoId == cityId);
            db.CityInfoes.Remove(cityInfo);
            db.SaveChanges();

            // TODO: Also delete city files... or mark them as "orphan" somehow.
        }

        public static IQueryable<CityInfo> GetCities(string username)
        {
            IQueryable<CityInfo> query =
                from c in db.CityInfoes
                where c.User.Equals(username)
                select c;
            return query;
        }

        private static void storeCity(City parserCity, string username, string filepath, Stream cityFileStream)
        {

            // Fetch relevant data from parserCity.
            var city = new CityInfo(parserCity, username, filepath, DateTime.Now);
            db.CityInfoes.Add(city);

            // TODO: Serialize and store parserCity.

            // Save .sc2 file on the server.
            cityFileStream.Position = 0;
            using (Stream outputStream = File.OpenWrite(filepath))
            {
                cityFileStream.CopyTo(outputStream);
            }
            
            db.SaveChanges();
        }


        private static string generateCityFilepath(string username, string filename, string serverRoot)
        {
            string filepath = String.Format("{0}{1}{2}-{3}-{4}",
                serverRoot,
                SiteControl.CityFileDirectory,
                username,
                filename,
                random.Next());

            return File.Exists(filepath) ? generateCityFilepath(username, filename, serverRoot) : filepath;
        }
    }
}
