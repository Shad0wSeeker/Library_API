using Library.Application.DTOs;
using Library.Application.Interfaces;
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
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        //[Authorize(Roles ="Admin")]
        public async Task<ActionResult<AuthorDto>> CreateUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdUser = await _userService.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut]
        //[Authorize]
        public async Task<ActionResult<AuthorDto>> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest("ID mismatched");
            }

            var updatedUser = await _userService.UpdateUserAsync(id, userDto);

            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }

        [HttpDelete]
        //[Authorize(Roles ="Admin")]
        public async Task<ActionResult<UserDto>> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userService.DeleteUserAsync(id);
            return NoContent();

        }
    }
}
