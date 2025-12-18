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
    public class RoutineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoutineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Routine/Index?type=Morning or Night
        [HttpGet]
        public async Task<IActionResult> Index(string? type)
        {
            var routineType = ParseRoutineType(type);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var items = await _context.RoutineItems
                .Where(r => r.UserId == userId && r.RoutineType == routineType)
                .Include(r => r.Product)!.ThenInclude(p => p.Category)
                .OrderBy(r => r.StepOrder)
                .ToListAsync();

            var vm = new RoutinePageViewModel
            {
                SelectedType = routineType,
                Items = items.Select(r => new RoutineItemDisplayViewModel
                {
                    RoutineItemId = r.Id,
                    ProductId = r.ProductId,
                    ProductName = r.Product?.Name ?? "",
                    CategoryName = r.Product?.Category?.Name,
                    ImageUrl = r.Product?.ImageUrl,
                    StepOrder = r.StepOrder
                }).ToList()
            };

            return View(vm);
        }

        // POST: /Routine/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, string routineType)
        {
            var type = ParseRoutineType(routineType);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Avoid duplicates
            bool exists = await _context.RoutineItems.AnyAsync(r =>
                r.UserId == userId &&
                r.ProductId == productId &&
                r.RoutineType == type);

            if (!exists)
            {
                int nextOrder = (await _context.RoutineItems
                    .Where(r => r.UserId == userId && r.RoutineType == type)
                    .MaxAsync(r => (int?)r.StepOrder)) ?? 0;

                var item = new RoutineItem
                {
                    UserId = userId,
                    ProductId = productId,
                    RoutineType = type,
                    StepOrder = nextOrder + 1
                };

                _context.RoutineItems.Add(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { type = type.ToString() });
        }

        // POST: /Routine/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id, string routineType)
        {
            var type = ParseRoutineType(routineType);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var item = await _context.RoutineItems
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (item != null)
            {
                _context.RoutineItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { type = type.ToString() });
        }

        private RoutineType ParseRoutineType(string? type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return RoutineType.Morning;

            return Enum.TryParse<RoutineType>(type, true, out var result)
                ? result
                : RoutineType.Morning;
        }
    }
}