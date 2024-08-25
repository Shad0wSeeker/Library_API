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
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
            {
                return BadRequest("Email and password are required.");
            }
            var user = await _userService.AuthenticateAsync(userDto.Email, userDto.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var userModel = _mapper.Map<User>(user); // Преобразование UserDto в User
            var accessToken = _tokenService.GenerateAccessToken(userModel);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Сохранение refresh токена в куки
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new { AccessToken = accessToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized();
            }

            var principal = _tokenService.GetPrincipalFromExpiredToken(refreshToken);
            var userId = int.Parse(principal.Identity.Name);
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return Unauthorized();
            }

            var userModel = _mapper.Map<User>(user); // Преобразование UserDto в User
            var newAccessToken = _tokenService.GenerateAccessToken(userModel);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            // Обновление refresh токена в куки
            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new { AccessToken = newAccessToken });
        }
    }
}
