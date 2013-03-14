using System.Data.Entity.Migrations;

namespace CompareCity.Model
{

    /// <summary>
    /// Summary description for Configuration
    /// </summary>
    public class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}