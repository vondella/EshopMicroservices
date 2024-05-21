
namespace basketApi.basket.GetBasket
{
    public record GetBasketQuery(string username) : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCart Cart);
    public class GetBasketHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            var basket = await repository.GetBasket(query.username);
            return new GetBasketResult(basket);

        }
    }
}
