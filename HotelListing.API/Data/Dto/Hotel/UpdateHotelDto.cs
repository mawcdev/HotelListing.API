using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Data.Dto.Hotel
{
    public class UpdateHotelDto : BaseHotelDto
    {
        [Required]
        public int Id { get; set; }
    }
}
