namespace ShoppingCart.Models
{
    public class SellerProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }  // Activate or deactivate the seller
    }
}
