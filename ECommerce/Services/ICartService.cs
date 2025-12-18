using ECommerce.ViewModels;

namespace ECommerce.Services
{
    public interface ICartService
    {
        Task<List<CartItemViewModel>> GetCartItemsAsync();
        Task AddToCartAsync(int productId, int quantity);
        Task UpdateQuantityAsync(int productId, int quantity);
        Task RemoveFromCartAsync(int productId);
        Task ClearCartAsync();
    }
}