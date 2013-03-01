using System.ComponentModel.DataAnnotations;

using CityParser2000;

namespace CompareCity.Model
{
    /// <summary>
    /// A light-weight representation of a .sc2 city file.
    /// This class does not represent all possible data contained in a file, 
    /// rather it contains some common city metrics which a user may find 
    /// interesting.
    /// </summary>
    public class CityInfo
    {
        [ScaffoldColumn(false)]
        public int CityInfoId { get; set; }

        // Parsed statistics. 
        public string CityName { get; set; }
        public string Mayor { get; set; }
        public int CitySize { get; set; }
        public int AvailableFunds { get; set; }
        public int LifeExpectancy { get; set; }
        public int EducationQuotent { get; set; }
        public int YearOfFounding { get; set; }
        public int DaysSinceFounding { get; set; }
        public int WorkforcePercentage { get; set; }

        public int NeighborSize1 { get; set; }
        public int NeighborSize2 { get; set; }
        public int NeighborSize3 { get; set; }
        public int NeighborSize4 { get; set; }

        public int SteelDemand { get; set; }
        public int TextilesDemand { get; set; }
        public int PetrochemicalDemand { get; set; }
        public int FoodDemand { get; set; }
        public int ConstructionDemand { get; set; }
        public int AutomotiveDemand { get; set; }
        public int AerospaceDemand { get; set; }
        public int FinanceDemand { get; set; }
        public int MediaDemand { get; set; }
        public int ElectronicsDemand { get; set; }
        public int ToursimDemand { get; set; }
        public int SteelTaxRate { get; set; }
        public int TextilesTaxRate { get; set; }
        public int PetrochemicalTaxRate { get; set; }
        public int FoodTaxRate { get; set; }
        public int ConstructionTaxRate { get; set; }
        public int AutomotiveTaxRate { get; set; }
        public int AerospaceTaxRate { get; set; }
        public int FinanceTaxRate { get; set; }
        public int MediaTaxRate { get; set; }
        public int ElectronicsTaxRate { get; set; }
        public int ToursimTaxRate { get; set; }
        public int SteelRatio { get; set; }
        public int TextilesRatio { get; set; }
        public int PetrochemicalRatio { get; set; }
        public int FoodRatio { get; set; }
        public int ConstructionRatio { get; set; }
        public int AutomotiveRatio { get; set; }
        public int AerospaceRatio { get; set; }
        public int FinanceRatio { get; set; }
        public int MediaRatio { get; set; }
        public int ElectronicsRatio { get; set; }
        public int ToursimRatio { get; set; }

        // Not yet parsed but needed:
        public int NumberOfTrees { get; set; }
        public int ResidentialDemand { get; set; }
        public int CommercialDemand { get; set; }
        public int IndustrialDemand { get; set; }
        public int ResidentialSize { get; set; }
        public int CommercialSize { get; set; }
        public int IndustrialSize { get; set; }

        public int Pollution { get; set; }
        public int Traffic { get; set; }
        public int Crime { get; set; }
        public int PropertyValue { get; set; }
        public int PopulationGrowth { get; set; }
        public int PopulationDensity { get; set; }
        
        // Derivative statistics.
        public double YearsSinceFounding { get; set; }

        public string FilePath { get; set; }
        public string User { get; set; }
        public System.DateTime Uploaded { get; set; }

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

            // Not yet parsed but needed:
            //NumberOfTrees = parserCity.GetMiscStatistic(City.MiscStatistic);
            //ResidentialDemand = parserCity.GetMiscStatistic(City.MiscStatistic);
            //CommercialDemand = parserCity.GetMiscStatistic(City.MiscStatistic);
            //IndustrialDemand = parserCity.GetMiscStatistic(City.MiscStatistic);
            //ResidentialSize = parserCity.GetMiscStatistic(City.MiscStatistic);
            //CommercialSize = parserCity.GetMiscStatistic(City.MiscStatistic);
            //IndustrialSize = parserCity.GetMiscStatistic(City.MiscStatistic);

            //Pollution = parserCity.GetMiscStatistic(City.MiscStatistic);
            //Traffic = parserCity.GetMiscStatistic(City.MiscStatistic);
            //Crime = parserCity.GetMiscStatistic(City.MiscStatistic);
            //PropertyValue = parserCity.GetMiscStatistic(City.MiscStatistic);
            //PopulationGrowth = parserCity.GetMiscStatistic(City.MiscStatistic);
            //PopulationDensity = parserCity.GetMiscStatistic(City.MiscStatistic);

            // Derivative statistics.
            YearsSinceFounding = System.Math.Round(DaysSinceFounding / (25.0 * 12), 2);
        }

        public CityInfo()
        {
        }

    }
}
