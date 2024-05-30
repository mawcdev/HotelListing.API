using HotelListing.API.Core.Models.Dto.Country;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.API.Core.Models.Dto.Hotel
{
    public class HotelDto : BaseHotelDto
    {
        public int Id { get; set; }

        public GetCountryDto Country { get; set; }
    }
}
