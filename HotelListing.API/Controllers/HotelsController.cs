using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Data.Models;
using AutoMapper;
using HotelListing.API.Data.Dto.Hotel;
using HotelListing.API.Contracts;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsRepository _repo;
        private readonly IMapper _mapper;

        public HotelsController(IHotelsRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetHotelDto>>> GetHotels()
        {
            var hotels = await _repo.GetAllAsync(); //.Hotels.ToListAsync();
            var dto = _mapper.Map<List<GetHotelDto>>(hotels);
            return Ok(dto);
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotel = await _repo.GetDetailsAsync(id); //.Hotels.Include(c => c.Country).FirstOrDefaultAsync(h => h.Id == id);

            if (hotel == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<HotelDto>(hotel);

            return Ok(dto);
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, UpdateHotelDto updateHotel)
        {
            if (id != updateHotel.Id)
            {
                return BadRequest();
            }

            //_context.Entry(hotel).State = EntityState.Modified;
            var hotel = await _repo.GetAsync(id); //.Hotels.FindAsync(id);
            if(hotel == null)
            {
                return NotFound();
            }
            _mapper.Map(updateHotel, hotel);

            try
            {
                await _repo.UpdateAsync(hotel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExistsAsync(id))
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

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HotelDto>> PostHotel(CreateHotelDto createHotel)
        {
            var hotel = _mapper.Map<Hotel>(createHotel);
            await _repo.AddAsync(hotel); //.Hotels.Add(hotel);
            //await _repo.SaveChangesAsync();
            var dto = _mapper.Map<GetHotelDto>(hotel);
            return CreatedAtAction("GetHotel", new { id = dto.Id }, dto);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _repo.GetAsync(id); //.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            await _repo.DeleteAsync(id); //.Hotels.Remove(hotel);
            //await _repo.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> HotelExistsAsync(int id)
        {
            return await _repo.Exists(id); //.Hotels.Any(e => e.Id == id);
        }
    }
}
