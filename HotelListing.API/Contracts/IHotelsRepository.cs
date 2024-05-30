using HotelListing.API.Data.Dto.Hotel;
using HotelListing.API.Data.Models;

namespace HotelListing.API.Contracts
{
    public interface IHotelsRepository : IGenericRepository<Hotel>
    {
        Task<Hotel> GetDetailsAsync(int id);
        Task<HotelDto> GetMappedDetailsAsync(int id);
    }
}
