using System.Data.Entity;


namespace CompareCity.Model
{
    /// <summary>
    /// Defines a context class for data access for <see cref="CityInfo"/>.
    /// </summary>
    public class CityInfoContext : DbContext
    {
        public CityInfoContext() : base("CompareCity")
        {
        }

        public DbSet<CityInfo> Cities { get; set; }
    }
}