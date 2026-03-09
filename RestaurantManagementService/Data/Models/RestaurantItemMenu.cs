using Microsoft.EntityFrameworkCore;

namespace RestaurantManagementService.Data.Models
{
    [Keyless]
    public class RestaurantItemMenu
    {
        public int RestaurantId { get; set; }
        public int OwnerId { get; set; }
        public int CategoryId { get; set; }
        public int MenuId { get; set; }
        public int MenuItemId { get; set; }

        public string CategoryName { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantDescription { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }

        public string MenuName { get; set; }
        public string MenuDescription { get; set; }

        public bool RestarantsIsApproved { get; set; }
        public bool RestarantsIsAvailable { get; set; }
        public bool MenuIsAvailable { get; set; }

        public string MenuItemName { get; set; }
        public string MenuItemDescription { get; set; }
        public decimal MenuItemPrice { get; set; }
        public string MenuItemImage { get; set; }
        public bool MenuItemIsAvailable { get; set; }
    }
}
