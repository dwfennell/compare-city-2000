using System.Data.Entity;


namespace CompareCity.Models
{
    /// <summary>
    /// Defines a context class for data access for <see cref="City"/>.
    /// </summary>
    public class CityInfoContext : DbContext
    {
        public CityInfoContext() : base("CompareCity")
        {
        }

        public DbSet<CityInfo> Cities { get; set; }
    }
}