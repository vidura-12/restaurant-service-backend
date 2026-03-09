namespace RestaurantManagementService.Data.Models
{
    public class RestaurantMenu
    {
        public int RestaurantId { get; set; }
        public int OwnerId { get; set; }
        public int CategoryId { get; set; }
        public int MenuId { get; set; }
        public string CategoryName { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantDescription { get; set; }
        public string Address { get; set; }
        public string MenuName { get; set; }
        public string MenuDescription { get; set; }     
        public bool MenuApproved { get; set; }
        public bool IsActive { get; set; }
        //public bool MenuApproved { get; set; }
    }
}
