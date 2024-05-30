using HotelListing.API.Core.Models.Dto.Country;
using HotelListing.API.Data;
using System.Threading.Tasks;

namespace HotelListing.API.Core.Contracts
{
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetailsAsync(int id);

        Task<CountryDto> GetMappedDetailsAsync(int id);
    }
}
