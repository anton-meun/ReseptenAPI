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
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [Authorize]
        [HttpPost("Addfavorites")]
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

                await _favoriteService.AddFavoriteAsync(userId, favorite);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("Getfavorites")]
        public async Task<IActionResult> GetFavoritesByUser()
        {
            // get userId from JWT-token
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var favorites = await _favoriteService.GetFavoritesByUserAsync(userId);
            return Ok(favorites);
        }

        [Authorize]
        [HttpDelete("Deletefavorites/{mealId}")]
        public async Task<IActionResult> DeleteFavorite(int mealId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var deleted = await _favoriteService.DeleteFavoriteAsync(userId, mealId);
                if (!deleted) return NotFound("Favorite not found.");

                return Ok("Favorite removed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
