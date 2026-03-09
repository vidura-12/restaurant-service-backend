using System;
using System.Collections.Generic;

namespace RestaurantManagementService.Data.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
