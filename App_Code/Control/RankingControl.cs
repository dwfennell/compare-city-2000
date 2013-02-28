using System;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;
using System.Collections.Generic;

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

    private static readonly Dictionary<string, Type> citySearchTableColumns = new Dictionary<string, Type>
    {
        {"User", typeof(string)}, 
        {"City Name", typeof(string)}, 
        {"Size", typeof(int)},
        {"Funds", typeof(int)},
        {"CityID", typeof(int)}
    };

    private static readonly Dictionary<string, Type> rankingMemberTableColumns = new Dictionary<string, Type>
    {
        {"City", typeof(string)},
        {"Score", typeof(double)},
        {"User", typeof(string)}
    };

    private const int rankingCityIdColumn = 0;
    private const int rankingScoreColumn = 3;
    #endregion

    public enum RuleSetKeys { Name, Formula };
    public enum RankingKeys { Name, RuleSetId, RuleSetName, RuleSetFormula};

    #region rankings

    public static DataTable GetEmptyRankingMemberTable()
    {
        return initDataTable(rankingMemberTableColumns);
    }

    public static DataTable GetRankingMemberTable(int rankingId)
    {
        return constructRankingMemberTable(rankingId);
    }

    public static void AddRankedCity(int rankingId, int cityId)
    {
        if (db.RankingMembers.Any(r => r.CityInfoId == cityId && r.RankingId == rankingId))
        {
            // City is already being ranked.
            return;
        }

        CityInfo city = getCity(cityId);

        RankingMember newRankingMember = new RankingMember
        {
            RankingId = rankingId,
            CityInfoId = cityId,
            Score = 0.0,
            User = city.User
        };
        db.RankingMembers.Add(newRankingMember);

        db.SaveChanges();
    }

    public static List<ListItem> GetRankings(string username)
    {
        var rankings = new List<ListItem>();
        var query =
            from g in db.Rankings
            where g.User == username
            select g;

        foreach (Ranking group in query)
        {
            rankings.Add(new ListItem(group.RankingName, group.RankingId.ToString()));
        }

        return rankings;
    }

    public static DataTable ScoreCities(int rankingId, int ruleSetId)
    {
        CityParser parser = new CityParser();

        // Fetch rule set formula for scoring.
        string formula = LoadRuleSet(ruleSetId)[RuleSetKeys.Formula];

        List<RankingMember> rankingMembers = getRankingMembers(rankingId);

        CityInfo city;
        City parsedCity;
        foreach (RankingMember rankingMember in rankingMembers)
        {
            city = getCity(rankingMember.CityInfoId);

            // Perform full city parse. 
            // TODO: Serialize the results from this and save to reduce redundant computation.
            using (FileStream cityStream = File.OpenRead(city.FilePath))
            {
                parsedCity = parser.ParseCityFile(cityStream);
            }

            // Score city!
            rankingMember.Score = scoreCity(parsedCity, formula);
        }
        db.SaveChanges();

        return constructRankingMemberTable(rankingId);
    }

    public static void SaveRankingCities(string rankingName, string user, DataTable citiesTable)
    {
        // Fetch parent ranking.
        Ranking ranking = db.Rankings.Single(c => c.RankingName == rankingName && c.User == user);

        // Delete old members.
        foreach (RankingMember rankedCity in db.RankingMembers.Where(c => c.RankingId == ranking.RankingId))
        {
            db.RankingMembers.Remove(rankedCity);
        }

        // Add new members.
        int cityId;
        double score;
        for (int i = 0; i < citiesTable.Rows.Count; i++)
        {
            cityId = (int)citiesTable.Rows[i][rankingCityIdColumn];
            score = (double)citiesTable.Rows[i][rankingScoreColumn];

            db.RankingMembers.Add(new RankingMember
            {
                CityInfoId = cityId,
                Score = score,
                RankingId = ranking.RankingId
            });
        }

        db.SaveChanges();
    }

    public static int SaveNewRanking(string username, string rankingName, int ruleSetId)
    {
        // Create new ranking.
        Ranking newRanking = new Ranking
        {
            RankingName = rankingName,
            RuleSetId = ruleSetId,
            User = username
        };
        db.Rankings.Add(newRanking);

        db.SaveChanges();
        return newRanking.RankingId;
    }

    public static void SaveRanking(int rankingId, string rankingName, int ruleSetId)
    {
        // Update existing ranking. 
        Ranking ranking = db.Rankings.First(r => r.RankingId == rankingId);

        ranking.RankingName = rankingName;
        ranking.RuleSetId = ruleSetId;

        db.SaveChanges();
    }

    public static DataTable LoadRankingCities(string rankingName, string user)
    {
        Ranking ranking = db.Rankings.Single(c => c.RankingName == rankingName && c.User == user);

        DataTable rankedCitiesTable = initDataTable(rankingMemberTableColumns);
        CityInfo city;
        List<RankingMember> rankedCities = db.RankingMembers.Where(c => c.RankingId == ranking.RankingId).ToList();
        foreach (RankingMember rankedCity in rankedCities)
        {
            city = db.CityInfoes.Single(c => c.CityInfoId == rankedCity.CityInfoId);
            rankedCitiesTable.Rows.Add(rankedCity.CityInfoId, city.CityName, user, rankedCity.Score);
        }

        return rankedCitiesTable;
    }

    public static Dictionary<RankingKeys, string> LoadRanking(int rankingId)
    {
        Ranking ranking = db.Rankings.Single(r => r.RankingId == rankingId);

        // Load rule set info, if available.
        string ruleSetName = "";
        string ruleSetFormula = "";
        string ruleSetId = "";
        if (ranking.RuleSetId > 0)
        {
            RuleSet ruleSet = db.RuleSets.Single(r => r.RuleSetId == ranking.RuleSetId);
            ruleSetName = ruleSet.RuleSetName;
            ruleSetFormula = ruleSet.Formula;
            ruleSetId = ruleSet.RuleSetId.ToString();
        }

        return new Dictionary<RankingKeys, string>
        {
            {RankingKeys.Name, ranking.RankingName},
            {RankingKeys.RuleSetId, ruleSetId},
            {RankingKeys.RuleSetName, ruleSetName},
            {RankingKeys.RuleSetFormula, ruleSetFormula}
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

    public static DataTable GetCitySearchTable(string currentUser, bool doSearchAllUsers, bool onlyCurrentUser, string userPattern, bool doGetAllCities, string cityNamePattern)
    {
        // Build query from search conditions.

        IQueryable<CityInfo> query;

        // Set user conditions.
        if (doSearchAllUsers)
        {
            query = from c in db.CityInfoes select c;
        }
        else if (onlyCurrentUser)
        {
            query = from c in db.CityInfoes where c.User == currentUser select c;
        } else {
            query = from c in db.CityInfoes where c.User.StartsWith(userPattern) select c;
        }

        // Set cityname conditions.
        if (!doGetAllCities)
        {
            query = query.Where(c => c.CityName.StartsWith(cityNamePattern));
        }

        // Construct table from search data. 
        
        DataTable citySearchTable = initDataTable(citySearchTableColumns);

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

        return scorer.CalculateScore();
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

    private static DataTable constructRankingMemberTable(int rankingId)
    {
        // TODO: This method is both inefficient and oft-run. Consider refactoring to be less db-intensive.
        //       Perhaps instead of loading the CityInfo class a description string can be generated and stored in the RankingMember when it is created... 
        //       ...Or letting Entity Framework deal with the CityInfo db access through a Virtual field.

        DataTable table = initDataTable(rankingMemberTableColumns);

        List<RankingMember> rankingMembers = getRankingMembers(rankingId);
        CityInfo city;
        foreach (RankingMember rankingMember in rankingMembers)
        {
            city = getCity(rankingMember.CityInfoId);
            table.Rows.Add(city.CityName, rankingMember.Score, rankingMember.User);
        }

        return table;
    }

    private static List<RankingMember> getRankingMembers(int rankingId)
    {
        return (from m in db.RankingMembers
                where m.RankingId == rankingId
                select m).ToList();
    }

    private static CityInfo getCity(int cityId)
    {
        return db.CityInfoes.Single(c => c.CityInfoId == cityId);
    }

    private static bool rankingExists(string rankingName, string username)
    {
        return db.Rankings.Any(g => g.RankingName == rankingName && g.User == username);
    }
    #endregion

}