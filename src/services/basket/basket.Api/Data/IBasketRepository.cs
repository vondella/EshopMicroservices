namespace basketApi.Data
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string username, CancellationToken cancellationToken = default);
        Task<ShoppingCart> StoreBasket(ShoppingCart Cart, CancellationToken cancellationToken = default);
        Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default);

    }
}
