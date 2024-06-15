namespace HotelListing.API.Core.Models.Dto.User
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public long UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
