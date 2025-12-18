using System.Security.Claims;
using ECommerce.Data;
using ECommerce.Models;
using ECommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Controllers
{
    [Authorize]
    public class SkinController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SkinController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Skin/Quiz
        [HttpGet]
        public IActionResult Quiz()
        {
            // Simple blank quiz for now; could prefill based on stored profile if desired
            return View(new SkinQuizViewModel());
        }

        // POST: /Skin/Quiz
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Quiz(SkinQuizViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (skinType, concern) = InferSkinProfile(model);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return RedirectToAction("Logout", "Account");

            user.SkinType = skinType;
            user.MainSkinConcern = concern;

            await _context.SaveChangesAsync();

            // After saving profile, redirect to personalized recommendations
            return RedirectToAction(nameof(Recommendations));
        }

        // GET: /Skin/Recommendations
        [HttpGet]
        public async Task<IActionResult> Recommendations()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return RedirectToAction("Logout", "Account");

            var query = _context.Products.Include(p => p.Category).AsQueryable();

            // Filter by skin type
            if (user.SkinType.HasValue)
            {
                var st = user.SkinType.Value;
                query = query.Where(p => !p.RecommendedSkinType.HasValue || p.RecommendedSkinType == st);
            }

            // Filter by main concern
            if (user.MainSkinConcern.HasValue && user.MainSkinConcern.Value != SkinConcern.None)
            {
                var sc = user.MainSkinConcern.Value;
                query = query.Where(p => !p.TargetConcern.HasValue || p.TargetConcern == sc);
            }

            var products = await query
                .OrderByDescending(p => p.IsPopular)
                .ThenBy(p => p.Name)
                .ToListAsync();

            var vm = new SkinRecommendationsViewModel
            {
                UserSkinType = user.SkinType,
                UserConcern = user.MainSkinConcern,
                Products = products
            };

            return View(vm);
        }

        // ----- Simple rule-based "AI" -----
        private (SkinType skinType, SkinConcern concern) InferSkinProfile(SkinQuizViewModel q)
        {
            // SkinType logic
            SkinType skinType;

            if (q.CombinationAreas)
            {
                skinType = SkinType.Combination;
            }
            else if (q.OilyShiny && !q.FeelsDry)
            {
                skinType = SkinType.Oily;
            }
            else if (q.FeelsDry && !q.OilyShiny)
            {
                skinType = SkinType.Dry;
            }
            else if (q.EasilyIrritated)
            {
                skinType = SkinType.Sensitive;
            }
            else
            {
                skinType = SkinType.Normal;
            }

            // Concern logic
            SkinConcern concern = SkinConcern.None;

            if (q.HasAcne)
                concern = SkinConcern.Acne;
            else if (q.HasDarkSpots)
                concern = SkinConcern.DarkSpots;
            else if (q.HasWrinkles)
                concern = SkinConcern.Aging;
            else if (q.FeelsDry)
                concern = SkinConcern.Dryness;
            else if (q.EasilyIrritated)
                concern = SkinConcern.Redness;

            return (skinType, concern);
        }
    }
}