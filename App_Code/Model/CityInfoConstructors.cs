using System.ComponentModel.DataAnnotations;

using CityParser2000;

namespace CompareCity.Model
{
    /// <summary>
    /// Summary description for CityInfoConstructors
    /// </summary>
    public partial class CityInfo
    {
        /// <summary>
        /// Constructs a CityInfo instance out of a CityParser2000 city object.
        /// </summary>
        /// <param name="parserCity">A fully parsed <c>CityParser2000</c>city.</param>
        public CityInfo(CityParser2000.City parserCity, string user, string filepath, System.DateTime uploaded)
        {
            User = user;
            FilePath = filepath;
            Uploaded = uploaded;

            CityName = parserCity.CityName;
            Mayor = parserCity.MayorName;
            CitySize = parserCity.GetMiscStatistic(City.MiscStatistic.CitySize);
            YearOfFounding = parserCity.GetMiscStatistic(City.MiscStatistic.YearOfFounding);
            DaysSinceFounding = parserCity.GetMiscStatistic(City.MiscStatistic.DaysSinceFounding);
            MonthsSinceFounding = parserCity.GetMiscStatistic(City.MiscStatistic.MonthsSinceFounding);
            YearsSinceFounding = parserCity.GetMiscStatistic(City.MiscStatistic.YearsSinceFounding);
            AvailableFunds = parserCity.GetMiscStatistic(City.MiscStatistic.AvailableFunds);
            LifeExpectancy = parserCity.GetMiscStatistic(City.MiscStatistic.LifeExpectancy);
            EducationQuotent = parserCity.GetMiscStatistic(City.MiscStatistic.EducationQuotent);
            WorkforcePercentage = parserCity.GetMiscStatistic(City.MiscStatistic.WorkforcePercentage);

            NeighborSize1 = parserCity.GetMiscStatistic(City.MiscStatistic.NeighborSize1);
            NeighborSize2 = parserCity.GetMiscStatistic(City.MiscStatistic.NeighborSize2);
            NeighborSize3 = parserCity.GetMiscStatistic(City.MiscStatistic.NeighborSize3);
            NeighborSize4 = parserCity.GetMiscStatistic(City.MiscStatistic.NeighborSize4);

            SteelDemand = parserCity.GetMiscStatistic(City.MiscStatistic.SteelMiningDemand);
            TextilesDemand = parserCity.GetMiscStatistic(City.MiscStatistic.TextilesDemand);
            PetrochemicalDemand = parserCity.GetMiscStatistic(City.MiscStatistic.PetrochemicalDemand);
            FoodDemand = parserCity.GetMiscStatistic(City.MiscStatistic.FoodDemand);
            ConstructionDemand = parserCity.GetMiscStatistic(City.MiscStatistic.ConstructionDemand);
            AutomotiveDemand = parserCity.GetMiscStatistic(City.MiscStatistic.AutomotiveDemand);
            AerospaceDemand = parserCity.GetMiscStatistic(City.MiscStatistic.AerospaceDemand);
            FinanceDemand = parserCity.GetMiscStatistic(City.MiscStatistic.FinanceDemand);
            MediaDemand = parserCity.GetMiscStatistic(City.MiscStatistic.MediaDemand);
            ElectronicsDemand = parserCity.GetMiscStatistic(City.MiscStatistic.ElectronicsDemand);
            ToursimDemand = parserCity.GetMiscStatistic(City.MiscStatistic.TourismDemand);

            SteelTaxRate = parserCity.GetMiscStatistic(City.MiscStatistic.SteelMiningTaxRate);
            TextilesTaxRate = parserCity.GetMiscStatistic(City.MiscStatistic.TextilesTaxRate);
            PetrochemicalTaxRate = parserCity.GetMiscStatistic(City.MiscStatistic.PetrochemicalTaxRate);
            FoodTaxRate = parserCity.GetMiscStatistic(City.MiscStatistic.FoodTaxRate);
            ConstructionTaxRate = parserCity.GetMiscStatistic(City.MiscStatistic.ConstructionTaxRate);
            AutomotiveTaxRate = parserCity.GetMiscStatistic(City.MiscStatistic.AutomotiveTaxRate);
            AerospaceTaxRate = parserCity.GetMiscStatistic(City.MiscStatistic.AerospaceTaxRate);
            FinanceTaxRate = parserCity.GetMiscStatistic(City.MiscStatistic.FinanceTaxRate);
            MediaTaxRate = parserCity.GetMiscStatistic(City.MiscStatistic.MediaTaxRate);
            ElectronicsTaxRate = parserCity.GetMiscStatistic(City.MiscStatistic.ElectronicsTaxRate);
            ToursimTaxRate = parserCity.GetMiscStatistic(City.MiscStatistic.TourismTaxRate);

            SteelRatio = parserCity.GetMiscStatistic(City.MiscStatistic.SteelMiningRatio);
            TextilesRatio = parserCity.GetMiscStatistic(City.MiscStatistic.TextilesRatio);
            PetrochemicalRatio = parserCity.GetMiscStatistic(City.MiscStatistic.PetrochemcalRatio);
            FoodRatio = parserCity.GetMiscStatistic(City.MiscStatistic.FoodRatio);
            ConstructionRatio = parserCity.GetMiscStatistic(City.MiscStatistic.ConstructionRatio);
            AutomotiveRatio = parserCity.GetMiscStatistic(City.MiscStatistic.AutomotiveRatio);
            AerospaceRatio = parserCity.GetMiscStatistic(City.MiscStatistic.AerospaceRatio);
            FinanceRatio = parserCity.GetMiscStatistic(City.MiscStatistic.FinanceRatio);
            MediaRatio = parserCity.GetMiscStatistic(City.MiscStatistic.MediaRatio);
            ElectronicsRatio = parserCity.GetMiscStatistic(City.MiscStatistic.ElectronicsRatio);
            ToursimRatio = parserCity.GetMiscStatistic(City.MiscStatistic.TourismRatio);

            TotalTraffic = parserCity.GetMiscStatistic(City.MiscStatistic.TotalTraffic);
            TotalPolicePower = parserCity.GetMiscStatistic(City.MiscStatistic.TotalPolicePower);
            TotalLandValue = parserCity.GetMiscStatistic(City.MiscStatistic.TotalLandValue);
            TotalCrime = parserCity.GetMiscStatistic(City.MiscStatistic.TotalCrime);
            TotalPollution = parserCity.GetMiscStatistic(City.MiscStatistic.TotalPollution);
            TotalFirePower = parserCity.GetMiscStatistic(City.MiscStatistic.TotalFirePower);
            TotalPopulationDensity = parserCity.GetMiscStatistic(City.MiscStatistic.TotalPopulationDensity);
            TotalPopulationGrowth = parserCity.GetMiscStatistic(City.MiscStatistic.TotalPopulationGrowth);

            ResidentialSize = parserCity.GetMiscStatistic(City.MiscStatistic.ResidentialSize);
            CommercialSize = parserCity.GetMiscStatistic(City.MiscStatistic.CommercialSize);
            IndustrialSize = parserCity.GetMiscStatistic(City.MiscStatistic.IndustrialSize);

            // Not yet parsed but useful:
            //NumberOfTrees = parserCity.GetMiscStatistic(City.MiscStatistic);
            //ResidentialDemand = parserCity.GetMiscStatistic(City.MiscStatistic);
            //CommercialDemand = parserCity.GetMiscStatistic(City.MiscStatistic);
            //IndustrialDemand = parserCity.GetMiscStatistic(City.MiscStatistic);

            //Pollution = parserCity.GetMiscStatistic(City.MiscStatistic);
            //Traffic = parserCity.GetMiscStatistic(City.MiscStatistic);
            //Crime = parserCity.GetMiscStatistic(City.MiscStatistic);
            //PropertyValue = parserCity.GetMiscStatistic(City.MiscStatistic);
            //PopulationGrowth = parserCity.GetMiscStatistic(City.MiscStatistic);
            //PopulationDensity = parserCity.GetMiscStatistic(City.MiscStatistic);
        }

        public CityInfo()
        {
        }
    }
}