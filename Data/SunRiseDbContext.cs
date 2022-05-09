
using Microsoft.EntityFrameworkCore;
using Model;

namespace Data
{
    public class SunRiseDbContext :DbContext
    {
        public SunRiseDbContext(DbContextOptions<SunRiseDbContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<City> Citys { get; set; }
    }
}
