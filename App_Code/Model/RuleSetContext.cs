using System.Data.Entity;


namespace CompareCity.Models
{
    /// <summary>
    /// Defines a context class for data access for <see cref="RuleSet"/>.
    /// </summary>
    public class RuleSetContext : DbContext
    {
        public RuleSetContext() : base("CompareCity")
        {
        }

        public DbSet<RuleSet> RuleSets { get; set; }
    }
}