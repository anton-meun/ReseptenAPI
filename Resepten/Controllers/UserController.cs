using _10_Resepten.DTO;
using _20_Business.Services;
using _30_Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _10_Resepten.Controllers
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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto) // use DTO so moddel wont be shown
        {
            try
            {
                var user = new User
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Password = userDto.Password
                };

                var createdUser = await _userService.CreateUserAsync(user);
                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [Authorize]
        [HttpPost("favorites")]
        public async Task<IActionResult> AddFavorite([FromBody] FavoriteDto favoriteDto) // use DTO so moddel wont be shown
        {
            try
            {
                // get userId from JWT-token
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var favorite = new Favorite
                {
                    UserId = userId,
                    MealId = favoriteDto.MealId,
                    Comment = favoriteDto.Comment
                };

                await _userService.AddFavoriteAsync(userId, favorite);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavoritesByUser()
        {
            // get userId from JWT-token
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var favorites = await _userService.GetFavoritesByUserAsync(userId);
            return Ok(favorites);
        }
    }

}
