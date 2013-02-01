using System.ComponentModel.DataAnnotations;

using CompareCity.Model;

/// <summary>
/// 
/// </summary>
public class ComparisonGroup
{
    public int ComparisonGroupId { get; set; }
    public string ComparisonGroupName { get; set; }

    public int RuleSetId { get; set; }
    public virtual RuleSet RuleSet { get; set; }

    public string User { get; set; }
}