using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;

using CompareCity.Model;
using CompareCity.Util;
using CityParser2000;
using FormulaScore;

/// <summary>
/// 
/// </summary>
public static class RankingControl
{
    // TODO: Context pool? 
    private static DatabaseContext db = new DatabaseContext();

    #region private 'constants'

    private static readonly Dictionary<string, Type> citySearchColumns = new Dictionary<string, Type>
    {
        {"User", typeof(string)}, 
        {"City Name", typeof(string)}, 
        {"Size", typeof(int)},
        {"Funds", typeof(int)},
        {"CityID", typeof(int)}
    };

    private static readonly Dictionary<string, Type> cityRankingsColumns = new Dictionary<string, Type>
    {
        {"CityID", typeof(int)},
        {"City Name", typeof(string)},
        {"User", typeof(string)},
        {"Score", typeof(double)}
    };

    private const int rankingCityIdColumn = 0;
    private const int rankingScoreColumn = 3;
    #endregion

    public enum RuleSetKeys { Name, Formula };
    public enum RankingKeys { Name, RuleSetId, RuleSetName, RuleSetFormula};

    #region rankings

    public static DataTable GetEmptyRankingTable()
    {
        return initDataTable(cityRankingsColumns);
    }

    public static DataTable AddRankedCity(DataTable rankedCities, int cityId)
    {
        if (rankedCities == null)
        {
            rankedCities = initDataTable(cityRankingsColumns);
        }

        // Load city data.
        CityInfo city = getCity(cityId);
        rankedCities.Rows.Add(city.CityInfoId, city.CityName, city.User, 0.0);

        return rankedCities;
    }

    public static List<ListItem> GetRankings(string username)
    {
        var rankings = new List<ListItem>();
        var query =
            from g in db.ComparisonGroups
            where g.User == username
            select g;

        foreach (ComparisonGroup group in query)
        {
            rankings.Add(new ListItem(group.ComparisonGroupName, group.ComparisonGroupId.ToString()));
        }

        return rankings;
    }

    public static DataTable ScoreCities(DataTable citiesTable, int ruleSetId)
    {
        string ruleSetFormula = LoadRuleSet(ruleSetId)[RuleSetKeys.Formula];

        // Load rule set into scorer. 
        FormulaScore.FormulaScore scorer = new FormulaScore.FormulaScore(ruleSetFormula);

        int cityId;
        CityInfo city;
        City parsedCity;
        CityParser parser = new CityParser();
        for (int i = 0; i < citiesTable.Rows.Count; i++)
        {
            cityId = (int)citiesTable.Rows[i][rankingCityIdColumn];
            city = getCity(cityId);

            using (FileStream cityStream = File.OpenRead(city.FilePath))
            {
                parsedCity = parser.ParseCityFile(cityStream);
            }
            // Score and update table value.
            citiesTable.Rows[i][rankingScoreColumn] = scoreCity(parsedCity, ruleSetFormula);
        }

        return citiesTable;
    }

    public static void SaveRankingCities(string rankingName, string user, DataTable citiesTable)
    {
        // Fetch parent ranking.
        ComparisonGroup ranking = db.ComparisonGroups.Single(c => c.ComparisonGroupName == rankingName && c.User == user);

        // Delete old members.
        foreach (ComparisonGroupMember rankedCity in db.ComparisonGroupMembers.Where(c => c.ComparisonGroupId == ranking.ComparisonGroupId))
        {
            db.ComparisonGroupMembers.Remove(rankedCity);
        }

        // Add new members.
        int cityId;
        double score;
        for (int i = 0; i < citiesTable.Rows.Count; i++)
        {
            cityId = (int)citiesTable.Rows[i][rankingCityIdColumn];
            score = (double)citiesTable.Rows[i][rankingScoreColumn];

            db.ComparisonGroupMembers.Add(new ComparisonGroupMember
            {
                CityInfoId = cityId,
                TotalScore = score,
                ComparisonGroupId = ranking.ComparisonGroupId
            });
        }

        db.SaveChanges();
    }

    public static void SaveRanking(string rankingName, string username, int ruleSetId)
    {
        if (rankingExists(rankingName, username))
        {
            // Update existing ranking. 
            ComparisonGroup ranking = db.ComparisonGroups.First(g => g.ComparisonGroupName == rankingName && g.User == username);
            ranking.RuleSetId = ruleSetId;
        }
        else
        {
            // Create new ranking.
            ComparisonGroup newRanking = new ComparisonGroup
            {
                ComparisonGroupName = rankingName,
                RuleSetId = ruleSetId,
                User = username
            };
            db.ComparisonGroups.Add(newRanking);
        }

        db.SaveChanges();
    }

    public static DataTable LoadRankingCities(string rankingName, string user)
    {
        ComparisonGroup ranking = db.ComparisonGroups.Single(c => c.ComparisonGroupName == rankingName && c.User == user);

        DataTable rankedCitiesTable = initDataTable(cityRankingsColumns);
        CityInfo city;
        List<ComparisonGroupMember> rankedCities = db.ComparisonGroupMembers.Where(c => c.ComparisonGroupId == ranking.ComparisonGroupId).ToList();
        foreach (ComparisonGroupMember rankedCity in rankedCities)
        {
            city = db.CityInfoes.Single(c => c.CityInfoId == rankedCity.CityInfoId);
            rankedCitiesTable.Rows.Add(rankedCity.CityInfoId, city.CityName, user, rankedCity.TotalScore);
        }

        return rankedCitiesTable;
    }

    public static Dictionary<RankingKeys, string> LoadRanking(int rankingId)
    {
        ComparisonGroup ranking = db.ComparisonGroups.Single(r => r.ComparisonGroupId == rankingId);
        RuleSet rankingRuleSet = db.RuleSets.Single(r => r.RuleSetId == ranking.RuleSetId);

        return new Dictionary<RankingKeys, string>
        {
            {RankingKeys.Name, ranking.ComparisonGroupName},
            {RankingKeys.RuleSetId, ranking.RuleSetId.ToString()},
            {RankingKeys.RuleSetName, rankingRuleSet.RuleSetName},
            {RankingKeys.RuleSetFormula, rankingRuleSet.Formula}
        };
    }

    public static string GetUntitledRankingName()
    {
        return "Untitled Ranking";
    }
    #endregion

    #region rule sets

    public static Dictionary<RuleSetKeys, string> LoadRuleSet(int ruleSetId)
    {
        RuleSet ruleSet = db.RuleSets.Single(r => r.RuleSetId == ruleSetId);

        return new Dictionary<RuleSetKeys, string>
        {
           {RuleSetKeys.Name, ruleSet.RuleSetName},
           {RuleSetKeys.Formula, ruleSet.Formula}
        };
    }

    public static List<ListItem> GetRuleSets(string username)
    {
        var ruleSets = new List<ListItem>();
        var query =
            from r in db.RuleSets
            where r.User == username && r.Valid == true
            select r;

        foreach (RuleSet rules in query)
        {
            ruleSets.Add(new ListItem(rules.RuleSetName, rules.RuleSetId.ToString()));
        }

        return ruleSets;
    }
    #endregion

    #region city search

    public static DataTable GetCitySearchTable(bool doSearchAllUsers, string userPattern, bool doGetAllCities, string cityNamePattern)
    {
        // Perform city search based on input parameters. 
        IQueryable<CityInfo> query;
        if (doSearchAllUsers && doGetAllCities)
        {
            query = db.CityInfoes;
        }
        else if (doSearchAllUsers)
        {
            query = db.CityInfoes.Where(c => c.CityName.StartsWith(cityNamePattern));
        }
        else if (doGetAllCities)
        {
            query = db.CityInfoes.Where(c => c.User.StartsWith(userPattern));
        }
        else
        {
            query = db.CityInfoes.Where(c => c.User.StartsWith(userPattern) && c.CityName.StartsWith(cityNamePattern));
        }

        // Form table from search data. 
        DataTable citySearchTable = initDataTable(citySearchColumns);

        foreach (CityInfo city in query)
        {
            citySearchTable.Rows.Add(city.User, city.CityName, city.CitySize, city.AvailableFunds, city.CityInfoId);
        }

        return citySearchTable;
    }
    #endregion

    #region private helper functions

    private static double scoreCity(City city, string formula)
    {
        FormulaScore.FormulaScore scorer = new FormulaScore.FormulaScore();
        scorer.ScoringFormula = formula;

        // Load scoring values into scorer.
        List<string> scoringIdentifiers = FormulaScore.FormulaScore.FetchScoringIDs(formula);
        foreach (string scoringId in scoringIdentifiers)
        {
            if (GetCityValue.IsValueIdentifier(scoringId)) 
            {
                scorer.AddScoringValue(scoringId, GetCityValue.GetValue(scoringId, city));
            }
        }

        double score = scorer.CalculateScore();
        return score;
    }

    private static DataTable initDataTable(Dictionary<string, Type> columns)
    {
        var table = new DataTable();

        foreach (KeyValuePair<string, Type> heading in columns)
        {
            table.Columns.Add(heading.Key, heading.Value);
        }

        return table;
    }

    private static CityInfo getCity(int cityId)
    {
        return db.CityInfoes.Single(c => c.CityInfoId == cityId);
    }

    private static bool rankingExists(string rankingName, string username)
    {
        return db.ComparisonGroups.Any(g => g.ComparisonGroupName == rankingName && g.User == username);
    }
    #endregion

}