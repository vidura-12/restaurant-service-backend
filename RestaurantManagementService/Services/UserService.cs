using BCrypt.Net;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementService.Data;
using RestaurantManagementService.Data.Models;
using System.Data;


public class UserService
{
    private readonly ApplicationDbContext _context;
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public bool VerifyPassword(string password, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, storedHash);
    }
    private readonly string _connectionString;

    public UserService(string connectionString, ApplicationDbContext context)
    {
        _connectionString = connectionString;
        _context = context;
    }

    public UserService()
    {
    }
    public async Task<LogUser?> GetUserByEmailAsync(string email)
    {
        var query = "SELECT * FROM LogUsers WHERE Email = @Email";

        var user = await _context.LogUsers
            .FromSqlRaw(query, new SqlParameter("@Email", email))
            .FirstOrDefaultAsync();

        return user;
    }

    public async Task<bool> RegisterUserAsync(string firstName, string lastName, string email, string phoneNumber, string passwordHash, int roleId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("RegisterUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Adding parameters
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.Parameters.AddWithValue("@RoleId", roleId);

                try
                {
                    await command.ExecuteNonQueryAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    // Log the error if needed
                    return false;
                }
            }
        }
    }

}
