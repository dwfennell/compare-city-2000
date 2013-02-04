using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CompareCity.Model;
using CompareCity.Control;

public partial class CompareCities : System.Web.UI.Page
{
    private ComparisonControl comparisonControl = new ComparisonControl(SiteControl.Username);

    private DataTable foundCities;

    /// <summary>
    /// Page load event handler.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // TODO: Some caching here could be useful.
        //comparisonControl = new ComparisonControl(SiteControl.Username);

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

    #region ranking events

    /// <summary>
    /// Event handler for <c>SaveButton</c> click. Saves the current comparison.
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
    protected void CalcRankingButton_Click(object sender, EventArgs e)
    {
        // TODO: Code ranking code.
    }

    /// <summary>
    /// Event handler for <c>LoadRuleSetButton</c> click.
    /// Loads the rule set indicated by <c>ScoringRulesList</c> and resets city scoring, if necessary.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LoadRuleSetButton_Click(object sender, EventArgs e)
    {
        int ruleSetId = getSelectedRuleSetId();

        comparisonControl.LoadRuleSet(ruleSetId);
        comparisonControl.SaveComparisonGroup();

        RuleSetLabel.Text = comparisonControl.GetRuleSetName();
        RuleFormulaLabel.Text = comparisonControl.GetRuleSetFormula();

        // TODO: Check if scoring was done using previous rule set and clear those scores.
    }

    /// <summary>
    /// Event handler for <c>LoadRankButton</c> click.
    /// Loads a previous comparison ranking.
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


    #region city search events

    /// <summary>
    /// Event handler for <c>FindCitiesButton</c> click. 
    /// Evaluates city search parameters and loads city data into <c>CitySearchGridView</c>.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FindCitiesButton_Click(object sender, EventArgs e)
    {
        // Gather search conditions.
        bool allUsers = CitySearchUserCheckBox.Checked;
        bool allCityNames = CitySearchCityNameCheckBox.Checked;
        string userPattern = CitySearchUserTextBox.Text.Trim();
        string cityNamePattern = CitySeachCityNameTextBox.Text.Trim();

        DataTable cities = comparisonControl.GetCities(allUsers, userPattern, allCityNames, cityNamePattern);

        // Display city search results
        CitySearchGridView.DataSource = null;
        CitySearchGridView.DataSource = cities;
        CitySearchGridView.DataBind();
    }

    /// <summary>
    /// Event handler for <c>CitySearchUserCheckBox</c> check changed. 
    /// Toggles user search textbox useability.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CitySearchUserCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        CitySearchUserTextBox.Enabled = !CitySearchUserCheckBox.Checked;
    }

    /// <summary>
    /// Event handler for <c>CitySearchCityNameCheckBox</c> check changed.
    /// Toggles city name textbox useability.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CitySearchCityNameCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        CitySeachCityNameTextBox.Enabled = !CitySearchCityNameCheckBox.Checked;
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