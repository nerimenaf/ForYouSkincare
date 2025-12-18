using System.Text.Json;
using ECommerce.Data;
using ECommerce.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Services
{
    public class CartService : ICartService
    {
        private const string CartSessionKey = "Cart";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public CartService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        private ISession Session => _httpContextAccessor.HttpContext!.Session;

        private List<CartItemViewModel> GetCart()
        {
            var data = Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(data))
                return new List<CartItemViewModel>();

            return JsonSerializer.Deserialize<List<CartItemViewModel>>(data) ?? new List<CartItemViewModel>();
        }

        private void SaveCart(List<CartItemViewModel> items)
        {
            var json = JsonSerializer.Serialize(items);
            Session.SetString(CartSessionKey, json);
        }

        public Task<List<CartItemViewModel>> GetCartItemsAsync()
        {
            return Task.FromResult(GetCart());
        }

        public async Task AddToCartAsync(int productId, int quantity)
        {
            var items = GetCart();
            var existing = items.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product == null) return;

                items.Add(new CartItemViewModel
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    UnitPrice = product.Price,
                    Quantity = quantity
                });
            }

            SaveCart(items);
        }

        public Task UpdateQuantityAsync(int productId, int quantity)
        {
            var items = GetCart();
            var item = items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0) items.Remove(item);
                else item.Quantity = quantity;
            }
            SaveCart(items);
            return Task.CompletedTask;
        }

        public Task RemoveFromCartAsync(int productId)
        {
            var items = GetCart();
            items.RemoveAll(i => i.ProductId == productId);
            SaveCart(items);
            return Task.CompletedTask;
        }

        public Task ClearCartAsync()
        {
            SaveCart(new List<CartItemViewModel>());
            return Task.CompletedTask;
        }
    }
}