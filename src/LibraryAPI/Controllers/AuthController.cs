using AutoMapper;
using Library.Application.Auth.Commands.Login;
using Library.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserResponseDto userDto, CancellationToken cancellationToken)
        {
            var loginCommand = new LoginCommand(userDto);
            var token = await _mediator.Send(loginCommand, cancellationToken);
            return Ok(new { AccessToken = token });
        }
    }
}

