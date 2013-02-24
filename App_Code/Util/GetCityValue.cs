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
        // TODO: Context pool? 
        private static DatabaseContext db = new DatabaseContext();

        public enum ValueIds { 
            CitySize, 
            AvailableFunds, 
            LifeExpectancy, 
            EducationQuotient, 
            WorkforcePercentage, 
            YearOfFounding,
            DaysSinceFounding,
            NeighborSize1,
            NeighborSize2,
            NeighborSize3,
            NeighborSize4,
            PolicePower,
            FirePower,
            Pollution,
            Traffic,
            Crime,
            PropertyValue,
            PopulationDensity, 
            PopulationGrowth
        };

        private static readonly Dictionary<string, ValueIds> _cityValueIdentifiers = new Dictionary<string, ValueIds> 
        {
            {"citySize", ValueIds.CitySize},
            {"availableFunds", ValueIds.AvailableFunds},
            {"lifeExpectancy", ValueIds.LifeExpectancy},
            {"educationQuotent", ValueIds.EducationQuotient},
            {"workforcePercentage", ValueIds.WorkforcePercentage},
            {"yearOfFounding", ValueIds.YearOfFounding},
            {"daysSinceFounding",ValueIds.DaysSinceFounding},
            //{"industryDemand",ValueIds}, // function?
            //{"industryTaxRate",ValueIds}, // function?
            //{"industryRatio",ValueIds}, // function?
            {"neighbor1Size",ValueIds.NeighborSize1},
            {"neighbor2Size",ValueIds.NeighborSize2},
            {"neighbor3Size",ValueIds.NeighborSize3},
            {"neighbor4Size",ValueIds.NeighborSize4},
            {"pollution",ValueIds.Pollution},
            {"traffic",ValueIds.Traffic},
            {"crime", ValueIds.Crime},
            {"propertyValue", ValueIds.PropertyValue},
            {"populationDensity", ValueIds.PopulationDensity},
            {"populationGrowth", ValueIds.PopulationGrowth}
        };

        public static bool IsValueIdentifier(string canditateString)
        {
            return db.ScoringIdentifiers.Any(s => s.Name == canditateString);
        }

        public static double GetValue(ValueIds valueId, City city)
        {
            switch (valueId)
            {
                case ValueIds.AvailableFunds:
                    return city.GetMiscStatistic(City.MiscStatistic.AvailableFunds);
                case ValueIds.CitySize:
                    return city.GetMiscStatistic(City.MiscStatistic.CitySize);
                case ValueIds.EducationQuotient:
                    return city.GetMiscStatistic(City.MiscStatistic.EducationQuotent);
                case ValueIds.LifeExpectancy:
                    return city.GetMiscStatistic(City.MiscStatistic.LifeExpectancy);
            }
            return 0.0;
        }

        public static double GetValue(string valueId, City city)
        {
            if (_cityValueIdentifiers.Keys.Contains(valueId))
            {
                return GetValue(_cityValueIdentifiers[valueId], city);
            }
            else
            {
                return 0.0;
            }
        }
    }
}