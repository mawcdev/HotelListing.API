using System.Diagnostics.CodeAnalysis;

namespace HotelListing.API.Data.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        [AllowNull]
        public virtual IList<Hotel> Hotels { get; set; }
    }
}