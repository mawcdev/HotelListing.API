using HotelListing.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data
{
    public class HotelListingDbContext : DbContext
    {
        public HotelListingDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id=1,
                    Name="Philippines",
                    ShortName="PH"
                },
                new Country
                {
                    Id=2,
                    Name="United States of America",
                    ShortName="US"
                },
                new Country
                {
                    Id=3,
                    Name="Japan",
                    ShortName="JP"
                });
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id=1,
                    Name="PH Hotel, Resort",
                    Address="Boracay",
                    CountryId=1,
                    Rating=4.9
                },
                new Hotel
                {
                    Id=2,
                    Name="MGM",
                    Address="Las Vegas",
                    CountryId=2,
                    Rating=4.9
                },
                new Hotel
                {
                    Id=3,
                    Name="Don Quixote",
                    Address="Akihabara",
                    CountryId=3,
                    Rating=4.99

                });
        }
    }
}
