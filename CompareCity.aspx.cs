﻿using System;
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

    private static readonly string rankingListDefaultText = "--Select Scoring Group--";
    private static readonly string ruleSetListDefaultText = "--Select Formula--";
    
    private static readonly int citySearchIdIndex = 5;

    private static readonly Color statusColorHappy = Color.DarkGreen;
    private static readonly Color statusColorSad = Color.Red;
    private static readonly Color ruleSetPresentColor = Color.Black;
    private static readonly Color ruleSetMissingColor = Color.Red;
    #endregion

    /// <summary>
    /// Page load event handler.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (String.IsNullOrWhiteSpace(SiteControl.Username))
        {
            Response.Redirect(SiteControl.AnonRedirect);
        }

        if (!IsPostBack)
        {
            // Populate dropdown lists. 
            populateRulesList();
            populateRankingsList();

            setRuleSetTextColor(false);

            // Bind empty data to Ranking gridview so the EmptyDataTemplate will show itself.
            bindToGridview(CityRankingGridView, new DataTable());
        }
        else
        {
            if (!sender.Equals(CitySearchGridView))
            {
                // Reload ranked cities table data.
                if (ViewState["rankingId"] != null)
                {
                    DataTable rankedCities = RankingControl.GetRankingMemberTable((int)ViewState["rankingId"]);
                    bindToGridview(CityRankingGridView, rankedCities);
                }
            }
        }
    }

    #region ranking events

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CalcRankingButton_Click(object sender, EventArgs e)
    {
        int rankingId = (int)ViewState["rankingId"];
        int ruleSetId = (int)ViewState["ruleSetId"];

        DataTable rankedCities = RankingControl.ScoreCities(rankingId, ruleSetId);
        bindToGridview(CityRankingGridView, rankedCities);
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
            // Fetch ranking data.
            Dictionary<RankingControl.RankingKeys, string> rankingInfo = RankingControl.LoadRanking(rankingId);

            ViewState["rankingId"] = rankingId;
            RankingNameLabel.Text = rankingInfo[RankingControl.RankingKeys.Name];

            // Load rule set.
            if (rankingInfo[RankingControl.RankingKeys.RuleSetName] != "") {

                ViewState["ruleSetId"] = Int32.Parse(rankingInfo[RankingControl.RankingKeys.RuleSetId]);

                //RuleSetLabel.Text = rankingInfo[RankingControl.RankingKeys.RuleSetName];
                RuleFormulaLabel.Text = "Formula: " + rankingInfo[RankingControl.RankingKeys.RuleSetFormula];
                
                setRuleSetTextColor(true);
                ScoringRulesList.SelectedIndex = 0;
            }

            // Load ranked cities table data.
            DataTable rankedCities = RankingControl.GetRankingMemberTable(rankingId);
            bindToGridview(CityRankingGridView, rankedCities);

            RankingNameList.SelectedIndex = 0;

            // Ensure visibility is sane.
            postFormulaControlVisability();

        }
        catch (InvalidOperationException)
        {
            LoadStatusLabel.Text = "Ranking not found!";
            return;
        }
    }

    protected void NewRankButton_Click(object sender, EventArgs e)
    {
        ViewState["rankingId"] = null;
        ViewState["ruleSetId"] = null;

        RankingNameLabel.Text = "New Score Group";

        RuleFormulaLabel.Text = "";
        setRuleSetTextColor(false);

        CalcRankingStatusLabel.Text = "";
        
        bindToGridview(CityRankingGridView, new DataTable());

        ScoringRulesList.SelectedIndex = 0;
        RankingNameList.SelectedIndex = 0;

        preFormulaControlVisability();
    }

    protected void DeleteRankButton_Click(object sender, EventArgs e)
    {
        if (ViewState["rankingId"] != null) {

            RankingControl.DeleteRanking((int)ViewState["rankingId"]);
            populateRankingsList();
            NewRankButton_Click(null, null);
        }
    }
    #endregion

    #region rule sets

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

            //RuleSetLabel.Text = ruleSetInfo[RankingControl.RuleSetKeys.Name];
            string rankingName = RankingControl.GetRankingName(ruleSetInfo[RankingControl.RuleSetKeys.Name]);

            RuleFormulaLabel.Text = "Formula : " + ruleSetInfo[RankingControl.RuleSetKeys.Formula];
            RankingNameLabel.Text = rankingName;
            ViewState["rankingId"] = RankingControl.SaveNewRanking(SiteControl.Username, rankingName, ruleSetId);
            ViewState["ruleSetId"] = ruleSetId;

            setRuleSetTextColor(true);
            CalcRankingStatusLabel.Text = "";
            
            // Toggle control visibility.
            postFormulaControlVisability();

            populateRankingsList();
        }
        catch (InvalidOperationException)
        {
            // Formula (Rule set) not found.
            setRuleSetTextColor(false);
            //RuleSetLabel.Text = "Formula not found!";
            //RuleFormulaLabel.Text = "Formula not found!";

            ViewState["ruleSetId"] = null;
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
        bool onlyMe = CitySearchOnlyMeCheckBox.Checked;
        bool allCityNames = CitySearchCityNameCheckBox.Checked;
        string userPattern = CitySearchUserTextBox.Text.Trim();
        string cityNamePattern = CitySeachCityNameTextBox.Text.Trim();

        DataTable cities = RankingControl.GetCitySearchTable(SiteControl.Username, allUsers, onlyMe, userPattern, allCityNames, cityNamePattern);

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
            
            int rankingId = (int)ViewState["rankingId"];
            RankingControl.AddRankedCity(rankingId, cityId);
            bindToGridview(CityRankingGridView, RankingControl.GetRankingMemberTable(rankingId));
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
        bool boxChecked = CitySearchUserCheckBox.Checked;

        CitySearchUserTextBox.Enabled = !boxChecked;
        CitySearchUserTextBox.Visible = !boxChecked;

        CitySearchUserLabel.Visible = !boxChecked;

        if (boxChecked)
        {
            CitySearchOnlyMeCheckBox.Checked = false;
        }
    }

    /// <summary>
    /// Event handler for <c>CitySearchCityNameCheckBox</c> check changed.
    /// Toggles city name textbox useability.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CitySearchCityNameCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        bool boxChecked = CitySearchCityNameCheckBox.Checked;

        CitySeachCityNameTextBox.Enabled = !boxChecked;
        CitySeachCityNameTextBox.Visible = !boxChecked;

        CitySearchCityNameLabel.Visible = !boxChecked;
    }

    /// <summary>
    /// Event hanler for <c>CitySearchOnlyMeCheckBox</c> check changed.
    /// If checked city search will search only current user's city.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CitySearchOnlyMeCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        bool boxChecked = CitySearchOnlyMeCheckBox.Checked;

        CitySearchUserTextBox.Enabled = !boxChecked;
        CitySearchUserTextBox.Visible = !boxChecked;

        CitySearchUserLabel.Visible = !boxChecked;

        if (boxChecked)
        {
            CitySearchUserCheckBox.Checked = false;
        }
    }
    #endregion

    #region private helper functions

    private int getSelectedRuleSetId()
    {
        int ruleSetId;
        return Int32.TryParse(ScoringRulesList.SelectedValue, out ruleSetId) ? ruleSetId : -1;
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

    private void setRuleSetTextColor(bool ruleSetPresent)
    {
        if (ruleSetPresent)
        {
            //RuleSetLabel.ForeColor = ruleSetPresentColor;
            RuleFormulaLabel.ForeColor = ruleSetPresentColor;
        }
        else
        {
            //RuleSetLabel.ForeColor = ruleSetMissingColor;
            RuleFormulaLabel.ForeColor = ruleSetMissingColor;
        }
    }

    private void postFormulaControlVisability()
    {
        LoadRuleSetButton.Visible = false;
        ScoringRulesList.Visible = false;
        FindCities1.Visible = true;
        FindCities2.Visible = true;
        Rule2.Visible = true;
        CalcRankingButton.Visible = true;
    }

    private void preFormulaControlVisability()
    {
        LoadRuleSetButton.Visible = true;
        ScoringRulesList.Visible = true;
        FindCities1.Visible = false;
        FindCities2.Visible = false;
        Rule2.Visible = false;
    }

    #endregion
}