using HotelListing.API.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.API.Data
{
    public class Hotel : AuditedEntity, IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }

        [ForeignKey(nameof(CountryId))]
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
