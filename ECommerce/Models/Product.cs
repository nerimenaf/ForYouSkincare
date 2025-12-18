using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        public bool IsPopular { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // ---- ForYou skincare-specific fields ----

        [StringLength(100)]
        public string? Brand { get; set; }

        [Display(Name = "Key Ingredients")]
        [StringLength(500)]
        public string? KeyIngredients { get; set; }

        [Display(Name = "Recommended Skin Type")]
        public SkinType? RecommendedSkinType { get; set; }

        [Display(Name = "Target Skin Concern")]
        public SkinConcern? TargetConcern { get; set; }

        // Period After Opening in months (for expiration reminders)
        public int? PaoMonths { get; set; }
    }
}