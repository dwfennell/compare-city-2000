using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using FormulaScore;
using CompareCity.Models;

public partial class ManageRules : System.Web.UI.Page
{
    // TODO: Something strange is going on with namespaces here.
    private FormulaScore.FormulaScore scorer = new FormulaScore.FormulaScore();

    // TODO: A context pool of some kind would be better... in a control class perhaps? 
    private RuleSetContext ruleDB = new RuleSetContext();

    // TODO: Refactor this for sure... this is not the right place for these values.
    private List<string> validScoringIds = new List<string>
    {
        "citysize", 
        "availablefunds", 
        "lifeexpectancy",
        "educationquotent"
    };

    private static readonly int formulaRowIndex = 2;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void SaveFormulaButton_Click(object sender, EventArgs e)
    {
        // TODO: Add a proper validation class?
        // TODO: Also check formula validity before saving.
        if (string.IsNullOrEmpty(FormulaNameTextBox.Text))
        {
            FormulaStatus.Text = "Formula name cannot be blank.";
            return;
        }
        else if (string.IsNullOrEmpty(FormulaTextBox.Text))
        {
            FormulaStatus.Text = "Formula cannot be blank.";
            return;
        }
        else if (isDuplicateName(FormulaNameTextBox.Text))
        {
            // Duplicate name, prompt user for overwrite.
            FormulaStatus.Text = "Duplicate formula names are not permitted.";
        }

        storeRuleSet(FormulaNameTextBox.Text.Trim(), FormulaTextBox.Text.Trim());

        FormulaStatus.Text = "Rule set saved!";

        // Refresh cities list. 
        // TODO: There must be a better way to refresh the list..
        Response.Redirect(Request.RawUrl);

    }

    protected void CheckFormulaButton_Click(object sender, EventArgs e)
    {
        string formula = FormulaTextBox.Text;

        // Make sure all value identifiers are valid.
        List<string> cityValueIds = FormulaScore.FormulaScore.FetchScoringIDs(formula);
        string badIds = "";
        foreach (string id in cityValueIds)
        {
            if (!validScoringIds.Contains(id))
            {
                badIds = badIds + id + " ";
            }
        }
        if (!string.IsNullOrEmpty(badIds))
        {
            FormulaStatus.Text = "Invalid value identifiers: " + badIds;
            return;
        }

        // Add dummy values for scoring ids.
        scorer.ScoringFormula = formula;
        foreach (string id in cityValueIds)
        {
            scorer.AddScoringValue(id, 1);
        }

        // Confirm valid arithmetic expression.
        if (scorer.CheckFormula())
        {
            FormulaStatus.Text = "Formula: OKAY!";
            SaveFormulaButton.Enabled = true;
        }
        else
        {
            FormulaStatus.Text = "Invalid arithmetic expression.";
        }
    }

    public void RuleSetsView_DeleteItem(int RuleSetId)
    {
        RuleSet ruleSet = ruleDB.RuleSets.First(i => i.RuleSetId == RuleSetId);
        ruleDB.RuleSets.Remove(ruleSet);
        ruleDB.SaveChanges();
    }

    protected void FormulaTextBox_TextChanged(object sender, EventArgs e)
    {
        SaveFormulaButton.Enabled = false;
    }

    public IQueryable<RuleSet> GetRules()
    {
        string username = getUsername();

        IQueryable<RuleSet> query =
            from c in ruleDB.RuleSets
            where c.User.Equals(username)
            select c;
        return query;
    }

    protected void RuleSetsView_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "CopyFormula")
        {
            // Fetch row and row data.
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = RuleSetsView.Rows[index];
            string formula = row.Cells[formulaRowIndex].Text;

            // Load formula data into edit area. 
            FormulaTextBox.Text = formula;
        }
    }

    private void storeRuleSet(string ruleSetName, string ruleSetFormula)
    {
        string username = getUsername();
        DateTime now = DateTime.Now;
        var ruleSet = new RuleSet
        {
            RuleSetName = ruleSetName,
            Formula = ruleSetFormula,
            User = username,
            Created = now,
            Updated = now
        };

        ruleDB.RuleSets.Add(ruleSet);
        ruleDB.SaveChanges();
    }

    private bool isDuplicateName(string name)
    {
        // Check for duplicate formula names.
        IQueryable<RuleSet> query =
            from r in ruleDB.RuleSets
            where r.RuleSetName.Equals(name)
            select r;
        return query.Count<RuleSet>() > 0;
    }

    // TODO: This should be centralized to avoid repetition between pages.
    private string getUsername()
    {
        return string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name) ? "" : HttpContext.Current.User.Identity.Name;
    }
}