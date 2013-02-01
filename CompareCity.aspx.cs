using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CompareCity.Model;
using CompareCity.Control;

public partial class CompareCities : System.Web.UI.Page
{
    private ComparisonControl comparisonControl;

    protected void Page_Load(object sender, EventArgs e)
    {
        // TODO: Some caching here could be useful.
        comparisonControl = new ComparisonControl();

        if (String.IsNullOrWhiteSpace(RankingNameTextBox.Text))
        {
            RankingNameTextBox.Text = comparisonControl.GetUntitledRankingName();
        }

        // Populate rule set list.
        IQueryable<RuleSet> ruleSets = comparisonControl.GetRuleSets(SiteControl.Username);
        foreach (RuleSet rule in ruleSets)
        {
            ScoringRulesList.Items.Add(new ListItem(rule.RuleSetName, rule.RuleSetId.ToString()));
        }

        // Populate rankings list.
        IQueryable<ComparisonGroup> comparisonGroups = comparisonControl.GetComparisonGroups(SiteControl.Username);
        foreach (ComparisonGroup group in comparisonGroups)
        {
            RankingNameList.Items.Add(new ListItem(group.ComparisonGroupName, group.ComparisonGroupId.ToString()));
        }
    }

    // The id parameter name should match the DataKeyNames value set on the control
    public void CityRanksView_DeleteItem(int id)
    {

    }

    // The return type can be changed to IEnumerable, however to support
    // paging and sorting, the following parameters must be added:
    //     int maximumRows
    //     int startRowIndex
    //     out int totalRowCount
    //     string sortByExpression
    public IQueryable<ComparisonGroupMember> CityRanksView_GetData()
    {
        return comparisonControl.GetComparisonGroupMembers();
    }

    protected void ScoringRulesList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (setSelectedRuleSet())
        {
            saveComparisonGroup();
        }
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        if (!setSelectedRuleSet())
        {
            SaveStatusLabel.Text = "Not Saved. Please select a rule set.";
            return;
        }

        // Persist comparison group data.
        saveComparisonGroup();
    }

    protected void AddCitiesButton_Click(object sender, EventArgs e)
    {

    }

    protected void CalcRankingButton_Click(object sender, EventArgs e)
    {

    }

    protected void LoadRankButton_Click(object sender, EventArgs e)
    {
        // Get ranking 

    }

    private bool setSelectedRuleSet()
    {
        // Get rule set id, if it exists.
        int ruleSetId;
        if (!Int32.TryParse(ScoringRulesList.SelectedValue, out ruleSetId))
        {
            // No rule set selected.
            ruleSetId = -1;
            return false;
        }
        else
        {
            comparisonControl.CurrentRuleSetId = ruleSetId;
            return true;
        }
    }

    private void saveComparisonGroup()
    {
        string rankingName = RankingNameTextBox.Text.Trim();
        comparisonControl.SaveComparisonGroup(rankingName, SiteControl.Username);
        SaveStatusLabel.Text = "Saved!";
    }
}