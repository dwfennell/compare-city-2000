using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CompareCity.Control;

public partial class CompareCities : System.Web.UI.Page
{
    private static readonly string rankingListDefaultText = "--Select Ranking--";
    private static readonly string ruleSetListDefaultText = "--Select Rule Set--";
    private static readonly int citySearchIdIndex = 5;

    private ComparisonControl comparisonControl = new ComparisonControl(SiteControl.Username);

    /// <summary>
    /// Page load event handler.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (String.IsNullOrWhiteSpace(RankingNameTextBox.Text))
            {
                RankingNameTextBox.Text = comparisonControl.GetUntitledRankingName();
            }

            populateRulesList();
            populateRankingsList();
        }
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

        DataTable cities = comparisonControl.GetCitiesTable(allUsers, userPattern, allCityNames, cityNamePattern);

        // Display city search results
        bindToGridview(CitySearchGridView, cities);
    }

    /// <summary>
    /// Event handler for <c>CitySearchGridView</c> commands.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CitySearchGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddCity")
        {
            // Fetch row and row data.
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = CitySearchGridView.Rows[index];

            // Fetch cityId from row data.
            int cityId;
            Int32.TryParse(row.Cells[citySearchIdIndex].Text, out cityId);

            DataTable rankedMembers = comparisonControl.AddRankedCity(cityId);

            bindToGridview(CityRanksView, rankedMembers);
        }

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

    private void populateRulesList()
    {
        List<ListItem> ruleSets = comparisonControl.GetRuleSets();
        ruleSets.Insert(0, new ListItem(ruleSetListDefaultText, "-1"));

        // Configure listbox for use with a List<ListItem>.
        ScoringRulesList.DataTextField = "Text";
        ScoringRulesList.DataValueField = "Value";

        ScoringRulesList.DataSource = null;
        ScoringRulesList.DataSource = ruleSets;
        ScoringRulesList.DataBind();
    }

    private void populateRankingsList()
    {
        // Populate rankings list.
        List<ListItem> comparisonGroups = comparisonControl.GetComparisonGroups();
        comparisonGroups.Insert(0, new ListItem(rankingListDefaultText, "-1"));

        // Configure listbox for use with a List<ListItem>. 
        RankingNameList.DataTextField = "Text";
        RankingNameList.DataValueField = "Value";

        RankingNameList.DataSource = null;
        RankingNameList.DataSource = comparisonGroups;
        RankingNameList.DataBind();
    }

    private void bindToGridview(GridView grid, DataTable table)
    {
        grid.DataSource = null;
        grid.DataSource = table;
        grid.DataBind();
    }

    #endregion
}