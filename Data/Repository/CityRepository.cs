using Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;

namespace Data.Repository
{
    public class CityRepository: Repository<City> , ICityRepository
    {
        private readonly SunRiseDbContext _db;
        private readonly ILogger<Repository<City>> _logger;
        internal DbSet<City> dbSet;

        public CityRepository(SunRiseDbContext db,ILogger<Repository<City>> logger) : base(db, logger)
        {
            _logger=logger;
            _db = db;
            dbSet = _db.Set<City>();
        }
    }
}
