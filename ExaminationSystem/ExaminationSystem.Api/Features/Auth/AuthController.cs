using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Api.Features.Auth.ForgotPassword;
using ExaminationSystem.Api.Features.Auth.Login;
using ExaminationSystem.Api.Features.Auth.Login.ViewModels;
using ExaminationSystem.Api.Features.Auth.PasswordReset;
using ExaminationSystem.Api.Features.Auth.Register;
using ExaminationSystem.Api.Features.Auth.ResendOtp;
using ExaminationSystem.Api.Features.Auth.VerifyOtp;
using ExaminationSystem.Api.Features.Auth.VerifyOtp.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel vm)
    {
        var command = new RegisterUserCommand(vm.Email, vm.Password, vm.FullName);
        return (await _mediator.Send(command)).ToActionResult();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel vm)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        var command = new LoginCommand(vm.Email, vm.Password, ipAddress);
        return (await _mediator.Send(command)).ToActionResult();
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpViewModel vm)
    {
        var command = new VerifyOtpCommand(vm.Email, vm.Code);
        return (await _mediator.Send(command)).ToActionResult();
    }

    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp([FromBody] ResendOtpViewModel vm)
    {
        var command = new ResendOtpCommand(vm.Email);
        return (await _mediator.Send(command)).ToActionResult();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel vm)
    {
        var command = new ForgotPasswordCommand(vm.Email);
        return (await _mediator.Send(command)).ToActionResult();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel vm)
    {
        var command = new ResetPasswordCommand(vm.Email, vm.Token, vm.NewPassword);
        return (await _mediator.Send(command)).ToActionResult();
    }
}
