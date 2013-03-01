using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using CityParser2000;
using CompareCity.Model;

namespace CompareCity.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class GetCityValue
    {
        public static bool IsValueIdentifier(string canditateString, DatabaseContext db)
        {
            return db.ScoringIdentifiers.Any(s => s.Name == canditateString);
        }

        public static double GetValue(string scoringIdName, CityInfo city, DatabaseContext db)
        {
            ScoringIdentifier scoringId = db.ScoringIdentifiers.Single(s => s.Name.Equals(scoringIdName));

            object objVal = city.GetType().GetProperty(scoringId.PropertyName).GetValue(city);
            double value;

            if (objVal.GetType() == typeof(int))
            {
                value = (double)(int)objVal;
            }
            else
            {
                // Identifier validity should have already been checked, so this should be a double.
                value = (double)objVal;
            }

            return value;
        }
    }
}