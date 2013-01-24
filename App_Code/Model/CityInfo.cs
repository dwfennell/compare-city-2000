using System.ComponentModel.DataAnnotations;

namespace CompareCity.Models
{
    /// <summary>
    /// A light-weight representation of a .sc2 city file.
    /// </summary>
    public class CityInfo
    {
        [Key, ScaffoldColumn(false)]
        public int CityInfoId { get; set; }

        /// <summary>
        /// The city's name.
        /// </summary>
        [Required, StringLength(32), Display(Name = "Name")]
        public string CityName { get; set; }

        /// <summary>
        /// The mayor of the city.
        /// </summary>
        [Required, StringLength(25), Display(Name = "Mayor")]
        public string MayorName { get; set; }

        /// <summary>
        /// Total city population.
        /// </summary>
        [Display(Name="CitySize")]
        public int CitySize { get; set; }

        /// <summary>
        /// The city's bank account balance.
        /// </summary>
        [Display(Name="AvailableFunds")]
        public int AvailableFunds { get; set; }

        /// <summary>
        /// Work force life expectancy.
        /// </summary>
        [Display(Name = "LifeExpectancy")]
        public int LifeExpectancy { get; set; }

        /// <summary>
        /// Education Quotent ("EQ") measures the education level of this city's sims.
        /// </summary>
        [Display(Name = "EducationQuotent")]
        public int EducationQuotent { get; set; }

        /// <summary>
        /// Year the city was created.
        /// </summary>
        [Display(Name = "YearOfFounding")]
        public int YearOfFounding { get; set; }

        /// <summary>
        /// Number of simdays that have passed since city founding. 
        /// Note: Every month has 25 days.
        /// </summary>
        [Display(Name = "DaysSinceFounding")]
        public int DaysSinceFounding { get; set; }

        /// <summary>
        /// File path of the .sc2 file on the server.
        /// </summary>
        [Required, StringLength(260)]
        public string FilePath { get; set; }

        /// <summary>
        /// Name of the user who uploaded this city.
        /// </summary>
        [Display(Name = "User")]
        public string User { get; set; }

        /// <summary>
        /// <c>DateTime</c> the city was uploaded.
        /// </summary>
        [Required, Display(Name = "Uploaded")]
        public System.DateTime Uploaded { get; set; }
    }
}
