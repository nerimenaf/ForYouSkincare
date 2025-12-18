using ECommerce.Models;

namespace ECommerce.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();

        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}