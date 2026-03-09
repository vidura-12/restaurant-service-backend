namespace RestaurantManagementService.Data.Models
{
    public class MenuItemDto
    {
        public int MenuItemId { get; set; }   // ✅ This is missing in your case
        public string Name { get; set; }
        public string Description { get; set; }
       // public string IsApproved { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsApproved { get; set; }
        public string ImageUrl { get; set; }
    }

}
