namespace ShoppingCart.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }  // To store the image path
        public int SellerId { get; set; }  // Seller ID
        public SellerProfile Seller { get; set; }  // Navigation property for Seller
    }
}
