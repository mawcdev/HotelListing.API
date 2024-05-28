using HotelListing.API.Data.Models;

namespace HotelListing.API.Contracts
{
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetailsAsync(int id);
    }
}
