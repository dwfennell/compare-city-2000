using System.ComponentModel.DataAnnotations;

using CompareCity.Model;

/// <summary>
/// 
/// </summary>
public class Ranking
{
    public int RankingId { get; set; }
    public string RankingName { get; set; }

    public int RuleSetId { get; set; }

    public string User { get; set; }
}