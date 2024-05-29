namespace HotelListing.API.Data.Dto.User
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string  UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
