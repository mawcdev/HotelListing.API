using HotelListing.API.Core.Models.Dto.Hotel;
using HotelListing.API.Data;
using System.Threading.Tasks;

namespace HotelListing.API.Core.Contracts
{
    public interface IHotelsRepository : IGenericRepository<Hotel>
    {
        Task<Hotel> GetDetailsAsync(int id);
        Task<HotelDto> GetMappedDetailsAsync(int id);
    }
}
