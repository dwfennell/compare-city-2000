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
        public DbSet<ComparisonGroupMember> ComparisionGroups { get; set; }

        public DatabaseContext() : base("CompareCity")   
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define primary keys.
            
            modelBuilder.Entity<CityInfo>().HasKey(ci => ci.CityInfoId);
            modelBuilder.Entity<RuleSet>().HasKey(rs => rs.RuleSetId);
            modelBuilder.Entity<ComparisonGroupMember>().HasKey(cg => cg.ComparisionGroupMemberId);

            // Define required fields.

            modelBuilder.Entity<CityInfo>().HasRequired(ci => ci.MayorName);
            modelBuilder.Entity<CityInfo>().HasRequired(ci => ci.FilePath);
            modelBuilder.Entity<CityInfo>().Property(ci => ci.Uploaded).IsRequired();

            modelBuilder.Entity<RuleSet>().Property(rs => rs.RuleSetName).IsRequired();
            modelBuilder.Entity<RuleSet>().Property(rs => rs.Formula).IsRequired();
            modelBuilder.Entity<RuleSet>().Property(rs => rs.Created).IsRequired();
            modelBuilder.Entity<RuleSet>().Property(rs => rs.Valid).IsRequired();

            modelBuilder.Entity<ComparisonGroupMember>().Property(cg => cg.GroupingIdenifier).IsRequired();
            modelBuilder.Entity<ComparisonGroupMember>().Property(cg => cg.GroupingName).IsRequired();
            modelBuilder.Entity<ComparisonGroupMember>().Property(cg => cg.CityInfoId).IsRequired();
            modelBuilder.Entity<ComparisonGroupMember>().Property(cg => cg.ScoringRulesId).IsRequired();

            // Define ComparisionGroupMember relationships.

            // TODO: Make this an actual one-to-many relationship... if it isn't already.
            modelBuilder.Entity<ComparisonGroupMember>().HasRequired(r => r.ScoringRules)
                .WithMany()
                .HasForeignKey(s => s.ScoringRulesId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ComparisonGroupMember>().HasRequired(c => c.CityInfo)
                .WithMany()
                .HasForeignKey(d => d.CityInfoId)
                .WillCascadeOnDelete(false);

        }
    }
}