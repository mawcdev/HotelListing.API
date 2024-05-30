using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Core.Models.Dto.Country
{
    public class UpdateCountryDto : BaseCountryDto
    {
        [Required]
        public int Id { get; set; }
    }
}
