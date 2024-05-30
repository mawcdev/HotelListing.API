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
using Microsoft.AspNetCore.Authorization;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsRepository _repo;

        public HotelsController(IHotelsRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<PagedResult<GetHotelDto>>> GetHotels([FromQuery] QueryParameters queryParameters)
        {
            var pagedHotelResult = await _repo.GetAllAsync<GetHotelDto>(queryParameters);
            return Ok(pagedHotelResult);
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetHotelDto>> GetHotel(int id)
        {
            var hotel = await _repo.GetAsync<GetHotelDto>(id);
            return hotel;
        }

        // GET: api/Hotels/Details/5
        [HttpGet("Details/{id}")]
        public async Task<ActionResult<HotelDto>> GetHotelDetails(int id)
        {
            var hotel = await _repo.GetMappedDetailsAsync(id);
            return hotel;
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

            try
            {
                await _repo.UpdateAsync<UpdateHotelDto>(id, updateHotel);
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
            var hotel = await _repo.AddAsync<HotelDto, CreateHotelDto>(createHotel);
            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> HotelExistsAsync(int id)
        {
            return await _repo.Exists(id);
        }
    }
}
