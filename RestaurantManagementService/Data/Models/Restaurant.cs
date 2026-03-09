using System;
using System.Collections.Generic;

namespace RestaurantManagementService.Data.Models;

public partial class Restaurant
{
    public int RestaurantId { get; set; }

    public int OwnerId { get; set; }

    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Address { get; set; }
    public string? ImageUrl { get; set; }
    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public TimeOnly OpeningTime { get; set; }

    public TimeOnly ClosingTime { get; set; }

    public bool IsApproved { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual RestaurantCategory Category { get; set; } = null!;

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual User Owner { get; set; } = null!;
}
