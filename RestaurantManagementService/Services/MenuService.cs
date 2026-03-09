
using Microsoft.Data.SqlClient;
using System.Data;

namespace RestaurantManagementService.Services
{
    public class MenuService : IMenuService
    {
        private readonly string _connectionString;

        public MenuService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<string> ManageMenuAsync(string action, int? menuId, int restaurantId, string menuName, string description, bool isActive, bool menuApproved)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("ManageMenu", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Action", action ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@MenuId", menuId.HasValue ? (object)menuId.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@RestaurantId", restaurantId);
                    command.Parameters.AddWithValue("@MenuName", string.IsNullOrWhiteSpace(menuName) ? (object)DBNull.Value : menuName.Trim());
                    command.Parameters.AddWithValue("@Description", string.IsNullOrWhiteSpace(description) ? (object)DBNull.Value : description.Trim());
                    command.Parameters.AddWithValue("@IsActive", isActive);
                    command.Parameters.AddWithValue("@menuApproved", menuApproved);
                    try
                    {
                        var result = await command.ExecuteNonQueryAsync();

                        if (result > 0)
                            return "Operation completed successfully.";
                        else
                            return "No rows affected.";
                    }
                    catch (Exception ex)
                    {
                        return $"An error occurred: {ex.Message}";
                    }
                }
            }
        }
    }
}