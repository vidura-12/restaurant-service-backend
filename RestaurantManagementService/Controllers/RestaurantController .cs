using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementService.Data;
using RestaurantManagementService.Data.Models;
using RestaurantManagementService.Services;

namespace RestaurantManagementService.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    namespace RestaurantManagementService.Controllers
    {

        [Route("api/[controller]")]
        [ApiController]
        
        public class RestaurantController : ControllerBase
        {
            private readonly ApplicationDbContext _context;
            private readonly RestaurantService _restaurantService;
            public RestaurantController(ApplicationDbContext context, RestaurantService restaurantService)
            {
                _context = context;
                _restaurantService = restaurantService;
            }

            [HttpGet("get-restaurants/categories")]
            public async Task<IActionResult> GetAllRestaurantscategories()
            {
                try
                {
                    var categories = await _restaurantService.GetAllRestaurantCategoriesAsync();
                    return Ok(categories);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }

            [HttpPost("add-restaurant")]
            [Authorize]
            public async Task<IActionResult> AddRestaurant([FromBody] RestaurantDto restaurantDto)
            {
                try
                {
                    // Extract userId and email from the token
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Access NameIdentifier claim
                    var userEmail = User.FindFirstValue(ClaimTypes.Name); // Access Name claim

                    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userEmail))
                    {
                        return Unauthorized("User ID or Email is missing in the token.");
                    }

                    // Log the extracted values for debugging
                    Console.WriteLine($"UserId: {userId}, Email: {userEmail}");

                    // Pass the email from the token to the service method
                    var result = await _restaurantService.ManageRestaurantAsync(
                        "Insert",
                        restaurantDto.RestruntId,
                        int.Parse(userId), // Convert userId to int
                        restaurantDto.CategoryId,
                        restaurantDto.Name,
                        restaurantDto.Description,
                        restaurantDto.Address,
                        restaurantDto.PhoneNumber,
                        userEmail, // Use email from token
                        restaurantDto.OpeningTime,
                        restaurantDto.ClosingTime,
                        restaurantDto.IsApproved,
                        restaurantDto.IsAvailable,
                        restaurantDto.ImageUrl
                    );

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
          

            [HttpPut("update-restaurant/{restaurantId}")]
            [Authorize]
            public async Task<IActionResult> UpdateRestaurant(int restaurantId, [FromBody] RestaurantDto restaurantDto)
            {
                try
                {
                    
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
                    var userEmail = User.FindFirstValue(ClaimTypes.Name); 

                    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userEmail))
                    {
                        return Unauthorized("User ID or Email is missing in the token.");
                    }
                   
                    restaurantDto.RestruntId = restaurantId;
                    restaurantDto.Email = userEmail;
                    int.TryParse(userId.ToString(), out int id);
                    
                    var result = await _restaurantService.ManageRestaurantAsync(
                        "Update",
                        restaurantId,
                        id,
                        null,
                        restaurantDto.Name,
                        restaurantDto.Description,
                        restaurantDto.Address,
                        restaurantDto.PhoneNumber,
                        null, 
                        restaurantDto.OpeningTime,
                        restaurantDto.ClosingTime,
                       restaurantDto.IsApproved,
                        restaurantDto.IsAvailable,
                        restaurantDto.ImageUrl
                    );

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }


            [HttpDelete("delete-restaurant/{restaurantId}")]
            [Authorize]
            public async Task<IActionResult> DeleteRestaurant(int restaurantId)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userEmail = User.FindFirstValue(ClaimTypes.Name);

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User ID or Email is missing in the token.");
                }

                int.TryParse(userId.ToString(), out int id);

                var result = await _restaurantService.ManageRestaurantAsync(
                    "Delete",             
                    restaurantId,          
                    id,                   
                    0,                      
                    null, 
                    null,
                    null,       
                    null, 
                    null,
                    TimeSpan.Zero,      // Set default DateTime.MinValue if necessary
                    TimeSpan.Zero,      // Set default DateTime.MinValue if necessary
                    true,                   // Can be set based on logic or validation
                    true,
                    null// Can be set based on logic or validation
                );

                return Ok(result);
            }
            [HttpGet("get-restaurants")]
            [Authorize]
            public async Task<IActionResult> GetRestaurantsForOwner()
            {
                try
                {
                    // Extract userId and email from the token
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Access NameIdentifier claim
                    var userEmail = User.FindFirstValue(ClaimTypes.Name); // Access Name claim

                    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userEmail))
                    {
                        return Unauthorized("User ID or Email is missing in the token.");
                    }
                    ;
                    int id = int.TryParse(userId.ToString(),out int passid) ? passid : 0 ;
                   
                    var restaurants = await _restaurantService.GetRestaurantsByOwnerEmailAsync(userEmail,id);

                    if (restaurants == null || !restaurants.Any())
                    {
                        return NotFound("No restaurant found for this owner.");
                    }

                    return Ok(restaurants);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }

            [HttpGet("get-restaurant-menu/{restaurantId}")]
            [Authorize]
            public async Task<IActionResult> GetRestaurantMenu(int restaurantId)
            {

                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


                return await _restaurantService.GetRestaurantMenusAsync(restaurantId, userId);
            }
            [HttpGet("get-all-restaurant-menuitems/")]
            [Authorize]
            public async Task<IActionResult> GetRestaurantMenuItems()
            {

                return await _restaurantService.GetRestaurantMenusItemsAsync();
            }

            [HttpGet("get-all-restaurants/")]
           
            public async Task<IActionResult> GetRestaurants()
            {
                var restaurants = await _restaurantService.GetRestaurantsasync();
                return Ok(restaurants); // Wrap the result in an Ok() to return it as IActionResult
            }

        }
    }

}
