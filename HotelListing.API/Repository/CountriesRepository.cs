using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Data.Dto.Country;
using HotelListing.API.Data.Models;
using HotelListing.API.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;
        public CountriesRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Country> GetDetailsAsync(int id)
        {
            return await _context.Countries.Include(c => c.Hotels).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CountryDto> GetMappedDetailsAsync(int id)
        {
            var result = await _context.Countries.Include(c => c.Hotels).FirstOrDefaultAsync(c => c.Id == id);
            if(result == null)
            {
                throw new NotFoundException(typeof(Country).Name, id);
            }
            return _mapper.Map<CountryDto>(result);
        }
    }
}
