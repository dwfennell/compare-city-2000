using System.Data.Entity;


namespace CompareCity.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DbSet<RuleSet> RuleSets { get; set; }
        public DbSet<CityInfo> CityInfoes { get; set; }
        public DbSet<ComparisonGroup> ComparisionGroups { get; set; }

        public DatabaseContext() : base("CompareCity")   
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define primary keys.

            modelBuilder.Entity<ComparisonGroup>().HasKey(cg => cg.ComparisionGroupId);

            // Define ComparisionGroup relationships.
            modelBuilder.Entity<ComparisonGroup>().HasRequired(r => r.ScoringRules)
                .WithMany()
                .HasForeignKey(s => s.ScoringRulesId)
                .WillCascadeOnDelete(false);
        }
    }
}