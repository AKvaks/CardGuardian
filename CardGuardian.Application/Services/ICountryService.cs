using CardGuardian.Application.Country.Models;

namespace CardGuardian.Application.Services
{
    public interface ICountryService : IGenericService<Domain.Entities.Country, int>
    {
        Task<IEnumerable<CountriesDropDownModel>> GetCountriesDropDownAsync();
    }
}
