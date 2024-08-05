using System.Runtime.CompilerServices;

namespace Play.Trading
{
  public record PurchaseRequested(Guid UserId,Guid ItemId,int Quantity,Guid CorrelationId);
  public record GetPurchaseState(Guid Id);
  
}
