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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // TODO: Some caching here could be useful.
        comparisonControl = new ComparisonControl(SiteControl.Username);

        if (!IsPostBack)
        {
            if (String.IsNullOrWhiteSpace(RankingNameTextBox.Text))
            {
                RankingNameTextBox.Text = comparisonControl.GetUntitledRankingName();
            }

            // Populate rule set list.
            IQueryable<RuleSet> ruleSets = comparisonControl.GetRuleSets();
            foreach (RuleSet rule in ruleSets)
            {
                ScoringRulesList.Items.Add(new ListItem(rule.RuleSetName, rule.RuleSetId.ToString()));
            }

            // Populate rankings list.
            IQueryable<ComparisonGroup> comparisonGroups = comparisonControl.GetComparisonGroups();
            foreach (ComparisonGroup group in comparisonGroups)
            {
                RankingNameList.Items.Add(new ListItem(group.ComparisonGroupName, group.ComparisonGroupId.ToString()));
            }
        }
    }

    // The return type can be changed to IEnumerable, however to support
    // paging and sorting, the following parameters must be added:
    //     int maximumRows
    //     int startRowIndex
    //     out int totalRowCount
    //     string sortByExpression
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IQueryable<ComparisonGroupMember> CityRanksView_GetData()
    {
        return comparisonControl.GetComparisonGroupMembers();
    }

    // The id parameter name should match the DataKeyNames value set on the control
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public void CityRanksView_DeleteItem(int id)
    {

    }

    #region button click events

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SaveButton_Click(object sender, EventArgs e)
    {
        if (!hasSelectedRuleSet())
        {
            SaveStatusLabel.Text = "Not Saved. Please select a rule set.";
            return;
        }

        string comparisonName = RankingNameTextBox.Text.Trim();

        if (!String.IsNullOrWhiteSpace(comparisonName))
        {
            comparisonControl.CurrentComparisonName = comparisonName;
            comparisonControl.SaveComparisonGroup();
        }
        else
        {
            SaveStatusLabel.Text = "Not Saved. Please enter a ranking name.";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void AddCitiesButton_Click(object sender, EventArgs e)
    {
        // TODO: Code 'add cities'.
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CalcRankingButton_Click(object sender, EventArgs e)
    {
        // TODO: Code ranking code.
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LoadRuleSetButton_Click(object sender, EventArgs e)
    {
        int ruleSetId = getSelectedRuleSetId();

        comparisonControl.LoadRuleSet(ruleSetId);
        comparisonControl.SaveComparisonGroup();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LoadRankButton_Click(object sender, EventArgs e)
    {
        // Get comparison group id, if one is selected. 
        int comparisonGroupId;
        if (!Int32.TryParse(RankingNameList.SelectedValue, out comparisonGroupId))
        {
            LoadStatusLabel.Text = "No ranking selected.";
            return;
        }

        comparisonControl.LoadComparisonGroup(comparisonGroupId);

        RankingNameTextBox.Text = comparisonControl.CurrentComparisonName;

        RuleSetLabel.Text = comparisonControl.GetRuleSetName();
        RuleFormulaLabel.Text = comparisonControl.GetRuleSetFormula();

        // TODO: Load group members.

    }
    #endregion

    #region private helper functions

    private int getSelectedRuleSetId()
    {
        int ruleSetId;
        return Int32.TryParse(ScoringRulesList.SelectedValue, out ruleSetId) ? ruleSetId : -1;
    }

    private bool hasSelectedRuleSet()
    {
        return getSelectedRuleSetId() != -1;
    }

    private void saveComparisonGroup()
    {
        string rankingName = RankingNameTextBox.Text.Trim();
        comparisonControl.SaveComparisonGroup();
        SaveStatusLabel.Text = "Saved!";
    }
    #endregion
}