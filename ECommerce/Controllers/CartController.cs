using ECommerce.Services;
using ECommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ApplicationDbContext _context;
        public CartController(ICartService cartService, ApplicationDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _cartService.GetCartItemsAsync();
            var vm = new CartViewModel { Items = items };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int quantity = 1, string? returnUrl = null)
        {
            await _cartService.AddToCartAsync(productId, quantity);

            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product != null)
            {
                TempData["CartMessage"] = $"« {product.Name} » a été ajouté à votre panier.";
            }
            else
            {
                TempData["CartMessage"] = "Produit ajouté à votre panier.";
            }

            // If a returnUrl was sent from the form, go back there
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            // Fallback: go to cart
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int productId, int quantity)
        {
            await _cartService.UpdateQuantityAsync(productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            await _cartService.RemoveFromCartAsync(productId);
            return RedirectToAction("Index");
        }
    }
}