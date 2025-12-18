using ECommerce.Models;

namespace ECommerce.ViewModels
{
    public class IngredientCheckViewModel
    {
        public string IngredientsText { get; set; } = string.Empty;

        // Parsed list used in the analysis
        public List<string> ParsedIngredients { get; set; } = new();

        // User profile (if logged in)
        public SkinType? UserSkinType { get; set; }
        public SkinConcern? UserConcern { get; set; }

        // Results
        public List<string> HelpfulIngredients { get; set; } = new();
        public List<string> CautionIngredients { get; set; } = new();
        public List<string> AvoidIngredients { get; set; } = new();
        public List<string> ComboWarnings { get; set; } = new();

        public bool HasResult { get; set; }
    }
}