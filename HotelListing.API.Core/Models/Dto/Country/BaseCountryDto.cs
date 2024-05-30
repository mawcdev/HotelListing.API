using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Core.Models.Dto.Country
{
    public abstract class BaseCountryDto
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
