using AutoMapper;
using AutoMapper.Internal.Mappers;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models.Dto.User;
using HotelListing.API.Core.Repository;
using HotelListing.API.Data;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.API.Core.Users
{
    public class UsersService : GenericRepository<ApiUser>, IUsersService
    {
        private readonly IGenericRepository<ApiUser> _repository;
        private readonly UserManager<ApiUser> _userManager;
        private readonly RoleManager<ApiRole> _roleManager;
        private readonly IGenericRepository<ApiRole> _roleRepository;
        private readonly IPasswordHasher<ApiUser> _passwordHasher;
        private readonly IAuthManager _logInManager;
        private readonly IMapper _mapper;
        private readonly HotelListingDbContext _context;

        public UsersService(
           IGenericRepository<ApiUser> repository,
           UserManager<ApiUser> userManager,
           RoleManager<ApiRole> roleManager,
           IGenericRepository<ApiRole> roleRepository,
           IPasswordHasher<ApiUser> passwordHasher,
           IAuthManager logInManager,
           IMapper mapper,
           HotelListingDbContext context) : base(context, mapper)
        {
            _repository = repository;
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _logInManager = logInManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ApiUserDto> AddAsync(RegisterUserDto input)
        {
            //CheckCreatePermission();

            var user = _mapper.Map<ApiUser>(input);

            //user.TenantId = AbpSession.TenantId;
            user.EmailConfirmed = true;
            user.UserName = input.Email;

            //await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            await _userManager.CreateAsync(user, input.Password);

            if (input.RoleNames != null)
            {
                await _userManager.AddToRolesAsync(user, input.RoleNames);
            }

            return _mapper.Map<ApiUserDto>(user);
        }

        public async Task<ApiUserDto> UpdateAsync(UpdateUserDto input)
        {
            //CheckUpdatePermission();

            var user = await _repository.GetAsync(input.Id);
            //var user = await _userManager.GetUserAsync(input.Id);

            _mapper.Map(input, user);
            user.UserName = input.Email;

            await _userManager.UpdateAsync(user);


            if (input.RoleNames != null)
            {
                await SetRolesAsync(user, input.RoleNames);
            }

            return _mapper.Map<ApiUserDto>(user);
        }

        public virtual async Task<IdentityResult> SetRolesAsync(ApiUser user, string[] roleNames)
        {

            var userRoles = await _userManager.GetRolesAsync(user);

            //Remove from removed roles
            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null && roleNames.All(roleName => role.Name != roleName))
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    if (!result.Succeeded)
                    {
                        return result;
                    }
                }
            }

            //Add to added roles
            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (roleNames.All(ur => ur != role.Name))
                {
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    if (!result.Succeeded)
                    {
                        return result;
                    }
                }
            }

            return IdentityResult.Success;
        }

        //public async Task DeleteAsync(EntityDto<long> input)
        //{
        //    var user = await _userManager.GetUserByIdAsync(input.Id);
        //    await _userManager.DeleteAsync(user);
        //}

        public async Task Activate(int id)
        {
            var user = await _repository.GetAsync(id);
            user.IsActive = true;
            await UpdateAsync(user);
        }

        public async Task Deactivate(int id)
        {
            var user = await _repository.GetAsync(id);
            user.IsActive = false;
            await UpdateAsync(user);
        }

        //public async Task<ListResultDto<RoleDto>> GetRoles()
        //{
        //    var roles = await _roleRepository.GetAllListAsync();
        //    return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        //}

        //protected IQueryable<ApiUser> CreateFilteredQuery(PagedUserResultRequestDto input)
        //{
        //    return Repository.GetAllIncluding(x => x.Roles)
        //        .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
        //        .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        //}

        //protected override async Task<ApiUser> GetEntityByIdAsync(long id)
        //{
        //    var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

        //    if (user == null)
        //    {
        //        throw new EntityNotFoundException(typeof(User), id);
        //    }

        //    return user;
        //}

        //protected override IQueryable<ApiUser> ApplySorting(IQueryable<ApiUser> query, PagedUserResultRequestDto input)
        //{
        //    return query.OrderBy(r => r.UserName);
        //}

        //public async Task<bool> ChangePassword(ChangePasswordDto input)
        //{
        //    await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

        //    var user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
        //    if (user == null)
        //    {
        //        throw new Exception("There is no current user!");
        //    }

        //    if (await _userManager.CheckPasswordAsync(user, input.CurrentPassword))
        //    {
        //await _userManager.ChangePasswordAsync(user, input.NewPassword;
        //    }
        //    else
        //    {
        //        IdentityResult.Failed(new IdentityError
        //        {
        //            Description = "Incorrect password."
        //        });
        //    }

        //    return true;
        //}

        //public async Task<bool> ResetPassword(ResetPasswordDto input)
        //{
        //    if (_abpSession.UserId == null)
        //    {
        //        throw new UserFriendlyException("Please log in before attempting to reset password.");
        //    }

        //    var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
        //    var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
        //    if (loginAsync.Result != AbpLoginResultType.Success)
        //    {
        //        throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
        //    }

        //    if (currentUser.IsDeleted || !currentUser.IsActive)
        //    {
        //        return false;
        //    }

        //    var roles = await _userManager.GetRolesAsync(currentUser);
        //    if (!roles.Contains(StaticRoleNames.Tenants.Admin))
        //    {
        //        throw new UserFriendlyException("Only administrators may reset passwords.");
        //    }

        //    var user = await _userManager.GetUserByIdAsync(input.UserId);
        //    if (user != null)
        //    {
        //        user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
        //        await CurrentUnitOfWork.SaveChangesAsync();
        //    }

        //    return true;
        //}
    }
}
