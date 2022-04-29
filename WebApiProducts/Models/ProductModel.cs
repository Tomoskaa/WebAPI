using System.ComponentModel.DataAnnotations;

namespace WebApiProducts.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public int Hours { get; set; }
        public int Lectures { get; set; }
        public string Author { get; set; }
        public string InStock { get; set; }
    }
}
