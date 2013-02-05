using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

using CompareCity.Model;

/// <summary>
/// Summary description for ComparisonControl
/// </summary>
public class ComparisonControl
{
    // TODO: Once again, a context pool might be useful.
    private static DatabaseContext db = new DatabaseContext();

    public string CurrentComparisonName { get; set; }
    public int CurrentComparisonGroupId { get; private set; }
    public string CurrentUser { get; private set; }

    public int CurrentRuleSetId { get; private set; }
    public RuleSet CurrentRuleSet { get; private set; }

    #region contructors

    public ComparisonControl(string user)
	{
        // Default to -1 to match no ids if grouping id is not set. 
        CurrentComparisonGroupId = -1;
        CurrentRuleSetId = -1;
        CurrentUser = user;
	}
    #endregion

    #region persistance

    public void SaveComparisonGroup()
    {
        if (CurrentComparisonGroupId == -1)
        {
            // Create new comparison group.
            var newGroup = new ComparisonGroup
            {
                ComparisonGroupName = CurrentComparisonName,
                RuleSetId = CurrentRuleSetId,
                RuleSet = CurrentRuleSet,
                User = CurrentUser
            };

            db.ComparisonGroups.Add(newGroup);
        }
        else
        {
            // Update existing comparison group.
            var cg = db.ComparisonGroups.First(i => i.ComparisonGroupId == CurrentComparisonGroupId);

            cg.ComparisonGroupName = CurrentComparisonName;
            cg.RuleSetId = CurrentRuleSetId;
            cg.RuleSet = CurrentRuleSet;
        }
        db.SaveChanges();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public void LoadComparisonGroup(int id)
    {
        ComparisonGroup group = getComparisonGroup(id);

        CurrentComparisonGroupId = id;
        CurrentComparisonName = group.ComparisonGroupName;
        CurrentRuleSetId = group.RuleSetId;
        CurrentRuleSet = group.RuleSet;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ruleSetId"></param>
    public void LoadRuleSet(int ruleSetId)
    {
        CurrentRuleSetId = ruleSetId;
        CurrentRuleSet = getRuleSet(ruleSetId);
    }

    #endregion

    #region getters

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
    public List<ListItem> GetComparisonGroups()
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
    /// <returns></returns>
    public IQueryable<ComparisonGroupMember> GetComparisonGroupMembers()
    {
        var query =
            from c in db.ComparisonGroupMembers
            where c.ComparisonGroupId == CurrentComparisonGroupId
            select c;

        return query;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="doSearchAllUsers"></param>
    /// <param name="userPattern"></param>
    /// <param name="doGetAllCities"></param>
    /// <param name="cityNamePattern"></param>
    /// <returns></returns>
    public DataTable GetCities(bool doSearchAllUsers, string userPattern, bool doGetAllCities, string cityNamePattern)
    {
        // Perform city search based on parameters. 
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

        DataTable table = new DataTable();
        
        // Set up table columns.
        table.Columns.Add("User", typeof(string));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Size", typeof(int));
        table.Columns.Add("Funds", typeof(int));

        // Load search data.
        foreach (CityInfo city in query)
        {
            table.Rows.Add(city.User, city.CityName, city.CitySize, city.AvailableFunds);
        }

        return table;
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetUntitledRankingName()
    {
        return "Untitled";
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

    #endregion
}