using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, ITokenService tokenService, IMapper mapper)
        {
            _userService = userService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserResponseDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.Email))
            {
                return BadRequest("Email and password are required.");
            }
            var user = await _userService.AuthenticateAsync(userDto.Email);

            if (user == null)
            {
                return Unauthorized();
            }

            var userModel = _mapper.Map<User>(user); 
            var accessToken = _tokenService.GenerateAccessToken(userModel);
            var refreshToken = _tokenService.GenerateRefreshToken();

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new { AccessToken = accessToken });
        }

       
    }
}
