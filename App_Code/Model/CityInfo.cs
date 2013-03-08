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
    public partial class CityInfo
    {
        [ScaffoldColumn(false)]
        public int CityInfoId { get; set; }

        // Parsed statistics. 
        public string CityName { get; set; }
        public string Mayor { get; set; }
        public int CitySize { get; set; }
        public int CitySizeArcology { get; set; }
        public int CitySizeNoArcology { get; set; }
        public int AvailableFunds { get; set; }
        public int LifeExpectancy { get; set; }
        public int EducationQuotent { get; set; }
        public int YearOfFounding { get; set; }
        public int DaysSinceFounding { get; set; }
        public int MonthsSinceFounding { get; set; }
        public int YearsSinceFounding { get; set; }
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

        public int ResidentialSize { get; set; }
        public int CommercialSize { get; set; }
        public int IndustrialSize { get; set; }

        public int TotalTraffic { get; set; }
        public int TotalPolicePower { get; set; }
        public int TotalLandValue { get; set; }
        public int TotalCrime { get; set; }
        public int TotalPollution { get; set; }
        public int TotalFirePower { get; set; }
        public int TotalPopulationDensity { get; set; }
        public int TotalPopulationGrowth { get; set; }

        public string FilePath { get; set; }
        public string User { get; set; }
        public System.DateTime Uploaded { get; set; }

        // Not yet parsed but would be nice:
        public int NumberOfTrees { get; set; }
        public int ResidentialDemand { get; set; }
        public int CommercialDemand { get; set; }
        public int IndustrialDemand { get; set; }

        // These would be averages? 
        public int Pollution { get; set; }
        public int Traffic { get; set; }
        public int Crime { get; set; }
        public int PropertyValue { get; set; }
        public int PopulationGrowth { get; set; }
        public int PopulationDensity { get; set; }

    }
}
