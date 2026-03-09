using System;

namespace RestaurantManagementService.Controllers
{
    public class RestaurantDto
    {
        public int? RestruntId { get; set; }  // Allow nullable restaurant ID for partial update
        public int? OwnerId { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }  // Nullable Name
        public string Description { get; set; }  // Nullable Description
        public string Address { get; set; }  // Nullable Address
        public string PhoneNumber { get; set; }  // Nullable PhoneNumber
        public string? Email { get; set; }
        public TimeSpan? OpeningTime { get; set; }  // Nullable
        public TimeSpan? ClosingTime { get; set; }  // Nullable
        public bool? IsApproved { get; set; }  // Nullable boolean
        public bool? IsAvailable { get; set; }  // Nullable boolean
        public string ImageUrl { get; set; }
    }

}
