using System;
using System.Data.Entity;

using CompareCity.Control;

namespace CompareCity.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DbSet<RuleSet> RuleSets { get; set; }
        public DbSet<CityInfo> CityInfoes { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<RankingMember> RankingMembers { get; set; }
        public DbSet<ScoringIdentifier> ScoringIdentifiers { get; set; }

        public DatabaseContext()
            : base("DefaultConnection")
        {
            //// Make sure MARS is set.
            //var configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            //var connectionString = configuration.ConnectionStrings.ConnectionStrings["DefaultConnection"].ConnectionString;
            //if (!connectionString.Contains("MultipleActiveResultSets=True;"))
            //{
            //    connectionString += ";MultipleActiveResultSets=True;";
            //    configuration.ConnectionStrings.ConnectionStrings["DefaultConnection"].ConnectionString = connectionString;
            //    configuration.Save();
            //}
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Configuration>());

            // Define primary keys.

            modelBuilder.Entity<CityInfo>().HasKey(ci => ci.CityInfoId);
            modelBuilder.Entity<RuleSet>().HasKey(rs => rs.RuleSetId);
            modelBuilder.Entity<Ranking>().HasKey(r => r.RankingId);
            modelBuilder.Entity<RankingMember>().HasKey(m => m.RankingMemberId);
            modelBuilder.Entity<ScoringIdentifier>().HasKey(s => s.Name);

            // Define required fields.

            modelBuilder.Entity<CityInfo>().Property(ci => ci.CityName).IsRequired();
            modelBuilder.Entity<CityInfo>().Property(ci => ci.Mayor).IsRequired();
            modelBuilder.Entity<CityInfo>().Property(ci => ci.FilePath).IsRequired();
            modelBuilder.Entity<CityInfo>().Property(ci => ci.Uploaded).IsRequired();

            modelBuilder.Entity<RuleSet>().Property(rs => rs.RuleSetName).IsRequired();
            modelBuilder.Entity<RuleSet>().Property(rs => rs.Formula).IsRequired();
            modelBuilder.Entity<RuleSet>().Property(rs => rs.Created).IsRequired();
            modelBuilder.Entity<RuleSet>().Property(rs => rs.Valid).IsRequired();

            modelBuilder.Entity<Ranking>().Property(r => r.User).IsRequired();

            modelBuilder.Entity<RankingMember>().Property(m => m.RankingId).IsRequired();
            modelBuilder.Entity<RankingMember>().Property(m => m.CityInfoId).IsRequired();
            modelBuilder.Entity<RankingMember>().Property(m => m.User).IsRequired();

            modelBuilder.Entity<ScoringIdentifier>().Property(s => s.ShortName).IsRequired();
        }
    }
}