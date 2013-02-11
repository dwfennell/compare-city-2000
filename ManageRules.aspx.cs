using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CompareCity.Model;
using CompareCity.Control;

public partial class ManageRules : System.Web.UI.Page
{
    private static readonly int formulaRowIndex = 2;

    public void Page_Load(object sender, EventArgs e)
    {
        // Don't allow users without logins here.
        if (String.IsNullOrWhiteSpace(SiteControl.Username))
        {
            Response.Redirect("~/");
        }
    }

    public void SaveFormulaButton_Click(object sender, EventArgs e)
    {
        string formulaName = FormulaNameTextBox.Text.Trim();
        string formula = FormulaTextBox.Text.Trim();

        if (string.IsNullOrEmpty(formulaName))
        {
            FormulaStatus.Text = "Formula name cannot be blank.";
        }
        else if (string.IsNullOrEmpty(formula))
        {
            FormulaStatus.Text = "Formula cannot be blank.";
        }
        else if (RulesControl.IsDuplicateName(formulaName, SiteControl.Username))
        {
            FormulaStatus.Text = "Duplicate names not permitted.";
        }
        else
        {
            // Save rule set.
            RulesControl.CreateRuleSet(formulaName, formula, SiteControl.Username);
            FormulaStatus.Text = "Rule set saved!";

            // Refresh rule set list.
            Response.Redirect(Request.RawUrl);
        }
    }

    public void CheckFormulaButton_Click(object sender, EventArgs e)
    {
        string formula = FormulaTextBox.Text.Trim();
        
        // Check for invalid city value identifiers.
        // Valid identifiers specify values like city size, etc. 
        if (!RulesControl.ValidateFormulaIdentifiers(formula))
        {
            FormulaStatus.Text = "Invalid value identifiers: " + RulesControl.GetBadFormulaIdentifiers(formula);
            return;
        }

        // Confirm formula is a valid arithmetic expression.
        if (RulesControl.ValidateFormulaArithmetic(formula))
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
        RulesControl.DeleteRuleSet(RuleSetId);
    }

    public void RuleSetsView_RowCommand(Object sender, GridViewCommandEventArgs e)
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

    public IQueryable<RuleSet> GetRules()
    {
        return RulesControl.GetRuleSets(SiteControl.Username);
    }
}