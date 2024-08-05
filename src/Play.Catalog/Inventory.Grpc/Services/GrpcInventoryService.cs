using Grpc.Core;

namespace Inventory.Grpc.Services
{
    public class GrpcInventoryService: InventoryItemService.InventoryItemServiceBase
    {
        public override Task<GetInventoryResponse> GetInvetory(GetInventoryRequest request, ServerCallContext context)
        {
            //return base.GetInvetory(request, context);
            return Task.FromResult(new GetInventoryResponse
            {
                
            });
        }
        public override Task<CreateInventoryResponse> CreateInvetory(CreateInventoryRequest request, ServerCallContext context)
        {
            //return base.CreateInvetory(request, context);
            return Task.FromResult(new CreateInventoryResponse
            {
                Id = "success Id"
            });
        }
    }
}
