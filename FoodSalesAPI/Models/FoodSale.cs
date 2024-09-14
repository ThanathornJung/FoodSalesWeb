namespace FoodSalesAPI.Models
{
    public class FoodSale
    {
        public int Id { get; set; }
        public DateOnly OrderDate { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Category { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
