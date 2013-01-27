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
    // TODO: THis can also be centralized in a control class.
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
            FormulaStatus.Text = "Duplicate names not permitted.";
            return;
        }

        storeRuleSet(FormulaNameTextBox.Text.Trim(), FormulaTextBox.Text.Trim(), validateRuleSetFormula());

        FormulaStatus.Text = "Rule set saved!";

        // Refresh cities list. 
        // TODO: There must be a better way to refresh the list..
        Response.Redirect(Request.RawUrl);
    }

    protected void CheckFormulaButton_Click(object sender, EventArgs e)
    {
        string formula = FormulaTextBox.Text;
        List<string> cityValueIds = FormulaScore.FormulaScore.FetchScoringIDs(formula);
        
        // Check for invalid city value identifiers.
        // Valid identifiers specify values like city size, etc. 
        string badIds = getBadFormulaIds(formula, cityValueIds);
        if (!badIds.Equals(""))
        {
            FormulaStatus.Text = "Invalid value identifiers: " + badIds;
            return;
        }

        // Confirm formula is a valid arithmetic expression.
        if (validateFormulaArithmetic(formula, cityValueIds))
        {
            FormulaStatus.Text = "Formula: OKAY!";
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

    private void storeRuleSet(string ruleSetName, string ruleSetFormula, bool isValidFormula)
    {
        string username = getUsername();
        DateTime now = DateTime.Now;
        var ruleSet = new RuleSet
        {
            RuleSetName = ruleSetName,
            Formula = ruleSetFormula,
            User = username,
            Created = now,
            Valid = isValidFormula
        };

        ruleDB.RuleSets.Add(ruleSet);
        ruleDB.SaveChanges();
    }

    private bool validateRuleSetFormula()
    {
        string formula = FormulaTextBox.Text;
        List<string> cityValueIds = FormulaScore.FormulaScore.FetchScoringIDs(formula);
        string badIds = getBadFormulaIds(formula, cityValueIds);

        return badIds.Equals("") && validateFormulaArithmetic(formula, cityValueIds);
    }

    private bool validateFormulaArithmetic(string formula, List<string> cityIds)
    {
        // Add dummy values for scoring ids.
        scorer.ScoringFormula = formula;
        foreach (string id in cityIds)
        {
            try
            {
                scorer.AddScoringValue(id, 1);
            }
            catch (ArgumentException)
            {
                // Identifier already added. Ignore exception.
            }
        }

        return scorer.CheckFormula();
    }

    private string getBadFormulaIds(string formula, List<string> cityIds)
    {
        // Make sure all value identifiers are valid.

        string badIds = "";
        foreach (string id in cityIds)
        {
            if (!validScoringIds.Contains(id))
            {
                badIds = badIds + id + " ";
            }
        }

        return badIds;
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