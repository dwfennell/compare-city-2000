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
        public int ComparisionGroupMemberId { get; set; }

        // GroupingIdentifier and GroupingName are used to clump cities (and their total scores) into groups.
        // They are not keys in the regular sense. 
        [ScaffoldColumn(false)]
        public int GroupingIdenifier { get; set; }
        [ScaffoldColumn(false)]
        public string GroupingName { get; set; }

        public int CityInfoId { get; set; }
        public virtual CityInfo CityInfo { get; set; }

        // The user who created this comparision group.
        public string User { get; set; }

        public int ScoringRulesId { get; set; }
        public virtual RuleSet ScoringRules { get; set; }

        public double TotalScore { get; set; }
    }
}