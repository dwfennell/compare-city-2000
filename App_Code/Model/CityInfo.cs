using System.ComponentModel.DataAnnotations;

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

        public string CityName { get; set; }
        public string Mayor { get; set; }
        public int CitySize { get; set; }
        public int AvailableFunds { get; set; }
        public int LifeExpectancy { get; set; }
        public int EducationQuotent { get; set; }
        public int YearOfFounding { get; set; }
        public int DaysSinceFounding { get; set; }

        public string FilePath { get; set; }
        // TODO: Link to ASP user?
        public string User { get; set; }
        public System.DateTime Uploaded { get; set; }
    }
}
