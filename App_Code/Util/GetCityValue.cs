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
            {"citysize", ValueIds.CitySize},
            {"availablefunds", ValueIds.AvailableFunds},
            {"lifeexpectancy", ValueIds.LifeExpectancy},
            {"educationquotent", ValueIds.EducationQuotient},
            {"workforcepercentage", ValueIds.WorkforcePercentage},
            {"yearoffounding", ValueIds.YearOfFounding},
            {"dayssincefounding",ValueIds.DaysSinceFounding},
            //{"industryDemand",ValueIds}, // function?
            //{"industryTaxRate",ValueIds}, // function?
            //{"industryRatio",ValueIds}, // function?
            {"neighbor1size",ValueIds.NeighborSize1},
            {"neighbor2size",ValueIds.NeighborSize2},
            {"neighbor3size",ValueIds.NeighborSize3},
            {"neighbor4size",ValueIds.NeighborSize4},
            {"pollution",ValueIds.Pollution},
            {"traffic",ValueIds.Traffic},
            {"crime", ValueIds.Crime},
            {"propertyvalue", ValueIds.PropertyValue},
            {"populationdensity", ValueIds.PopulationDensity},
            {"populationgrowth", ValueIds.PopulationGrowth}
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