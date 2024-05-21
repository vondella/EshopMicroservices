
using FluentValidation;

namespace catalogApi.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id,string Name, List<string> Category,
        string Description,string ImageFile,decimal price):ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);
    public class UpdateProductCommandValidator:AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
            RuleFor(command => command.Name).NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 to 150 characters");

            RuleFor(command => command.price).GreaterThan(0).WithMessage("Price must be greater than zero");
        }
    }
    internal class UpdateProductCommandHandler(IDocumentSession session,ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(request.Id,cancellationToken);
            if(product is null)
            {
                throw new ProductNotFoundException(request.Id);
            }
            product.Name = request.Name;
            product.Category = request.Category;
            product.Description = request.Description;
            product.ImageFile = request.ImageFile;
            product.price = request.price;
            session.Update(product);

            await session.SaveChangesAsync();
            return new UpdateProductResult(true);

        }
    }
}
