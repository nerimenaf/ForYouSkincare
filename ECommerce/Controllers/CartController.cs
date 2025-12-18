using ECommerce.Services;
using ECommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _cartService.GetCartItemsAsync();
            var vm = new CartViewModel { Items = items };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            await _cartService.AddToCartAsync(productId, quantity);
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