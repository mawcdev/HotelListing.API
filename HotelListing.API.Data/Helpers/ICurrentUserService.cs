using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.API.Data.Helpers
{
    public interface ICurrentUserService
    {
        string GetCurrentUserName();
        long GetCurrentUserId();
    }
}
