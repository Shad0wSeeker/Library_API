using Library.Application.DTOs;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id, CancellationToken cancellationToken = default)
        {
            var user = await _userService.GetUserByIdAsync(id, cancellationToken);            
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserDto userDto, CancellationToken cancellationToken = default)
        {
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/
            var createdUser = await _userService.CreateUserAsync(userDto, cancellationToken);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UserDto userDto, CancellationToken cancellationToken = default)
        {          
            var updatedUser = await _userService.UpdateUserAsync(id, userDto, cancellationToken);
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<UserDto>> DeleteUser(int id, CancellationToken cancellationToken = default)
        {            
            await _userService.DeleteUserAsync(id, cancellationToken);
            return Ok(new { message = "User deleted successfully" });
        }
    }
}
