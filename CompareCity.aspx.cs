using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

using CompareCity.Control;

public partial class CompareCities : System.Web.UI.Page
{
    #region private constants

    private static readonly string rankingListDefaultText = "--Select Ranking--";
    private static readonly string ruleSetListDefaultText = "--Select Rule Set--";
    
    private static readonly int citySearchIdIndex = 5;

    private static readonly Color ruleSetPresentColor = Color.DarkGreen;
    private static readonly Color ruleSetMissingColor = Color.Red;
    #endregion

    private bool ruleSetLoaded = false;

    private enum sessionKeys { rankedCitiesTable };

    //private RankingControl rankControl = new RankingControl(SiteControl.Username);

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
                RankingNameTextBox.Text = RankingControl.GetUntitledRankingName();
            }

            // Populate dropdown lists. 
            populateRulesList();
            populateRankingsList();

            setRuleSetTextColor(false);

            // Init ranking GridView.
            DataTable rankedCities = RankingControl.GetEmptyRankingTable();
            Session["rankedCities"] = rankedCities;
            // Bind here so EmptyDataTemplate will show itself in the gridview.
            bindToGridview(CityRankingGridView, rankedCities);
        }
        else
        {
            // Reload ranked cities table data.
            DataTable rankedCities = (DataTable)Session["rankedCities"];
            bindToGridview(CityRankingGridView, rankedCities);
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
        // TODO: Validate rule set and name are set.

        saveRanking();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CalcRankingButton_Click(object sender, EventArgs e)
    {
        var rankedCities = (DataTable)Session["rankedCities"];
        var ruleSetId = (int)Session["ruleSetId"];

        rankedCities = RankingControl.ScoreCities(rankedCities, ruleSetId);
        bindToGridview(CityRankingGridView, rankedCities);
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

        try
        {
            Dictionary<RankingControl.RuleSetKeys, string> ruleSetInfo = RankingControl.LoadRuleSet(ruleSetId);

            RuleSetLabel.Text = ruleSetInfo[RankingControl.RuleSetKeys.Name];
            RuleFormulaLabel.Text = ruleSetInfo[RankingControl.RuleSetKeys.Formula];

            setRuleSetTextColor(true);

            ruleSetLoaded = true;
            Session["ruleSetId"] = ruleSetId;
        }
        catch (InvalidOperationException)
        {
            // Rule set not found. 
            RuleSetLabel.Text = "Rule set not found!";
            RuleFormulaLabel.Text = "Rule set not found!";
            setRuleSetTextColor(false);
            return;
        }

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
        int rankingId;
        if (!Int32.TryParse(RankingNameList.SelectedValue, out rankingId))
        {
            LoadStatusLabel.Text = "No ranking selected.";
            return;
        }

        try
        {
            Dictionary<RankingControl.RankingKeys, string> rankingInfo = RankingControl.LoadRanking(rankingId);

            RankingNameTextBox.Text = rankingInfo[RankingControl.RankingKeys.Name];

            RuleSetLabel.Text = rankingInfo[RankingControl.RankingKeys.RuleSetName];
            RuleFormulaLabel.Text = rankingInfo[RankingControl.RankingKeys.RuleSetFormula];
            setRuleSetTextColor(true);
            ScoringRulesList.SelectedIndex = 0;

            Session["ruleSetId"] = Int32.Parse(rankingInfo[RankingControl.RankingKeys.RuleSetId]);

            // TODO: Load group members.

        }
        catch (InvalidOperationException)
        {
            LoadStatusLabel.Text = "Ranking not found!";
            return;
        }
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

        DataTable cities = RankingControl.GetCitySearchTable(allUsers, userPattern, allCityNames, cityNamePattern);

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

            //DataTable rankedCities = CityRankingGridView.DataSource as DataTable;
            DataTable rankedCities = (DataTable)Session["rankedCities"];

            rankedCities = RankingControl.AddRankedCity(rankedCities, cityId);
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

    private void saveRanking()
    {
        // Store ranking name.
        string rankingName = RankingNameTextBox.Text.Trim();
        int ruleSetId = -1;

        // Store rule set, if present.
        if (ruleSetLoaded && hasSelectedRuleSet())
        {
            ruleSetId = getSelectedRuleSetId();
        }

        // TODO: Get "ranked" cities and save them. 
    }


    private int getSelectedRuleSetId()
    {
        int ruleSetId;
        return Int32.TryParse(ScoringRulesList.SelectedValue, out ruleSetId) ? ruleSetId : -1;
    }

    private bool hasSelectedRuleSet()
    {
        return getSelectedRuleSetId() != -1;
    }

    private void populateRulesList()
    {
        List<ListItem> ruleSets = RankingControl.GetRuleSets(SiteControl.Username);
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
        List<ListItem> rankings = RankingControl.GetRankings(SiteControl.Username);
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

    private DataRowCollection gridViewRowsToDataTableRows(GridViewRowCollection gridViewRows)
    {
        // Using DataTable since DataRowCollection has no contstructors. 
        DataTable table = new DataTable();
        
        foreach (GridViewRow gridRow in gridViewRows)
        {
            table.Rows.Add(gridRow);
        }

        return table.Rows;
    }

    private void setRuleSetTextColor(bool ruleSetPresent)
    {
        if (ruleSetPresent)
        {
            RuleSetLabel.ForeColor = ruleSetPresentColor;
            RuleFormulaLabel.ForeColor = ruleSetPresentColor;
        }
        else
        {
            RuleSetLabel.ForeColor = ruleSetMissingColor;
            RuleFormulaLabel.ForeColor = ruleSetMissingColor;
        }
    }

    #endregion
}