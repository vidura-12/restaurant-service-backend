namespace RestaurantManagementService.Services
{
    public interface IMenuService
    {
        Task<string> ManageMenuAsync(string action, int? menuId, int restaurantId, string menuName, string description, bool isActive, bool IsApproved);
    }
}
