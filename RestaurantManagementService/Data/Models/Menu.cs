using System;
using System.Collections.Generic;

namespace RestaurantManagementService.Data.Models;

public partial class Menu
{
    public int MenuId { get; set; }

    public int RestaurantId { get; set; }

    public string MenuName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

    public virtual Restaurant Restaurant { get; set; } = null!;
}
