using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CompareCity.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class ComparisonGroup
    {
        public int ComparisionGroupId { get; set; }
        public int ScoringRulesId { get; set; }
        public virtual RuleSet ScoringRules { get; set; }

        public virtual ICollection<CityInfo> CityInfoes { get; set; }
    }
}