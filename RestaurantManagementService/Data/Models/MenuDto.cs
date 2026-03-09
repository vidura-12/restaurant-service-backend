namespace RestaurantManagementService.Data.Models
{
    public class MenuDto
    {
        public int? MenuId { get; set; }
        public int? RestaurantId { get; set; }
        public string MenuName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool menuApproved { get; set; }
    }
}
