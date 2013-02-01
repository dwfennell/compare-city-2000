using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CompareCity.Model
{
    /// <summary>
    /// Specifies cities and a 
    /// </summary>
    public class ComparisonGroupMember
    {
        // Primary key. 
        [ScaffoldColumn(false)]
        public int ComparisonGroupMemberId { get; set; }

        [ScaffoldColumn(false)]
        public int ComparisonGroupId { get; set; }
        public ComparisonGroup ComparisonGroup { get; set; }

        public string CityUser { get; set; }

        public int CityInfoId { get; set; }
        public virtual CityInfo CityInfo { get; set; }

        public double TotalScore { get; set; }
    }
}