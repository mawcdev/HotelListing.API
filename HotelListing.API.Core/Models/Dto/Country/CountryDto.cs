using HotelListing.API.Core.Models.Dto.Hotel;
using System.Collections.Generic;

namespace HotelListing.API.Core.Models.Dto.Country
{
    public class CountryDto : BaseCountryDto
    {
        public int Id { get; set; }
        public virtual IList<HotelDto> Hotels { get; set; }
    }
}
