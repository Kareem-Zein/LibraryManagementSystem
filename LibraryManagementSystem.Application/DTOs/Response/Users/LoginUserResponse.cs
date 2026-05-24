namespace LibraryManagementSystem.Application.DTOs.Response.Users
{
    public class LoginUserResponse
    {
        public string Token { set; get; } = null!;
        public string RefreshToken { set; get; } = null!;
    }
}
