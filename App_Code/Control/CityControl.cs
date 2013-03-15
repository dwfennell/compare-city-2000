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
        private static ThreadSafeRandom random = new ThreadSafeRandom();

        public static void UploadCity(Stream cityFileStream, string filename, string serverRoot, string username)
        {
            // Parse city file (quick-parse, does not fetch all information).
            var parser = new CityParser();
            City parserCity = parser.ParseCityFile(cityFileStream);

            createCityFileDirectory(serverRoot);
            storeCity(parserCity, username, generateCityFilepath(username, filename, serverRoot), cityFileStream);
        }

        public static void DeleteCity(int cityId)
        {
            using (var db = new DatabaseContext())
            {
                CityInfo cityInfo = db.CityInfoes.First(i => i.CityInfoId == cityId);
                db.CityInfoes.Remove(cityInfo);

                // Remove the city from any rankings it was present in.
                var rankedRefs = from r in db.RankingMembers
                                 where r.CityInfoId == cityInfo.CityInfoId
                                 select r;
                foreach (RankingMember rankedReference in rankedRefs)
                {
                    db.RankingMembers.Remove(rankedReference);
                }

                db.SaveChanges();
            }

            // TODO: Also delete city files... or mark them as "orphan" somehow.
            //       Maybe the city ref should just be marked as orphan in the db... that way we can save the ranked instances.
        }

        public static IQueryable<CityInfo> GetCities(string username)
        {
            IQueryable<CityInfo> cities;
            using (var db = new DatabaseContext())
            {
                IQueryable<CityInfo> query = from c in db.CityInfoes
                        where c.User.Equals(username)
                        select c;

                cities = query.ToList().AsQueryable();
            }

            return cities;
        }

        private static void storeCity(City parserCity, string username, string filepath, Stream cityFileStream)
        {
            // Fetch relevant data from parserCity.
            var city = new CityInfo(parserCity, username, filepath, DateTime.Now);

            // Save .sc2 file on the server.
            cityFileStream.Position = 0;
            using (Stream outputStream = File.OpenWrite(filepath))
            {
                cityFileStream.CopyTo(outputStream);
            }

            // Save parsed city data to database.
            using (var db = new DatabaseContext())
            {
                db.CityInfoes.Add(city);
                db.SaveChanges();
            }
        }

        private static void createCityFileDirectory(string serverRoot)
        {
            string directoryPath = String.Format("{0}{1}", serverRoot, SiteControl.CityFileDirectory);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
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
