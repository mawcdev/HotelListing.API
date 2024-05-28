using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        private readonly HotelListingDbContext _context;
        public HotelsRepository(HotelListingDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Hotel> GetDetailsAsync(int id)
        {
            return await _context.Hotels.Include(h => h.Country).FirstOrDefaultAsync(h => h.Id == id);
        }
    }
}
