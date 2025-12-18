using ECommerce.Models;

namespace ECommerce.ViewModels
{
    public class DermatologyTipsViewModel
    {
        public SkinType? UserSkinType { get; set; }
        public SkinConcern? UserConcern { get; set; }

        public List<string> GeneralTips { get; set; } = new();
        public List<string> SkinTypeTips { get; set; } = new();
        public List<string> ConcernTips { get; set; } = new();
        public List<string> Warnings { get; set; } = new();

        public IEnumerable<Product> Cleansers { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Product> Moisturizers { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Product> Treatments { get; set; } = Enumerable.Empty<Product>(); // serums
        public IEnumerable<Product> Sunscreens { get; set; } = Enumerable.Empty<Product>();
    }
}