using System;
using System.Collections.Generic;

namespace RestaurantManagementService.Data.Models;

public partial class RestaurantCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
}
