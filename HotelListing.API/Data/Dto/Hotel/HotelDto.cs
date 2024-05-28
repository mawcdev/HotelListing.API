using HotelListing.API.Data.Dto.Country;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.API.Data.Dto.Hotel
{
    public class HotelDto : BaseHotelDto
    {
        public int Id { get; set; }

        public GetCountryDto Country { get; set; }
    }
}
