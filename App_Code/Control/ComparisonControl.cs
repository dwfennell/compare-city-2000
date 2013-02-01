using System;
using System.Collections.Generic;
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

    private IQueryable<ComparisonGroupMember> groupMembers;

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

    public void LoadRuleSet(int ruleSetId)
    {
        CurrentRuleSetId = ruleSetId;
        CurrentRuleSet = getRuleSet(ruleSetId);
    }

    #endregion

    #region getters

    public IQueryable<RuleSet> GetRuleSets()
    {
        var query =
            from r in db.RuleSets
            where r.User == CurrentUser
            select r;

        return query;
    }

    public IQueryable<ComparisonGroup> GetComparisonGroups()
    {
        var query =
            from g in db.ComparisonGroups
            where g.User == CurrentUser
            select g;

        return query;
    }

    public IQueryable<ComparisonGroupMember> GetComparisonGroupMembers()
    {
        var query =
            from c in db.ComparisonGroupMembers
            where c.ComparisonGroupId == CurrentComparisonGroupId
            select c;

        return query;
    }

    public string GetRuleSetName()
    {
        return CurrentRuleSet == null ? "" : CurrentRuleSet.RuleSetName;
    }

    public string GetRuleSetFormula()
    {
        return CurrentRuleSet == null ? "" : CurrentRuleSet.Formula;
    }

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