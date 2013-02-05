using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

using CompareCity.Model;

/// <summary>
/// 
/// </summary>
public class RankingControl
{
    #region private 'constants'

    private readonly Dictionary<string, Type> citySearchColumns = new Dictionary<string, Type>
    {
        {"User", typeof(string)}, 
        {"City Name", typeof(string)}, 
        {"Size", typeof(int)},
        {"Funds", typeof(int)},
        {"CityID", typeof(int)}
    };

    private readonly Dictionary<string, Type> cityRankingsColumns = new Dictionary<string, Type>
    {
        {"ID", typeof(int)},
        {"City Name", typeof(string)},
        {"User", typeof(string)},
        {"Score", typeof(double)}
    };
    #endregion

    #region private member variables

    private DataTable cityRankingsTable;
    private DataTable citySearchTable;

    // TODO: Once again, a context pool might be useful.
    private static DatabaseContext db = new DatabaseContext();
    #endregion

    #region public properties

    public string CurrentRankingName { get; set; }
    public int CurrentRankingId { get; private set; }
    public string CurrentUser { get; private set; }
    public int CurrentRuleSetId { get; private set; }
    public RuleSet CurrentRuleSet { get; private set; }
    #endregion

    #region contructors

    public RankingControl(string user)
	{
        // Default to -1 to match no ids if grouping id is not set. 
        CurrentRankingId = -1;
        CurrentRuleSetId = -1;
        CurrentRuleSet = null;
        CurrentUser = user;

        cityRankingsTable = initDataTable(cityRankingsColumns);
        citySearchTable = initDataTable(citySearchColumns);
	}
    #endregion

    #region ranking methods

    public void SaveRanking()
    {
        if (CurrentRankingId == -1)
        {
            // Create new comparison group.
            var newGroup = new ComparisonGroup
            {
                ComparisonGroupName = CurrentRankingName,
                RuleSetId = CurrentRuleSetId,
                RuleSet = CurrentRuleSet,
                User = CurrentUser
            };

            db.ComparisonGroups.Add(newGroup);
        }
        else
        {
            // Update existing comparison group.
            var cg = db.ComparisonGroups.First(i => i.ComparisonGroupId == CurrentRankingId);

            cg.ComparisonGroupName = CurrentRankingName;
            cg.RuleSetId = CurrentRuleSetId;
            cg.RuleSet = CurrentRuleSet;
        }
        db.SaveChanges();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<ListItem> GetRankings()
    {
        var comparisonGroups = new List<ListItem>();
        var query =
            from g in db.ComparisonGroups
            where g.User == CurrentUser
            select g;

        foreach (ComparisonGroup group in query)
        {
            comparisonGroups.Add(new ListItem(group.ComparisonGroupName, group.ComparisonGroupId.ToString()));
        }

        return comparisonGroups;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public void LoadComparisonGroup(int id)
    {
        ComparisonGroup group = getComparisonGroup(id);

        CurrentRankingId = id;
        CurrentRankingName = group.ComparisonGroupName;
        CurrentRuleSetId = group.RuleSetId;
        CurrentRuleSet = group.RuleSet;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetUntitledRankingName()
    {
        return "Untitled";
    }
    #endregion

    #region city ranking table methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cityId"></param>
    public void AddRankedCity(int cityId)
    {
        // Create new ComparisonGroupMember
        var newRankedCity = new ComparisonGroupMember
        {
            ComparisonGroupId = CurrentRankingId,
            CityInfoId = cityId,
            TotalScore = 0.0
        };
        db.ComparisonGroupMembers.Add(newRankedCity);
        db.SaveChanges();

        var city = getCity(cityId);
        // Construct new city rankings table row.
        cityRankingsTable.Rows.Add(city.CityName, city.User, newRankedCity.TotalScore);
    }

    /// <summary>
    /// Delete a city ranking.
    /// </summary>
    /// <param name="cityId"></param>
    public void RemoveRankedCity(int cityId, int tableIndex)
    {
        // Delete ComparisonGroupMember from database.
        ComparisonGroupMember groupMember = db.ComparisonGroupMembers
            .First(m => m.CityInfoId == cityId && m.ComparisonGroupId == CurrentRankingId);
        db.ComparisonGroupMembers.Remove(groupMember);

        // Remove city rankings table row. 
        cityRankingsTable.Rows.RemoveAt(tableIndex);
    }

    public DataTable GetRankedCitiesTable()
    {
        return cityRankingsTable;
    }

    #endregion

    #region city searching

    /// <summary>
    /// 
    /// </summary>
    /// <param name="doSearchAllUsers"></param>
    /// <param name="userPattern"></param>
    /// <param name="doGetAllCities"></param>
    /// <param name="cityNamePattern"></param>
    /// <returns></returns>
    public DataTable GetCitySearchTable(bool doSearchAllUsers, string userPattern, bool doGetAllCities, string cityNamePattern)
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
        if (citySearchTable == null)
        {
            citySearchTable = initDataTable(citySearchColumns);
        }
        citySearchTable.Rows.Clear();
        foreach (CityInfo city in query)
        {
            citySearchTable.Rows.Add(city.User, city.CityName, city.CitySize, city.AvailableFunds, city.CityInfoId);
        }

        return citySearchTable;
    }
    #endregion

    #region rule set methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ruleSetId"></param>
    public void LoadRuleSet(int ruleSetId)
    {
        CurrentRuleSetId = ruleSetId;
        CurrentRuleSet = getRuleSet(ruleSetId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<ListItem> GetRuleSets()
    {
        var ruleSets = new List<ListItem>();
        var query =
            from r in db.RuleSets
            where r.User == CurrentUser
            select r;

        foreach (RuleSet rules in query)
        {
            ruleSets.Add(new ListItem(rules.RuleSetName, rules.RuleSetId.ToString()));
        }

        return ruleSets;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetRuleSetName()
    {
        return CurrentRuleSet == null ? "" : CurrentRuleSet.RuleSetName;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetRuleSetFormula()
    {
        return CurrentRuleSet == null ? "" : CurrentRuleSet.Formula;
    }
    #endregion

    #region private helper methods

    private ComparisonGroup getComparisonGroup(int id)
    {
        return db.ComparisonGroups.First(i => i.ComparisonGroupId == id);
    }

    private RuleSet getRuleSet(int id)
    {
        return db.RuleSets.First(i => i.RuleSetId == id);
    }

    private DataTable initDataTable(Dictionary<string, Type> columns)
    {
        var table = new DataTable();

        foreach (KeyValuePair<string, Type> heading in columns)
        {
            table.Columns.Add(heading.Key, heading.Value);
        }

        return table;
    }

    private CityInfo getCity(int cityId)
    {
        return db.CityInfoes.First(c => c.CityInfoId == cityId);
    }
    #endregion
}