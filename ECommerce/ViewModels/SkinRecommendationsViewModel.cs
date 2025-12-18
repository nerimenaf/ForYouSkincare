using ECommerce.Models;

namespace ECommerce.ViewModels
{
    public class SkinRecommendationsViewModel
    {
        public SkinType? UserSkinType { get; set; }
        public SkinConcern? UserConcern { get; set; }

        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    }
}