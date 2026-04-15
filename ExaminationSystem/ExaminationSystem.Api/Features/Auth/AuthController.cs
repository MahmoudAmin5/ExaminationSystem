using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Api.Features.Auth.Register;
using ExaminationSystem.Api.Features.Auth.VerifyOtp;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
           
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpCommand command)
        {
            var result = await _mediator.Send(new VerifyOtpCommand(command.Email, command.Code));
            return result.ToActionResult();
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp(ResendOtpCommand command)
        {
            var result = await _mediator.Send(new ResendOtpCommand(command.Email));
            return result.ToActionResult();
        }
    }
}
