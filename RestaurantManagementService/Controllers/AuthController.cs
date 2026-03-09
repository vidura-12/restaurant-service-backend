using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
using RestaurantManagementService.Data.Models;
using RestaurantManagementService.Services;
using BCrypt.Net;

using RestaurantManagementService.Data;

namespace RestaurantManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        public AuthController(JwtService jwtService, ApplicationDbContext context, UserService userService, IConfiguration configuration)
        {
            _jwtService = jwtService;
            _context = context;
            _userService = userService;
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            _userService = new UserService(connectionString, _context);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registrationDto)
        {
            try
            {
                // Hash the password
                var hashedPassword = new UserService().HashPassword(registrationDto.Password);

                // Register the user
                var registrationSuccess = await _userService.RegisterUserAsync(
                                   registrationDto.FirstName,
                                   registrationDto.LastName,
                                   registrationDto.Email,
                                   registrationDto.PhoneNumber,
                                   hashedPassword,
                                   registrationDto.id
                               );

                if (!registrationSuccess)
                {
                    return BadRequest(new { error = "User registration failed. Please try again." });
                }


                // Retrieve the newly created user by email (case-insensitive)
                var registeredUser = await _userService.GetUserByEmailAsync(registrationDto.Email.ToLower());

                if (registeredUser == null)
                {
                    return BadRequest(new { error = "User registration failed. User not found after creation." });
                }

                // Generate JWT token
                var token = _jwtService.GenerateToken(
                    registeredUser.RoleName,
                    registeredUser.UserId,
                    registeredUser.Email
                );

                // Return token and role
                return Ok(new
                {
                    Token = token,
                    Role = registeredUser.RoleName
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Error registering user: {ex.Message}" });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // Use UserService to find the user by email using a SQL query
            var logUser = await _userService.GetUserByEmailAsync(loginDto.Email);

            // Check if the user exists and if the password matches
            if (logUser == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, logUser.PasswordHash))
            {
                return Unauthorized("Invalid credentials");
            }

            var token = _jwtService.GenerateToken(logUser.RoleName, logUser.UserId, logUser.Email);

            var role = logUser.RoleName;
            var restaurantId = logUser.RestaurantId;
            // Return the token and role in the response
            return Ok(new { Token = token, Role = role, RestaurantId = restaurantId });
        }



    }
}
