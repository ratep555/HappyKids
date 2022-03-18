using System.Linq;
using Core.Interfaces;

namespace Infrastructure.Data.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly HappyKidsContext _context;
        public CountryRepository(HappyKidsContext context)
        {
            _context = context;
        }

        public string GetCountryName(int id)
        {
            return _context.Countries.Where(x => x.Id == id).First().Name;
        }

    }
}