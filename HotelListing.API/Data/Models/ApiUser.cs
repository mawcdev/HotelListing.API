using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Data.Models
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
