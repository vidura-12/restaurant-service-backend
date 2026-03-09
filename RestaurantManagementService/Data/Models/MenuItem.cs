using System;
using System.Collections.Generic;

namespace RestaurantManagementService.Data.Models;

public partial class MenuItem
{
    public int MenuItemId { get; set; }

    public int MenuId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public bool IsAvailable { get; set; }
  //  public bool IsApproved { get; set; }
    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Menu Menu { get; set; } = null!;
}
