using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Data.Models;
using HotelListing.API.Data.Dto.Country;
using AutoMapper;
using HotelListing.API.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountriesController : ControllerBase
    {
        private readonly ICountriesRepository _repo;
        private readonly IMapper _mapper; 

        public CountriesController(ICountriesRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            var countries = await _repo.GetAllAsync();
            var getCountry = _mapper.Map<List<GetCountryDto>>(countries); 
            return Ok(getCountry);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await _repo.GetDetailsAsync(id);//.GetAsync(id); //.Countries.Include(h => h.Hotels)
                //.FirstOrDefaultAsync(c => c.Id == id);

            if (country == null)
            {
                return NotFound();
            }

            var getCountry = _mapper.Map<CountryDto>(country);

            return Ok(getCountry);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountry)
        {
            if (id != updateCountry.Id)
            {
                return BadRequest();
            }

            //_context.Entry(country).State = EntityState.Modified;
            var getCountry = await _repo.GetAsync(id); //.Countries.FindAsync(id);
            if(getCountry == null)
            {
                return NotFound();
            }
            _mapper.Map(updateCountry, getCountry);

            try
            {
                await _repo.UpdateAsync(getCountry);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CountryDto>> PostCountry(CreateCountryDto createCountry)
        {
            var country = _mapper.Map<Country>(createCountry);
            await _repo.AddAsync(country); //.Countries.Add(country);
            //await _repo.SaveChangesAsync();
            var dto = _mapper.Map<CountryDto>(country);
            return CreatedAtAction("GetCountry", new { id = dto.Id }, dto);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _repo.GetAsync(id); //.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            await _repo.DeleteAsync(id); //.Countries.Remove(country);
            //await _repo.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> CountryExistsAsync(int id)
        {
            return await _repo.Exists(id); //.Any(e => e.Id == id);
        }
    }
}
