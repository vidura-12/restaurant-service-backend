using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementService.Data.Models;
using RestaurantManagementService.Services;

namespace RestaurantManagementService.Controllers
{
    [ApiController]
    [Route("api/restaurant/[controller]")]
    [Authorize]
    public class MenusController : ControllerBase // Use ControllerBase for APIs
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpPost("{restaurantId}/menus")]
        [Authorize]
        public async Task<IActionResult> AddMenu(int restaurantId, [FromBody] MenuDto menuDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _menuService.ManageMenuAsync(
                "Insert",
                null,
                restaurantId,
                menuDto.MenuName,
                menuDto.Description,
                menuDto.IsActive,
                menuDto.menuApproved
            );

            return Ok(result);
        }

        [HttpPut("{restaurantId}/menus/{menuId}")]
        [Authorize]
        public async Task<IActionResult> UpdateMenu(int restaurantId, int menuId, [FromBody] MenuDto menuDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _menuService.ManageMenuAsync(
                "Update",
                menuId,
                restaurantId,
                menuDto.MenuName,
                menuDto.Description,
                menuDto.IsActive,
                menuDto.menuApproved
            );

            return Ok(result);
        }

        [HttpDelete("{restaurantId}/menus/{menuId}")]
        [Authorize]
        public async Task<IActionResult> DeleteMenu(int restaurantId, int menuId)
        {
            var result = await _menuService.ManageMenuAsync(
                "Delete",
                menuId,
                restaurantId,
                null,
                null,
                false,
                false
            );

            return Ok(result);
        }
    }

}
