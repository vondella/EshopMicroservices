using buildingBlock.CQRS;
using Mapster;
using MassTransit;
using System.Security.Claims;
using System.Windows.Input;

namespace Play.Trading.Purchases.CreatePurchase
{
    public record CreatePurchaseCommand(Guid ItemId,int Quantity,Guid IdempotencyId) :ICommand<CreatePurchaseResult>;
    public record CreatePurchaseResult(Guid IdempotencyId);
    public class CreatePurchaseHandler(IPublishEndpoint _endPoint, IHttpContextAccessor _httpContextAccessor) : ICommandHandler<CreatePurchaseCommand, CreatePurchaseResult>
    {
        public async Task<CreatePurchaseResult> Handle(CreatePurchaseCommand command, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("sub");
            var message = new PurchaseRequested(
               Guid.Parse(userId),
               command.ItemId,
               command.Quantity,
               command.IdempotencyId
           );
            await _endPoint.Publish(message);            
            return new CreatePurchaseResult(command.IdempotencyId);
        }
    }
}
