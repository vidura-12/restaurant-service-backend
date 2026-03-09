using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementService.Data.Models;
using RestaurantManagementService.Services;

namespace RestaurantManagementService.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    namespace RestaurantManagementService.Controllers
    {
        [ApiController]
        [Route("api/restaurant/menu/[controller]")]
       
        public class MenuItemsController : ControllerBase
        {
            private readonly IMenuItemService _menuItemService;

            public MenuItemsController(IMenuItemService menuItemService)
            {
                _menuItemService = menuItemService;
            }
            
            [HttpGet("/")]
            public async Task<IActionResult> GetAllMenuItems(int restaurantId, int menuId)
            {
                var result = await _menuItemService.GetMenuItemsAsync(restaurantId, menuId);

                if (result == null || !result.Any())
                    return NotFound("No menu items found.");

                return Ok(result);
            }
            
            [Authorize]
            [HttpGet("{restaurantId}/menus/{menuId}/items")]
            public async Task<IActionResult> GetMenuItems(int restaurantId, int menuId)
            {
                var result = await _menuItemService.GetMenuItemsAsync(restaurantId, menuId);

                if (result == null || !result.Any())
                    return NotFound("No menu items found.");

                return Ok(result);
            }

            // ✅ Get a specific menu item by id
            [Authorize]
            [HttpGet("{restaurantId}/menus/{menuId}/items/{menuItemId}")]
            public async Task<IActionResult> GetMenuItem(int restaurantId, int menuId, int menuItemId)
            {
                var result = await _menuItemService.GetMenuItemByIdAsync(restaurantId, menuId, menuItemId);

                if (result == null)
                    return NotFound($"Menu item with ID {menuItemId} not found.");

                return Ok(result);
            }

            // ✅ Add a menu item
            [HttpPost("{restaurantId}/menus/{menuId}/items")]
            [Authorize]
            public async Task<IActionResult> AddMenuItem(int restaurantId, int menuId, [FromBody] MenuItemDto menuItemDto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _menuItemService.ManageMenuItemAsync(
                    "Insert",
                    null,
                    menuId,
                    menuItemDto.Name,
                    menuItemDto.Description,
                    menuItemDto.Price,
                    menuItemDto.IsAvailable,
                    menuItemDto.ImageUrl,
                    menuItemDto.IsApproved

                );

                return Ok(new { message = "Menu item added successfully.", data = result });
            }

            // ✅ Update a menu item
            [HttpPut("{restaurantId}/menus/{menuId}/items/{menuItemId}")]
            [Authorize]
            public async Task<IActionResult> UpdateMenuItem(int restaurantId, int menuId, int menuItemId, [FromBody] MenuItemDto menuItemDto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _menuItemService.ManageMenuItemAsync(
                    "Update",
                    menuItemId,
                    menuId,
                    menuItemDto.Name,
                    menuItemDto.Description,
                    menuItemDto.Price,
                    menuItemDto.IsAvailable,
                    menuItemDto.ImageUrl,
                    menuItemDto.IsApproved
                );

                return Ok(new { message = "Menu item updated successfully.", data = result });
            }

            // ✅ Delete a menu item
            [HttpDelete("{restaurantId}/menus/{menuId}/items/{menuItemId}")]
            [Authorize]
            public async Task<IActionResult> DeleteMenuItem(int restaurantId, int menuId, int menuItemId)
            {
                    var result = await _menuItemService.ManageMenuItemAsync(
                         "Delete",
                         menuItemId,
                         menuId,
                         null,
                         null,
                         0,          // Price
                         null,       // IsAvailable
                         null,
                         null// ✅ Add the imageUrl argument, probably null for delete
 );


                return Ok(new { message = "Menu item deleted successfully.", data = result });
            }
        }
    }
}
