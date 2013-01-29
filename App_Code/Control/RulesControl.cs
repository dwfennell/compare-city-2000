using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CompareCity.Models;

namespace CompareCity.Control
{
    public class RulesControl
    {
        private static readonly List<string> _cityValueIdentifiers = new List<string> 
        {
            "citysize", 
            "availablefunds", 
            "lifeexpectancy",
            "educationquotent"
        };
        /// <summary>
        /// 
        /// </summary>
        public static List<string> CityValueIdentifiers { get { return _cityValueIdentifiers; } private set { } }

        // TODO: Will having a single static db connection variable slow things to a crawl when there are multiple users? 
        //       Maybe not, but more efficient persistance access also couldn't hurt. 
        private static RuleSetContext db = new RuleSetContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteRuleSet(int id) 
        {
            RuleSet ruleSet = db.RuleSets.First(i => i.RuleSetId == id);
            db.RuleSets.Remove(ruleSet);
            db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleset"></param>
        public static void CreateRuleSet(string name, string formula, string username)
        {
            DateTime now = DateTime.Now;
            bool isValidFormula = ValidateFormula(formula);

            var ruleSet = new RuleSet
            {
                RuleSetName = name,
                Formula = formula,
                User = username,
                Created = now,
                Valid = isValidFormula
            };

            db.RuleSets.Add(ruleSet);
            db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static IQueryable<RuleSet> GetRuleSets(string username)
        {
            IQueryable<RuleSet> query =
                from c in db.RuleSets
                where c.User.Equals(username)
                select c;
            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static bool ValidateFormula(string formula)
        {
            List<string> cityValueIds = FormulaScore.FormulaScore.FetchScoringIDs(formula);
            return validateFormulaIdentifiers(formula, cityValueIds) && validateFormulaArithmetic(formula, cityValueIds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static bool ValidateFormulaArithmetic(string formula)
        {
            List<string> cityValueIds = FormulaScore.FormulaScore.FetchScoringIDs(formula);
            return validateFormulaArithmetic(formula, cityValueIds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static bool ValidateFormulaIdentifiers(string formula)
        {
            List<string> cityValueIds = FormulaScore.FormulaScore.FetchScoringIDs(formula);
            return validateFormulaIdentifiers(formula, cityValueIds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool IsDuplicateName(string name, string username)
        {
            // Check for duplicate formula names.
            IQueryable<RuleSet> query =
                from r in db.RuleSets
                where r.RuleSetName.Equals(name) && r.User.Equals(username)
                select r;
            return query.Count<RuleSet>() > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static string GetBadFormulaIdentifiers(string formula)
        {
            List<string> cityValueIds = FormulaScore.FormulaScore.FetchScoringIDs(formula);
            return getBadFormulaIds(formula, cityValueIds);
        }

        private static string getBadFormulaIds(string formula, List<string> cityIds)
        {
            // Make sure all value identifiers are valid.

            string badIds = "";
            foreach (string id in cityIds)
            {
                if (!RulesControl.CityValueIdentifiers.Contains(id))
                {
                    badIds = badIds + id + " ";
                }
            }

            return badIds;
        }

        private static bool validateFormulaIdentifiers(string formula, List<string> cityIds)
        {
            string badIds = getBadFormulaIds(formula, cityIds);
            return badIds.Equals("");
        }

        private static bool validateFormulaArithmetic(string formula, List<string> cityIds)
        {
            // Add dummy values for scoring ids.

            // TODO: This seems wasteful. Evaluate efficiency and consider alternatives. 
            FormulaScore.FormulaScore scorer = new FormulaScore.FormulaScore();
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
    }
}
