using HotelListing.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Audit.Core;

namespace HotelListing.API.Data.Helpers
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException();
        }
        public long GetCurrentUserId()
        {
            if(_httpContextAccessor.HttpContext == null)
            {
                return 0;
            }
            if (_httpContextAccessor.HttpContext.User.Claims.Count() > 0)
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    return long.Parse(_httpContextAccessor.HttpContext.User.FindFirst("uid").Value);
                }
            }
            return 0;
        }

        public string GetCurrentUserName()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return "";
            }
            if (_httpContextAccessor.HttpContext.User.Claims.Count() > 0)
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    return _httpContextAccessor.HttpContext.User.Identity.Name;
                }
            }
            return "";
        }
    }
}
