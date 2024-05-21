
namespace ordering.infrastracture.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasConversion(p => p.Value, pId => ProductId.Of(pId));
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();

        }
    }
}
