using Microsoft.Data.SqlClient;
using RestaurantManagementService.Data.Models;
using System.Data;

namespace RestaurantManagementService.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public MenuItemService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ Get All Menu Itemss
        public async Task<IEnumerable<MenuItemDto>> GetMenuItemsAsync(int restaurantId, int menuId)
        {
            var menuItems = new List<MenuItemDto>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetMenuItemsByMenuId", conn)) // Assume you have this procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RestaurantId", restaurantId);
                    cmd.Parameters.AddWithValue("@MenuId", menuId);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            menuItems.Add(new MenuItemDto
                            {
                                MenuItemId = reader.GetInt32(reader.GetOrdinal("MenuItemId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                //IsApproved = reader.GetString(reader.GetOrdinal("IsApproved")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                                IsApproved = reader.GetBoolean(reader.GetOrdinal("IsApproved")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl"))
                            });
                        }
                    }
                }
            }

            return menuItems;
        }

        // ✅ Get One Menu Item By Id
        public async Task<MenuItemDto> GetMenuItemByIdAsync(int restaurantId, int menuId, int menuItemId)
        {
            MenuItemDto menuItem = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetMenuItemById", conn)) // Assume you have this procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RestaurantId", restaurantId);
                    cmd.Parameters.AddWithValue("@MenuId", menuId);
                    cmd.Parameters.AddWithValue("@MenuItemId", menuItemId);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            menuItem = new MenuItemDto
                            {
                                MenuItemId = reader.GetInt32(reader.GetOrdinal("MenuItemId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl"))
                            };
                        }
                    }
                }
            }

            return menuItem;
        }

        // ✅ Insert / Update / Delete Menu Item
        public async Task<int> ManageMenuItemAsync(
            string action,
            int? menuItemId,
            int menuId,
            string name,
            
            string description,
            decimal? price,
            bool? isAvailable,
            string imageUrl,
            bool? IsApproved)
        {
            int result;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ManageMenuItem", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Action", action);
                    cmd.Parameters.AddWithValue("@MenuItemId", (object)menuItemId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MenuId", menuId);
                    cmd.Parameters.AddWithValue("@Name", (object)name ?? DBNull.Value);
                  //  cmd.Parameters.AddWithValue("@IsApproved", (object)IsApproved ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Price", (object)price ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsAvailable", (object)isAvailable ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsApproved", (object)IsApproved ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImageUrl", (object)imageUrl ?? DBNull.Value);

                    await conn.OpenAsync();
                    result = await cmd.ExecuteNonQueryAsync(); // You can change to ExecuteScalarAsync if SP returns an ID
                }
            }

            return result; // Can return affected rows or inserted MenuItemId if SP returns it
        }
    }
}
