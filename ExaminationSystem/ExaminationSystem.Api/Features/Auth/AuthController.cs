using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Api.Features.Auth.Register;
using ExaminationSystem.Api.Features.Auth.VerifyOtp;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using static ExaminationSystem.Api.Features.Auth.Login.Login;
using static ExaminationSystem.Api.Features.Auth.PasswordReset.ForgotPassword;
using static ExaminationSystem.Api.Features.Auth.PasswordReset.ResetPasswordpublic;
using static ExaminationSystem.Api.Features.Auth.Register.RegisterUserCommand;

namespace ExaminationSystem.Api.Features.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpCommand command)
        {
            var result = await _mediator.Send(new VerifyOtpCommand(command.Email, command.Code));
            return result.ToActionResult();
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpCommand command)
        {
            var result = await _mediator.Send(new ResendOtpCommand(command.Email));
            return result.ToActionResult();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            var request = await Request.ReadFromJsonAsync<LoginRequestDto>();
            if (request is null)
                return Result.Failure(Error.Validation("request", "Request body is required.")).ToActionResult();

            var result = await _mediator.Send(new LoginCommand(request.Email, request.Password));

            if (result.IsFailure)
                return result.ToActionResult();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", result.Value!.RefreshToken, cookieOptions);

            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            var result = await _mediator.Send(new ForgotPasswordCommand(command.Email));
            return result.ToActionResult();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var result = await _mediator.Send(new ResetPasswordCommand(command.Email, command.Token, command.NewPassword));
            return result.ToActionResult();
        }
    }
}
