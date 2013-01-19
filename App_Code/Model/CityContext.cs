using System.Data.Entity;


namespace CompareCity.Models
{
    /// <summary>
    /// Defines a context class for data access for <see cref="City"/>.
    /// </summary>
    public class CityContext : DbContext
    {
        public CityContext() : base("CompareCity")
        {
        }

        public DbSet<City> Cities { get; set; }
    }
}