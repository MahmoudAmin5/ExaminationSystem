using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Api.Features.Auth.Register;
using ExaminationSystem.Api.Features.Auth.VerifyOtp;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

using ExaminationSystem.Api.Features.Auth.ForgotPassword;
using ExaminationSystem.Api.Features.Auth.PasswordReset;
using ExaminationSystem.Api.Features.Auth.Login;

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
        public async Task<IActionResult> Register([FromBody] RegisterUserOrchestrator command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpOrchestrator command)
        {
            var result = await _mediator.Send(new VerifyOtpOrchestrator(command.Email, command.Code));
            return result.ToActionResult();
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpOrchestrator command)
        {
            var result = await _mediator.Send(new ResendOtpOrchestrator(command.Email));
            return result.ToActionResult();
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login()
        //{
        //    var request = await Request.ReadFromJsonAsync<L>();
        //    if (request is null)
        //        return Result.Failure(Error.Validation("request", "Request body is required.")).ToActionResult();

        //    var result = await _mediator.Send(new LoginOrchestrator(request.Email, request.Password));

        //    if (result.IsFailure)
        //        return result.ToActionResult();

        //    var cookieOptions = new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,
        //        SameSite = SameSiteMode.Strict,
        //        Expires = DateTime.UtcNow.AddDays(7)
        //    };

        //    Response.Cookies.Append("refreshToken", result.Value!.RefreshToken, cookieOptions);

        //    return Ok(result);
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginOrchestrator request) 
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var command = new LoginOrchestrator(request.Email, request.Password, ipAddress);

            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordOrchestrator command)
        {
            var result = await _mediator.Send(new ForgotPasswordOrchestrator(command.Email));
            return result.ToActionResult();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordOrchestrator command)
        {
            var result = await _mediator.Send(new ResetPasswordOrchestrator (command.Email, command.Token, command.NewPassword));
            return result.ToActionResult();
        }
    }
}
