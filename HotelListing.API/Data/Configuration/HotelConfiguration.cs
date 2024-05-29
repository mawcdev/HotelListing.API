using HotelListing.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configuration
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "PH Hotel, Resort",
                    Address = "Boracay",
                    CountryId = 1,
                    Rating = 4.9
                },
                new Hotel
                {
                    Id = 2,
                    Name = "MGM",
                    Address = "Las Vegas",
                    CountryId = 2,
                    Rating = 4.9
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Don Quixote",
                    Address = "Akihabara",
                    CountryId = 3,
                    Rating = 4.99

                });
        }
    }
}
