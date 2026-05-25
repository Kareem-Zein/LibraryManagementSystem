namespace LibraryManagementSystem.Application.DTOs.Response.Users
{
    public class LogoutUserResponse
    {
        public DateTime LogoutAtUtc { set; get; } = DateTime.UtcNow;
    }
}
