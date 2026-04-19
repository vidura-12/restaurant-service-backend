using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementService.Data;
using RestaurantManagementService.Data.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RestaurantManagementService.Services
{
    public class RestaurantService
    {
        private readonly string _connectionString;
        private readonly ApplicationDbContext _context;
        public RestaurantService(string connectionString, ApplicationDbContext context)
        {
            _context = context;

            _connectionString = connectionString;
        }
        public async Task<IEnumerable<Restaurant>> GetRestaurantsByOwnerEmailAsync(string email, int ownerId)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }

            string sqlQuery = @"
                                SELECT r.*
                                FROM Restaurants r
                                INNER JOIN Users u ON r.OwnerId = u.UserId
                                WHERE u.Email = @Email AND r.OwnerId = @OwnerId";

            var parameters = new[]
            {
        new SqlParameter("@Email", email),
        new SqlParameter("@OwnerId", ownerId)
             };

            var restaurants = await _context.Restaurants
                .FromSqlRaw(sqlQuery, parameters)
                .ToListAsync();

            return restaurants;
        }
        public async Task<IActionResult> GetRestaurantMenusAsync(int restaurantId, int userId)
        {
            // Query the RB_RESTAURANTS_MENUS view
            var menus = await _context.RB_RESTAURANTS_MENUS
                .Where(rm => rm.RestaurantId == restaurantId)
                .ToListAsync();

            if (menus == null || !menus.Any())
            {
                return new NotFoundObjectResult("No menus found for this restaurant.");
            }

            return new OkObjectResult(menus);
        }
        public async Task<IActionResult> GetRestaurantMenusItemsAsync()
        {
            // Query the RB_RESTAURANTS_MENUS view
            var menus = await _context.RB_RESTAURANTS_ITEMSMENUS

                .ToListAsync();

            if (menus == null || !menus.Any())
            {
                return new NotFoundObjectResult("No menus found for this restaurant.");
            }

            return new OkObjectResult(menus);
        }
        public async Task<string> ManageRestaurantAsync(
            string action,
            int? restaurantId,
            int? ownerId,
            int? categoryId,
            string name,
            string description,
            string address,
            string phoneNumber,
            string? email,
            TimeSpan? openingTime,
            TimeSpan? closingTime,
            bool? isApproved,
            bool? isAvailable,
            string ImageUrl)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("ManageRestaurant", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Action
                    command.Parameters.AddWithValue("@Action", action ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@RestaurantId", restaurantId.HasValue ? (object)restaurantId.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@OwnerId", ownerId.HasValue ? (object)ownerId.Value : DBNull.Value); // Check for null
                    command.Parameters.AddWithValue("@CategoryId", categoryId.HasValue ? (object)categoryId.Value : DBNull.Value); // Check for null

                    // Conditional field updates (Only update if provided)
                    command.Parameters.AddWithValue("@Name", string.IsNullOrWhiteSpace(name) ? (object)DBNull.Value : name.Trim());
                    command.Parameters.AddWithValue("@Description", string.IsNullOrWhiteSpace(description) ? (object)DBNull.Value : description.Trim());
                    command.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(address) ? (object)DBNull.Value : address.Trim());
                    command.Parameters.AddWithValue("@PhoneNumber", string.IsNullOrWhiteSpace(phoneNumber) ? (object)DBNull.Value : phoneNumber.Trim());
                    command.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(email) ? (object)DBNull.Value : email.Trim());

                    // Time fields (Only update if provided)
                    command.Parameters.AddWithValue("@OpeningTime", openingTime.HasValue ? (object)openingTime.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@ClosingTime", closingTime.HasValue ? (object)closingTime.Value : DBNull.Value);

                    // Boolean fields (Only update if provided)
                    command.Parameters.AddWithValue("@IsApproved", isApproved.HasValue ? (object)isApproved.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@IsAvailable", isAvailable.HasValue ? (object)isAvailable.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@ImageUrl", string.IsNullOrWhiteSpace(ImageUrl) ? (object)DBNull.Value : ImageUrl.Trim());

                    try
                    {
                        var result = await command.ExecuteNonQueryAsync();

                        if (result > 0)
                        {
                            return "Operation completed successfully.";
                        }
                        else
                        {
                            return "No rows affected.";
                        }
                    }
                    catch (Exception ex)
                    {
                        return $"An error occurred: {ex.Message}";
                    }
                }
            }
        }
        public async Task<IEnumerable<RestaurantCategoryDto>> GetAllRestaurantCategoriesAsync()
        {
            string sqlQuery = @"
                                SELECT 
                                    [CategoryId],
                                    [CategoryName],
                                    [Description]
                                FROM [restaurantDB].[dbo].[RestaurantCategories]";

            var categories = await _context.RestaurantCategories
                .FromSqlRaw(sqlQuery)
                .Select(c => new RestaurantCategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description
                })
                .ToListAsync();

            return categories;
        }


        public async Task<List<Restaurant>> GetRestaurantsasync()
        {
            var restaurants = new List<Restaurant>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                const string query = @"
                SELECT 
                    RestaurantId,
                    OwnerId,
                    CategoryId,
                    Name,
                    Description,
                    Address,
                    ImageUrl,
                    PhoneNumber,
                    Email,
                    OpeningTime,
                    ClosingTime,
                    IsApproved,
                    IsAvailable,
                    CreatedAt,
                    UpdatedAt
                FROM Restaurants";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        restaurants.Add(new Restaurant
                        {
                            RestaurantId = reader.GetInt32(0),
                            OwnerId = reader.GetInt32(1),
                            CategoryId = reader.GetInt32(2),
                            Name = reader.GetString(3),
                            Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Address = reader.IsDBNull(5) ? null : reader.GetString(5),
                            ImageUrl = reader.IsDBNull(6) ? null : reader.GetString(6),
                            PhoneNumber = reader.IsDBNull(7) ? null : reader.GetString(7),
                            Email = reader.IsDBNull(8) ? null : reader.GetString(8),
                            OpeningTime = (TimeOnly)(reader.IsDBNull(9) ? (TimeOnly?)null : TimeOnly.FromTimeSpan(reader.GetTimeSpan(9))),
                            ClosingTime = (TimeOnly)(reader.IsDBNull(10) ? (TimeOnly?)null : TimeOnly.FromTimeSpan(reader.GetTimeSpan(10))),
                            IsApproved = reader.GetBoolean(11),
                            IsAvailable = reader.GetBoolean(12),
                            CreatedAt = reader.GetDateTime(13),
                            UpdatedAt = reader.IsDBNull(14) ? (DateTime?)null : reader.GetDateTime(14)
                        });
                    }
                }
            }

            return restaurants;
        }
    }
}
