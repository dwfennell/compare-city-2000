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

    private RankingControl rankControl = new RankingControl(SiteControl.Username);

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
                RankingNameTextBox.Text = rankControl.GetUntitledRankingName();
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
        string rankingName = RankingNameTextBox.Text.Trim();
        storeRanking();
        rankControl.SaveRanking();
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

        rankControl.LoadRuleSet(ruleSetId);
        rankControl.SaveRanking();

        RuleSetLabel.Text = rankControl.GetRuleSetName();
        RuleFormulaLabel.Text = rankControl.GetRuleSetFormula();

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

        rankControl.LoadComparisonGroup(comparisonGroupId);

        RankingNameTextBox.Text = rankControl.CurrentRankingName;

        RuleSetLabel.Text = rankControl.GetRuleSetName();
        RuleFormulaLabel.Text = rankControl.GetRuleSetFormula();

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

        DataTable cities = rankControl.GetCitySearchTable(allUsers, userPattern, allCityNames, cityNamePattern);

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
            // Update control object before adding a new city to ranking.
            storeRanking();

            // Fetch row and row data.
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = CitySearchGridView.Rows[index];

            // Fetch cityId from row data.
            int cityId;
            Int32.TryParse(row.Cells[citySearchIdIndex].Text, out cityId);

            rankControl.AddRankedCity(cityId);
            DataTable rankedCities = rankControl.GetRankedCitiesTable();

            bindToGridview(CityRankingGridView, rankedCities);
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

    private bool storeRanking()
    {
        // Store ranking name.
        string name = RankingNameTextBox.Text.Trim();
        rankControl.CurrentRankingName = name;
        
        // Store rule set, if present.
        // TODO: Revisit this. It is possible to set a rule set here that has not been explicitly "loaded".
        if (hasSelectedRuleSet())
        {
            int ruleSetId = getSelectedRuleSetId();
            rankControl.LoadRuleSet(ruleSetId);
        }

        return true;
    }

    private void populateRulesList()
    {
        List<ListItem> ruleSets = rankControl.GetRuleSets();
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
        List<ListItem> rankings = rankControl.GetRankings();
        rankings.Insert(0, new ListItem(rankingListDefaultText, "-1"));

        // Configure listbox for use with a List<ListItem>. 
        RankingNameList.DataTextField = "Text";
        RankingNameList.DataValueField = "Value";

        RankingNameList.DataSource = null;
        RankingNameList.DataSource = rankings;
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