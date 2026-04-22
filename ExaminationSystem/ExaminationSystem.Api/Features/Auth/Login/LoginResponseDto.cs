namespace ExaminationSystem.Api.Features.Auth.Login
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
