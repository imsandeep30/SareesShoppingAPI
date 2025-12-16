namespace SareesShop.Models
{
    public class Saree
    {
        public int Id { get; set; }
        public string SareeName { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
