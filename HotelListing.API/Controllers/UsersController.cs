using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Core.Users;
using HotelListing.API.Core.Models;
using HotelListing.API.Core.Models.Dto.User;
using HotelListing.API.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<PagedResult<GetUserDto>>> GetUsers([FromQuery] QueryParameters queryParameters)
        {
            var pagedUsersResult = await _usersService.GetAllAsync<GetUserDto>(queryParameters);
            return Ok(pagedUsersResult);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDto>> GetUser(int id)
        {
            var apiUser = await _usersService.GetAsync<GetUserDto>(id);

            if (apiUser == null)
            {
                return NotFound();
            }

            return apiUser;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UpdateUserDto updateUser)
        {
            if (id != updateUser.Id)
            {
                return BadRequest("Invalid record Id");
            }

            try
            {
                await _usersService.UpdateAsync(updateUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExistsAsync(id))
                {
                    throw new NotFoundException(nameof(PutUser), id);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/Users/activate/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("activate/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            try
            {
                await _usersService.Activate(id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExistsAsync(id))
                {
                    throw new NotFoundException(nameof(ActivateUser), id);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/Users/deactivate/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            try
            {
                await _usersService.Deactivate(id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExistsAsync(id))
                {
                    throw new NotFoundException(nameof(DeactivateUser), id);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiUserDto>> PostUser(RegisterUserDto apiUser)
        {
            var user = await _usersService.AddAsync(apiUser);

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _usersService.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> UserExistsAsync(int id)
        {
            return await _usersService.Exists(id);
        }
    }
}
