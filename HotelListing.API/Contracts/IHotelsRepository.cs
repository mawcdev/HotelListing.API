using HotelListing.API.Data.Models;

namespace HotelListing.API.Contracts
{
    public interface IHotelsRepository : IGenericRepository<Hotel>
    {
        Task<Hotel> GetDetailsAsync(int id);
    }
}
