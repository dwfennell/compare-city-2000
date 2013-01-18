using System.ComponentModel.DataAnnotations;

namespace CompareCity.Models
{
    /// <summary>
    /// A light-weight representation of a .sc2 city file.
    /// </summary>
    public class City
    {
        [ScaffoldColumn(false)]
        public int CityId { get; set; }

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
        [Display(Name="Population")]
        public int Population { get; set; }

        /// <summary>
        /// File path of the .sc2 file on the server.
        /// </summary>
        [Required, StringLength(260)]
        public string FilePath { get; set; }
    }
}
