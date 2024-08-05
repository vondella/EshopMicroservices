
using MassTransit;
using Play.Catalog.Contracts;

namespace Play.Catalog.Client.Catalogs.CreateCatalog
{
    public record CreateItemCatalogCommand(string Name,decimal Price,string Description) : ICommand<CreateItemCataloResponse>;
    public record CreateItemCataloResponse(Guid Id);
    public class CreateItemCatalogCommandValidator : AbstractValidator<CreateItemCatalogCommand>
    {
        public CreateItemCatalogCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("catalog  Name is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Catalog description is required");
            RuleFor(x => x.Price).LessThan(0).WithMessage("negative price is not allowed")
                .NotEmpty().WithMessage("Catalog description is required");
        }
    }
    public class CreateCatalogHandler(IItemRepository<Item> _itemRepository,IPublishEndpoint _publishEndPoint) : ICommandHandler<CreateItemCatalogCommand, CreateItemCataloResponse>
    {
        public async Task<CreateItemCataloResponse> Handle(CreateItemCatalogCommand  command, CancellationToken cancellationToken)
        {
            var item = new Item {  Name=command.Name,Price=command.Price,Description=command.Description,CreatedDate=DateTime.Now};
            await _itemRepository.CreateAsync(item);
            await _publishEndPoint.Publish(new CatalogItemCreated(ItemId: item.Id, Description: item.Description, Price: command.Price, Name: item.Name));
            return new CreateItemCataloResponse(item.Id);
        }
    }
}
