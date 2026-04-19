using BCrypt.Net;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementService.Data;
using RestaurantManagementService.Data.Models;
using System.Data;


public class UserService
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;

    public UserService(IConfiguration configuration, ApplicationDbContext context)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _context = context;
    }

    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    public bool VerifyPassword(string password, string storedHash) => BCrypt.Net.BCrypt.Verify(password, storedHash);
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
                    // Log this to the console so you can see it in Visual Studio's Output window
                    Console.WriteLine("REGISTRATION ERROR: " + ex.Message);

                    // Temporarily throw the error so it shows up in Swagger
                    throw new Exception($"DB Error: {ex.Message}");
                }
                }
        }
    }

}
