using RestaurantManagementService.Data.Models;

namespace RestaurantManagementService.Services
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItemDto>> GetMenuItemsAsync(int restaurantId, int menuId);
        Task<MenuItemDto> GetMenuItemByIdAsync(int restaurantId, int menuId, int menuItemId);

        Task<int> ManageMenuItemAsync(
     string action,
     int? menuItemId,
     int menuId,
     string name,
     
     string description,
     decimal? price,
     bool? isAvailable,
     string imageUrl,
     bool? IsApproved); // This argument is required

    }

}
