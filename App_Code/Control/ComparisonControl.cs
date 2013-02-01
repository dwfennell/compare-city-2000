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

    public int CurrentComparisonGroupId { get; set; }
    public int CurrentRuleSetId { get; set; }

    public ComparisonControl()
	{
        // Default to -1 to match no ids if grouping id is not set. 
        CurrentComparisonGroupId = -1;
        CurrentRuleSetId = -1;
	}

    public void SaveComparisonGroup(string name, string user) 
    {
        if (CurrentComparisonGroupId != -1) 
        {
            // Update existing comparison group.
            var cg = db.ComparisionGroups.First(i => i.ComparisonGroupId == CurrentComparisonGroupId);
            
            cg.ComparisonGroupName = name;
            cg.RuleSetId = CurrentRuleSetId;
        }
        else
        {
            // Create new comparison group.
            var cg = new ComparisonGroup
            {
                ComparisonGroupName = name,
                RuleSetId = CurrentRuleSetId,
                User = user
            };
            db.ComparisionGroups.Add(cg);
        }
        db.SaveChanges();
    }

    public ComparisonGroup GetComparisonGroup(int id)
    {
        return db.ComparisionGroups.First(i => i.ComparisonGroupId == id);
    }

    public IQueryable<RuleSet> GetRuleSets(string user)
    {
        var query =
            from r in db.RuleSets
            where r.User == user
            select r;

        return query;
    }

    public IQueryable<ComparisonGroup> GetComparisonGroups(string user)
    {
        var query =
            from g in db.ComparisionGroups
            where g.User == user
            select g;

        return query;
    }

    public IQueryable<ComparisonGroupMember> GetComparisonGroupMembers()
    {
        var query =
            from c in db.ComparisionGroupMembers
            where c.ComparisonGroupId == CurrentComparisonGroupId
            select c;

        return query;
    }

    public string GetUntitledRankingName()
    {
        return "Untitled";
    }
}