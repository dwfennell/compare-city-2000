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
        public DbSet<ComparisonGroup> ComparisonGroups { get; set; }
        public DbSet<ComparisonGroupMember> ComparisonGroupMembers { get; set; }

        public DatabaseContext() : base("CompareCity")   
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define primary keys.
            
            modelBuilder.Entity<CityInfo>().HasKey(ci => ci.CityInfoId);
            modelBuilder.Entity<RuleSet>().HasKey(rs => rs.RuleSetId);
            modelBuilder.Entity<ComparisonGroup>().HasKey(cg => cg.ComparisonGroupId);
            modelBuilder.Entity<ComparisonGroupMember>().HasKey(cg => cg.ComparisonGroupMemberId);

            // Define required fields.

            modelBuilder.Entity<CityInfo>().Property(ci => ci.CityName).IsRequired();
            modelBuilder.Entity<CityInfo>().Property(ci => ci.Mayor).IsRequired();
            modelBuilder.Entity<CityInfo>().Property(ci => ci.FilePath).IsRequired();
            modelBuilder.Entity<CityInfo>().Property(ci => ci.Uploaded).IsRequired();

            modelBuilder.Entity<RuleSet>().Property(rs => rs.RuleSetName).IsRequired();
            modelBuilder.Entity<RuleSet>().Property(rs => rs.Formula).IsRequired();
            modelBuilder.Entity<RuleSet>().Property(rs => rs.Created).IsRequired();
            modelBuilder.Entity<RuleSet>().Property(rs => rs.Valid).IsRequired();

            modelBuilder.Entity<ComparisonGroup>().Property(cg => cg.User).IsRequired();

            modelBuilder.Entity<ComparisonGroupMember>().Property(cg => cg.ComparisonGroupId).IsRequired();
            modelBuilder.Entity<ComparisonGroupMember>().Property(cg => cg.CityInfoId).IsRequired();

            // Define ComparisionGroup/Member relationships.

            modelBuilder.Entity<ComparisonGroup>().HasRequired(r => r.RuleSet)
                .WithMany()
                .HasForeignKey(s => s.RuleSetId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ComparisonGroupMember>().HasRequired(cg => cg.ComparisonGroup)
                .WithMany()
                .HasForeignKey(cg => cg.ComparisonGroupId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ComparisonGroupMember>().HasRequired(c => c.CityInfo)
                .WithMany()
                .HasForeignKey(d => d.CityInfoId)
                .WillCascadeOnDelete(false);
        }
    }
}