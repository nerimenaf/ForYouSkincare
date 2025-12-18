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
    public class DermatologyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DermatologyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Dermatology/Tips
        [HttpGet]
        public async Task<IActionResult> Tips()
        {
            SkinType? skinType = null;
            SkinConcern? concern = null;

            if (User.Identity?.IsAuthenticated ?? false)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var user = await _context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userId);

                skinType = user?.SkinType;
                concern = user?.MainSkinConcern;
            }

            var generalTips = BuildGeneralTips();
            var skinTypeTips = BuildSkinTypeTips(skinType);
            var concernTips = BuildConcernTips(concern);
            var warnings = BuildWarnings(skinType, concern);

            var productsQuery = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (skinType.HasValue)
            {
                var st = skinType.Value;
                productsQuery = productsQuery.Where(p =>
                    !p.RecommendedSkinType.HasValue || p.RecommendedSkinType == st);
            }

            if (concern.HasValue && concern.Value != SkinConcern.None)
            {
                var sc = concern.Value;
                productsQuery = productsQuery.Where(p =>
                    !p.TargetConcern.HasValue || p.TargetConcern == sc);
            }

            var allProducts = await productsQuery
                .OrderByDescending(p => p.IsPopular)
                .ThenBy(p => p.Name)
                .ToListAsync();

            // Pick recommended products by category
            var cleansers = allProducts
                .Where(p => p.Category != null && p.Category.Name == "Cleansers")
                .Take(3).ToList();

            var moisturizers = allProducts
                .Where(p => p.Category != null && p.Category.Name == "Moisturizers")
                .Take(3).ToList();

            var treatments = allProducts
                .Where(p => p.Category != null && p.Category.Name == "Serums")
                .Take(3).ToList();

            var sunscreens = allProducts
                .Where(p => p.Category != null && p.Category.Name == "Sunscreens")
                .Take(3).ToList();

            var vm = new DermatologyTipsViewModel
            {
                UserSkinType = skinType,
                UserConcern = concern,
                GeneralTips = generalTips,
                SkinTypeTips = skinTypeTips,
                ConcernTips = concernTips,
                Warnings = warnings,
                Cleansers = cleansers,
                Moisturizers = moisturizers,
                Treatments = treatments,
                Sunscreens = sunscreens
            };

            return View(vm);
        }

        // --------- Rules / content ----------

        private List<string> BuildGeneralTips()
        {
            return new List<string>
            {
                "Use a gentle cleanser twice a day; avoid washing your face with very hot water.",
                "Always apply a broad-spectrum sunscreen SPF 30+ every morning, even on cloudy days.",
                "Introduce only one new active ingredient at a time and patch-test on a small area first.",
                "Apply skincare from thinnest to thickest: cleanser → toner → serum → moisturizer → SPF (morning).",
                "Avoid sleeping with makeup on; double cleanse if you wear heavy makeup or sunscreen.",
                "Stay hydrated and get enough sleep; internal health strongly affects your skin."
            };
        }

        private List<string> BuildSkinTypeTips(SkinType? skinType)
        {
            var tips = new List<string>();

            if (!skinType.HasValue)
            {
                tips.Add("Take the Skin Type Quiz so we can personalize tips for your specific skin type.");
                return tips;
            }

            switch (skinType.Value)
            {
                case SkinType.Oily:
                    tips.Add("Use gel or foaming cleansers that mention 'oil control' or 'non-comedogenic'.");
                    tips.Add("Look for ingredients like niacinamide and salicylic acid to help reduce excess oil and breakouts.");
                    tips.Add("Choose lightweight, oil-free moisturizers instead of heavy creams.");
                    break;

                case SkinType.Dry:
                    tips.Add("Use creamy or milky cleansers that do not foam too much, to avoid stripping your skin.");
                    tips.Add("Look for moisturizers with ceramides, glycerin, and hyaluronic acid to repair your barrier.");
                    tips.Add("Avoid over-exfoliating; keep strong acids to 1–2 times per week maximum.");
                    break;

                case SkinType.Combination:
                    tips.Add("Use gentle gel cleansers; you can spot-treat oily areas with targeted treatments.");
                    tips.Add("Use lightweight moisturizers on the T-zone and richer creams on drier areas if needed.");
                    tips.Add("Balance is key; avoid very harsh products that can make dry areas worse.");
                    break;

                case SkinType.Sensitive:
                    tips.Add("Avoid products with strong fragrance, alcohol, and harsh physical scrubs.");
                    tips.Add("Look for soothing ingredients like centella asiatica, panthenol, and aloe vera.");
                    tips.Add("Introduce new actives (like acids or retinoids) slowly and at low concentrations.");
                    break;

                case SkinType.Normal:
                default:
                    tips.Add("Maintain your routine with gentle products and consistent sunscreen use.");
                    tips.Add("You can experiment with mild actives but avoid overloading your routine.");
                    break;
            }

            return tips;
        }

        private List<string> BuildConcernTips(SkinConcern? concern)
        {
            var tips = new List<string>();

            if (!concern.HasValue || concern.Value == SkinConcern.None)
            {
                tips.Add("No main concern selected. You can take the Skin Quiz or update your profile to get targeted advice.");
                return tips;
            }

            switch (concern.Value)
            {
                case SkinConcern.Acne:
                    tips.Add("Look for products with salicylic acid, niacinamide, or benzoyl peroxide for acne-prone skin.");
                    tips.Add("Avoid picking or squeezing pimples to reduce the risk of scars and hyperpigmentation.");
                    tips.Add("Use non-comedogenic moisturizers; skipping moisturizer can make oiliness worse.");
                    break;

                case SkinConcern.Dryness:
                    tips.Add("Use a hydrating serum with hyaluronic acid under a rich moisturizer.");
                    tips.Add("Limit hot showers and aggressive cleansers that strip natural oils.");
                    tips.Add("Consider using a humidifier in dry environments.");
                    break;

                case SkinConcern.Aging:
                    tips.Add("Use sunscreen every day; UV exposure is the main cause of premature aging.");
                    tips.Add("Look for retinol or peptide serums at night (if your skin tolerates them).");
                    tips.Add("Include antioxidants like vitamin C in your morning routine to help with free radicals.");
                    break;

                case SkinConcern.DarkSpots:
                    tips.Add("Use vitamin C serums and niacinamide to help brighten dark spots over time.");
                    tips.Add("Never skip sunscreen; UV exposure can darken existing spots.");
                    tips.Add("Avoid picking at blemishes to reduce post-inflammatory hyperpigmentation.");
                    break;

                case SkinConcern.Redness:
                    tips.Add("Choose fragrance-free, alcohol-free products labeled for sensitive skin.");
                    tips.Add("Look for anti-redness ingredients like centella asiatica and panthenol.");
                    tips.Add("Avoid very hot water, spicy food, and extreme temperatures when possible.");
                    break;

                case SkinConcern.Pores:
                    tips.Add("Use salicylic acid cleansers or toners to gently clean within the pores.");
                    tips.Add("Niacinamide can help with the appearance of enlarged pores and skin texture.");
                    tips.Add("Avoid heavy, comedogenic makeup that can clog pores.");
                    break;
            }

            return tips;
        }

        private List<string> BuildWarnings(SkinType? skinType, SkinConcern? concern)
        {
            var warnings = new List<string>
            {
                "Retinoids (like retinol) are generally not recommended during pregnancy or breastfeeding. Always consult your doctor.",
                "Do not start multiple strong actives (like high-strength acids and retinoids) at the same time.",
                "High-strength vitamin C and retinoids can be irritating together; if you are new to actives, use them in different routines (morning/evening)."
            };

            if (skinType == SkinType.Sensitive)
            {
                warnings.Add("Avoid harsh physical scrubs and strong exfoliating acids; they can damage a sensitive skin barrier.");
            }

            if (concern == SkinConcern.Acne)
            {
                warnings.Add("Overusing drying acne treatments can damage your barrier and worsen breakouts. Start slowly.");
            }

            return warnings;
        }
    }
}