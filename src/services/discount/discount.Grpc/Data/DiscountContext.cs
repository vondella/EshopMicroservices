using discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace discount.Grpc.Data
{
    public class DiscountContext:DbContext
    {
        public DbSet<Coupon> coupons { get; set; } = default!;
        public DiscountContext(DbContextOptions<DiscountContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(new Coupon { Amount=150, Description="IPhone x Discount",ProductName="IPhone X",Id=1},
                new Coupon { Amount = 150, Description = "Samsung Discount", ProductName = "Samsung", Id = 2 }
                );
            base.OnModelCreating(modelBuilder);
        }

    }
}
