using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models.Dto.User;
using HotelListing.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.API.Core.Users
{
    public interface IUsersService : IGenericRepository<ApiUser>
    {
        Task<ApiUserDto> AddAsync(RegisterUserDto user);

        Task<ApiUserDto> UpdateAsync(UpdateUserDto user);
        Task Deactivate(int id);
        Task Activate(int id);
        //Task<ListResultDto<RoleDto>> GetRoles();

        //Task<bool> ChangePassword(ChangePasswordDto input);
    }
}
