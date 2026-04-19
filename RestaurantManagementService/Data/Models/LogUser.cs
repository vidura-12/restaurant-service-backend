namespace RestaurantManagementService.Data.Models
{
    public class LogUser
    {
        public int UserId { get; set; } // Assuming 'UserId' is the primary key in your table
        public string Email { get; set; } // 'lg_email' changed to 'Email'
        public string PasswordHash { get; set; } // 'lg_password' changed to 'PasswordHash'
        public int RoleId { get; set; } // 'lg_RoleId' changed to 'RoleId'
        public int? RestaurantId { get; set; } // The '?' allows NULL
        public string? RoleName { get; set; }
    }

}
