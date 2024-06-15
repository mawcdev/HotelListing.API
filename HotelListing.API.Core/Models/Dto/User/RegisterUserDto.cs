using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.API.Core.Models.Dto.User
{
    public class RegisterUserDto : LoginUserDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public string[] RoleNames { get; set; }
    }
}
