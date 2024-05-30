using HotelListing.API.Data.Dto.Country;
using HotelListing.API.Data.Models;

namespace HotelListing.API.Contracts
{
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetailsAsync(int id);

        Task<CountryDto> GetMappedDetailsAsync(int id);
    }
}
