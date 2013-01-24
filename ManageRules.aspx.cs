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
        
        storeRuleSet(FormulaNameTextBox.Text.Trim(), FormulaTextBox.Text.Trim());
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
}