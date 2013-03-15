using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

using CompareCity.Util;
using CompareCity.Model;

namespace CompareCity.Control
{
    public class RulesControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteRuleSet(int id) 
        {
            using (var db = new DatabaseContext())
            {
                RuleSet ruleSet = db.RuleSets.First(i => i.RuleSetId == id);
                db.RuleSets.Remove(ruleSet);

                // Remove references to this rule set (formula) from any rankings in which it appears.
                var rankings = from r in db.Rankings
                               where r.RuleSetId == ruleSet.RuleSetId
                               select r;
                foreach (Ranking ranking in rankings)
                {
                    ranking.RuleSetId = -1;
                }

                db.SaveChanges();
            }
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

            using (var db = new DatabaseContext())
            {
                db.RuleSets.Add(ruleSet);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static IQueryable<RuleSet> GetRuleSets(string username)
        {
            IQueryable<RuleSet> ruleSets;

            using (var db = new DatabaseContext())
            {
                IQueryable<RuleSet> query =
                    from c in db.RuleSets
                    where c.User.Equals(username)
                    select c;
                ruleSets = query.ToList().AsQueryable();
            }

            return ruleSets;
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
            bool result;
            using (var db = new DatabaseContext())
            {
                result = db.RuleSets.Any(r => r.User.Equals(username) && r.RuleSetName.Equals(name));
            }

            return result;
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

        public static IQueryable<ScoringIdentifier> GetScoringIdentifiers()
        {
            IQueryable<ScoringIdentifier> identifiers;
            var db = new DatabaseContext();

            identifiers = from s in db.ScoringIdentifiers
                          orderby s.DisplayOrder
                          select s;

            return identifiers;
        }

        public static void UpdateScoringIdentifiers(string serverHome)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                // Clear all scoring ids. 
                // ... inefficiently, but this only runs at application start so it will do.
                foreach (var scoringId in db.ScoringIdentifiers)
                {
                    db.ScoringIdentifiers.Remove(scoringId);
                }

                // Add all scoring ids.
                foreach (var scoringId in getScoringIdentifiers(serverHome))
                {
                    db.ScoringIdentifiers.Add(scoringId);
                }

                db.SaveChanges();
            }
        }

        private static List<ScoringIdentifier> getScoringIdentifiers(string serverHome)
        {
            List<ScoringIdentifier> scoringIdentifiers = new List<ScoringIdentifier>();

            string xmlFilepath = String.Format("{0}{1}", serverHome, SiteControl.ScoringIdentifierFilepath);

            // Parse xml file containing our scoring identifiers.
            using (XmlReader reader = XmlReader.Create(xmlFilepath))
            {
                for (int orderNum = 0; reader.Read(); orderNum++)
                {
                    if (reader.IsStartElement() && reader.Name.Equals("Identifier"))
                    {
                        scoringIdentifiers.Add(parseIdentifier(reader, orderNum));
                    }
                }
            }

            return scoringIdentifiers;
        }

        private static ScoringIdentifier parseIdentifier(XmlReader reader, int orderNumber)
        {
            string name = "";
            string shortName = "";
            string description = "";
            string propertyName = "";

            reader.Read();
            while (!"Identifier".Equals(reader.Name))
            {
                switch (reader.Name)
                {
                    case "Name":
                        name = reader.ReadElementContentAsString();
                        break;
                    case "ShortName":
                        shortName = reader.ReadElementContentAsString();
                        break;
                    case "Description":
                        description = reader.ReadElementContentAsString();
                        break;
                    case "PropertyName":
                        propertyName = reader.ReadElementContentAsString();
                        break;
                }

                reader.Read();
            }

            return new ScoringIdentifier
            {
                Name = name,
                ShortName = shortName,
                Descrition = description,
                PropertyName = propertyName,
                DisplayOrder = orderNumber
            };
        }

        private static string getBadFormulaIds(string formula, List<string> cityIds)
        {
            // Make sure all value identifiers are valid.

            string badIds = "";
            using (var db = new DatabaseContext())
            {
                foreach (string id in cityIds)
                {
                    if (!GetCityValue.IsValueIdentifier(id, db))
                    {
                        badIds = badIds + id + " ";
                    }
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
