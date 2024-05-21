
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace basketApi.Data
{
    public class CachedBasketRepository(IBasketRepository repository,IDistributedCache cache) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default)
        {
             await repository.DeleteBasket(username, cancellationToken);
            await cache.RemoveAsync(username, cancellationToken);
            return true;
        }

        public async Task<ShoppingCart> GetBasket(string username, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await cache.GetStringAsync(username, cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
            }

            var basket= await repository.GetBasket(username, cancellationToken);
            await cache.SetStringAsync(username, JsonSerializer.Serialize<ShoppingCart>(basket), cancellationToken);
            return basket;

        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart Cart, CancellationToken cancellationToken = default)
        {
             await repository.StoreBasket(Cart, cancellationToken);
            await cache.SetStringAsync(Cart.UserName, JsonSerializer.Serialize<ShoppingCart>(Cart), cancellationToken);
            return  Cart;
        }
    }
}
