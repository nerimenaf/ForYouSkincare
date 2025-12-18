using System.Security.Claims;
using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Services;
using ECommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;

        public CheckoutController(ApplicationDbContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return RedirectToAction("Logout", "Account");

            var vm = new CheckoutViewModel
            {
                FullName = user.FullName,
                Address = user.Address ?? "",
                City = user.City ?? "",
                Country = user.Country ?? "",
                PostalCode = user.PostalCode ?? ""
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cartItems = await _cartService.GetCartItemsAsync();
            if (!cartItems.Any())
            {
                ModelState.AddModelError(string.Empty, "Your cart is empty.");
                return View(model);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var total = cartItems.Sum(i => i.UnitPrice * i.Quantity);

            var order = new Order
            {
                OrderNumber = "ORD-" + DateTime.UtcNow.Ticks,
                UserId = userId,
                FullName = model.FullName,
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                PostalCode = model.PostalCode,
                TotalAmount = total
            };

            foreach (var item in cartItems)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            await _cartService.ClearCartAsync();

            return RedirectToAction("Confirmation", new { id = order.Id });
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }
    }
}