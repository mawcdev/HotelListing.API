using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Data.Dto.Country
{
    public class UpdateCountryDto : BaseCountryDto
    {
        [Required]
        public int Id { get; set;}
    }
}
