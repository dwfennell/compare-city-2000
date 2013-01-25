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
    private FormulaScore.FormulaScore scorer = new FormulaScore.FormulaScore();

    public IQueryable<RuleSet> GetRules()
    {
        // TODO: Remove duplication with ManageCities.aspx.cs?
        string username;
        if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
        {
            username = HttpContext.Current.User.Identity.Name;
        }
        else
        {
            username = "";
        }

        var db = new RuleSetContext();
        //IQueryable<CityInfo> query =
        //    from c in db.RuleSets
        //    where c.User.Equals(username)
        //    select c;
        IQueryable<RuleSet> query =
            from c in db.RuleSets
            select c;

        return query;
    }

    // TODO: Refactor this for sure... this is not the right place for these values.
    private List<string> validScoringIds = new List<string>
    {
        "citysize", 
        "availablefunds", 
        "lifeexpectancy",
        "educationquotent"
    };

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void SaveFormula_Page(object sender, EventArgs e)
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
        else if (!validateFormulaName(FormulaNameTextBox.Text))
        {
            FormulaStatus.Text = "Formula name already exists.";
            return;
        }
        
        storeRuleSet(FormulaNameTextBox.Text.Trim(), FormulaTextBox.Text.Trim());

        FormulaStatus.Text = "Rule set saved!";

        // Refresh cities list. 
        // TODO: There must be a better way to refresh the list..
        Response.Redirect(Request.RawUrl);
    }

    protected void CheckFormula_Page(object sender, EventArgs e)
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

    private void storeRuleSet(string ruleSetName, string ruleSetFormula)
    {
        var ruleSet = new RuleSet
        {
            RuleSetName = ruleSetName,
            Formula = ruleSetFormula
        };

        var context = new RuleSetContext();
        context.RuleSets.Add(ruleSet);
        context.SaveChanges();
    }


    protected void FormulaTextBox_TextChanged(object sender, EventArgs e)
    {
        SaveFormulaButton.Enabled = false;
        
    }

    protected void EditRow()
    {

    }

    // The id parameter name should match the DataKeyNames value set on the control
    public void RuleSetsView_DeleteItem(int id)
    {
        // TODO: Delete record.

    }

    private bool validateFormulaName(string name)
    {
        // TODO: Should be some sort of DB pool, or at least a local variable.
        var context = new RuleSetContext();
        
        IQueryable<RuleSet> query =
            from r in context.RuleSets
            where r.RuleSetName.Equals(name)
            select r;

        if (query.Count<RuleSet>() > 0)
        {
            return false;
        }

        return true;
    }
}