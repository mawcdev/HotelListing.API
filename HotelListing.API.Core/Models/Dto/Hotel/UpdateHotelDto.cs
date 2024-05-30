using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Core.Models.Dto.Hotel
{
    public class UpdateHotelDto : BaseHotelDto
    {
        [Required]
        public int Id { get; set; }
    }
}
