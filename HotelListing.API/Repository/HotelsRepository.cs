using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Data.Dto.Hotel;
using HotelListing.API.Data.Models;
using HotelListing.API.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;
        public HotelsRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Hotel> GetDetailsAsync(int id)
        {
            return await _context.Hotels.Include(h => h.Country).FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<HotelDto> GetMappedDetailsAsync(int id)
        {
            var result = await _context.Hotels.Include(h => h.Country).FirstOrDefaultAsync(h => h.Id == id);
            if(result == null)
            {
                throw new NotFoundException(typeof(Hotel).Name, id);
            }

            return _mapper.Map<HotelDto>(result);
        }
    }
}
