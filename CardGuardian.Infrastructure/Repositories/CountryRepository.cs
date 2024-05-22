using CardGuardian.Application.Country.Models;
using CardGuardian.Application.Services;
using CardGuardian.Domain.Entities;
using CardGuardian.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CardGuardian.Infrastructure.Repositories
{
    public class CountryRepository : GenericRepository<Country, int>, ICountryService
    {
        public CountryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<CountriesDropDownModel>> GetCountriesDropDownAsync()
        {
            try
            {
                return await _dbContext.Countries.Select(c => new CountriesDropDownModel
                {
                    Id = c.Id,
                    CountryName = c.CountryName
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
