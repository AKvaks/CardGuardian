using CardGuardian.Application.Country.Models;
using CardGuardian.Application.Services;
using MediatR;

namespace CardGuardian.Application.Country.Queries.GetCountriesDropDown
{
    public record GetCountriesDropDownQuery : IRequest<IEnumerable<CountriesDropDownModel>>;

    public class GetCountriesDropDownQueryHandler : IRequestHandler<GetCountriesDropDownQuery, IEnumerable<CountriesDropDownModel>>
    {
        private readonly ICountryService _countryService;
        public GetCountriesDropDownQueryHandler(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public async Task<IEnumerable<CountriesDropDownModel>> Handle(GetCountriesDropDownQuery request, CancellationToken cancellationToken)
        {
            return await _countryService.GetCountriesDropDownAsync();
        }
    }
}
