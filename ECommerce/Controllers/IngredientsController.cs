using System.Security.Claims;
using ECommerce.Data;
using ECommerce.Models;
using ECommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Controllers
{
    [AllowAnonymous]
    public class IngredientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IngredientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Ingredients/Checker
        [HttpGet]
        public async Task<IActionResult> Checker()
        {
            var vm = new IngredientCheckViewModel();

            if (User.Identity?.IsAuthenticated ?? false)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var user = await _context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userId);

                vm.UserSkinType = user?.SkinType;
                vm.UserConcern = user?.MainSkinConcern;
            }

            return View(vm);
        }

        // POST: /Ingredients/Checker
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checker(IngredientCheckViewModel model)
        {
            // Reload profile in case of logged-in user
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var user = await _context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userId);

                model.UserSkinType = user?.SkinType;
                model.UserConcern = user?.MainSkinConcern;
            }

            // Parse ingredients from text
            var parsed = ParseIngredients(model.IngredientsText);
            model.ParsedIngredients = parsed;

            AnalyzeIngredients(model);

            model.HasResult = true;
            return View(model);
        }

        // ------------ Helpers ------------

        private List<string> ParseIngredients(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<string>();

            // Split on commas, semicolons, line breaks
            var parts = text
                .Split(new[] { ',', ';', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToList();

            return parts;
        }

        private void AnalyzeIngredients(IngredientCheckViewModel model)
        {
            var lowerList = model.ParsedIngredients
                .Select(i => i.ToLowerInvariant())
                .ToList();

            // Define some reference sets (simplified demo lists)

            var hydrating = new[]
            {
                "hyaluronic acid", "glycerin", "ceramide", "ceramides", "squalane",
                "panthenol", "beta-glucan"
            };

            var acneHelpful = new[]
            {
                "salicylic acid", "bha", "niacinamide", "benzoyl peroxide", "azelaic acid"
            };

            var brightening = new[]
            {
                "vitamin c", "ascorbic acid", "niacinamide", "alpha arbutin", "kojic acid"
            };

            var antiAging = new[]
            {
                "retinol", "retinal", "retinoid", "peptide", "peptides"
            };

            var soothing = new[]
            {
                "centella asiatica", "cica", "aloe", "aloe vera", "allantoin"
            };

            var irritantsForSensitive = new[]
            {
                "fragrance", "parfum", "essential oil", "menthol", "eucalyptus oil", "peppermint oil",
                "alcohol denat", "denatured alcohol"
            };

            // Map ingredients to benefits based on user concern (if any)

            foreach (var ing in lowerList)
            {
                if (ContainsAny(ing, hydrating))
                    model.HelpfulIngredients.Add(GetOriginalName(model.ParsedIngredients, ing) + " (hydrating)");

                if (ContainsAny(ing, soothing))
                    model.HelpfulIngredients.Add(GetOriginalName(model.ParsedIngredients, ing) + " (soothing)");

                if (model.UserConcern == SkinConcern.Acne && ContainsAny(ing, acneHelpful))
                    model.HelpfulIngredients.Add(GetOriginalName(model.ParsedIngredients, ing) + " (acne-friendly)");

                if (model.UserConcern == SkinConcern.DarkSpots && ContainsAny(ing, brightening))
                    model.HelpfulIngredients.Add(GetOriginalName(model.ParsedIngredients, ing) + " (brightening)");

                if (model.UserConcern == SkinConcern.Aging && ContainsAny(ing, antiAging))
                    model.HelpfulIngredients.Add(GetOriginalName(model.ParsedIngredients, ing) + " (anti-aging)");
            }

            // Sensitive skin cautions
            if (model.UserSkinType == SkinType.Sensitive)
            {
                foreach (var ing in lowerList)
                {
                    if (ContainsAny(ing, irritantsForSensitive))
                    {
                        model.AvoidIngredients.Add(GetOriginalName(model.ParsedIngredients, ing)
                            + " (can irritate sensitive skin)");
                    }
                }
            }

            // General caution examples
            foreach (var ing in lowerList)
            {
                if (ing.Contains("aha") || ing.Contains("glycolic acid") || ing.Contains("lactic acid"))
                {
                    model.CautionIngredients.Add(GetOriginalName(model.ParsedIngredients, ing)
                        + " (exfoliating acid - start slowly)");
                }
            }

            // Combo warnings (very simplified)

            bool hasRetinoid = lowerList.Any(i =>
                i.Contains("retinol") || i.Contains("retinal") || i.Contains("retinoid"));

            bool hasStrongVitC = lowerList.Any(i =>
                i.Contains("ascorbic acid") || i.Contains("vitamin c"));

            bool hasStrongAcid = lowerList.Any(i =>
                i.Contains("aha") || i.Contains("glycolic acid") || i.Contains("lactic acid") || i.Contains("salicylic acid"));

            if (hasRetinoid && hasStrongVitC)
            {
                model.ComboWarnings.Add(
                    "Using strong vitamin C and retinoids together can increase irritation. " +
                    "If you are new to actives, use them in separate routines (e.g. vitamin C in the morning, retinoid at night).");
            }

            if (hasRetinoid && hasStrongAcid)
            {
                model.ComboWarnings.Add(
                    "Using strong exfoliating acids and retinoids at the same time can over-exfoliate your skin. " +
                    "Introduce one active at a time or alternate nights.");
            }

            if (!model.HelpfulIngredients.Any() &&
                !model.CautionIngredients.Any() &&
                !model.AvoidIngredients.Any() &&
                !model.ComboWarnings.Any())
            {
                model.CautionIngredients.Add("No specific matches found in our rule set. This does not mean the product is unsafe. " +
                    "Always patch-test new products and consult a dermatologist for medical advice.");
            }
        }

        private bool ContainsAny(string text, IEnumerable<string> patterns)
        {
            foreach (var p in patterns)
            {
                if (text.Contains(p))
                    return true;
            }
            return false;
        }

        private string GetOriginalName(IEnumerable<string> originalList, string lowerValue)
        {
            var match = originalList.FirstOrDefault(o =>
                o.Trim().Equals(lowerValue, StringComparison.OrdinalIgnoreCase) ||
                o.Trim().ToLowerInvariant().Contains(lowerValue));

            return string.IsNullOrWhiteSpace(match) ? lowerValue : match.Trim();
        }
    }
}