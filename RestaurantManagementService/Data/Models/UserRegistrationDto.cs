namespace RestaurantManagementService.Data.Models
{
    public class UserRegistrationDto
    {
        public string Email { get; set; }
        public int id { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
