using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CompareCity.Model
{
    /// <summary>
    /// Specifies cities and a 
    /// </summary>
    public class RankingMember
    {
        // Primary key. 
        [ScaffoldColumn(false)]
        public int RankingMemberId { get; set; }

        [ScaffoldColumn(false)]
        public int RankingId { get; set; }

        public int CityInfoId { get; set; }

        public double TotalScore { get; set; }
    }
}