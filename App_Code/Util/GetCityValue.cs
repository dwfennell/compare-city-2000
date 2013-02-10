using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CityParser2000;

namespace CompareCity.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class GetCityValue
    {
        public enum ValueIds { CitySize, AvailableFunds, LifeExpectancy, EducationQuotient };

        private static readonly Dictionary<string, ValueIds> _cityValueIdentifiers = new Dictionary<string, ValueIds> 
        {
            {"citysize", ValueIds.CitySize},
            {"availablefunds", ValueIds.AvailableFunds},
            {"lifeexpectancy", ValueIds.LifeExpectancy},
            {"educationquotent", ValueIds.EducationQuotient}
        };

        public static bool IsValueIdentifier(string canditateString)
        {
            return _cityValueIdentifiers.Keys.Contains(canditateString);
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