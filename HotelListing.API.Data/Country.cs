using HotelListing.API.Data.Entity;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HotelListing.API.Data
{
    public class Country : AuditedEntity, IEntity
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        [AllowNull]
        public virtual IList<Hotel> Hotels { get; set; }
    }
}