using E_Commerce_Clothes.Models;

namespace E_Commerce_Clothes.DTOs
{
    public class ProductsRequestDTO 
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public IFormFile? Image { get; set; }

        public decimal? Price { get; set; }

        public string? Brand { get; set; }

        public decimal? PriceWithDiscount { get; set; }

        public int? CategoryId { get; set; }

    }
}
