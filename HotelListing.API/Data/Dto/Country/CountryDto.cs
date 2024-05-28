using HotelListing.API.Data.Dto.Hotel;

namespace HotelListing.API.Data.Dto.Country
{
    public class CountryDto : BaseCountryDto
    {
        public int Id { get; set; }
        public virtual IList<HotelDto> Hotels { get; set; }
    }
}
