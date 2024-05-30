using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models.Dto.Country;
using HotelListing.API.Data;
using HotelListing.API.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HotelListing.API.Core.Repository
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
            var result = await _context.Countries.Include(c => c.Hotels)
                .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (result == null)
            {
                throw new NotFoundException(nameof(Country), id);
            }
            return result;
        }
    }
}
