using buildingBlock.CQRS;
using buildingBlock.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Play.Trading.Purchases.GetPurchase
{
    public record GetPurchaseStateCommand(Guid CorrelationId):ICommand<GetPurchaseStateResult>;
    public record GetPurchaseStateResult(PurchaseDto PurchaseDto);
    public class GetPurchaseStateHandler(IRequestClient<GetPurchaseState> purchaseClient) : ICommandHandler<GetPurchaseStateCommand, GetPurchaseStateResult>
    {
        public async Task<GetPurchaseStateResult> Handle(GetPurchaseStateCommand command, CancellationToken cancellationToken)
        {
         
            var response = await purchaseClient.GetResponse<PurchaseState>(new GetPurchaseState(command.CorrelationId));
            if (response.Message.CorrelationId  != command.CorrelationId) 
                throw new NotFoundException($"No data with {command.CorrelationId} was fpund");

            return new GetPurchaseStateResult(new PurchaseDto(response.Message.UserId,
                response.Message.ItemId, response.Message.PurchaseTotal, response.Message.Quantity,
                response.Message.CurrentState, response.Message.ErrorMessage, response.Message.Received, response.Message.LastUpdated));
        }
    }
}
