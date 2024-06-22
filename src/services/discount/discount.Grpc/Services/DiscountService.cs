using discount.Grpc.Data;
using discount.Grpc.Models;
using discount.Grpc.Protos;
using discount.Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger):DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            //var coupon= await dbContext.coupons.FirstOrDefaultAsync(x=>x.ProductName == request.Coupon.ProductName)
            var coupon = request.Adapt<Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument,"Invalid request argument"));

            dbContext.coupons.Add(coupon);
            await dbContext.SaveChangesAsync();

            var couponModel = coupon.Adapt<CouponModel>();
            return  couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with {request.ProductName} is not found"));
            dbContext.coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();
            return new DeleteDiscountResponse { Success = true };
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon == null)
                coupon = new Coupon { ProductName="No Discount", Amount=0, Description="No Discount Desc" };

            var couponModel = coupon.Adapt<CouponModel>();

            return  couponModel;
        }

        public override  async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Adapt<Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request argument"));

            dbContext.coupons.Update(coupon);
            await dbContext.SaveChangesAsync();


            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

    }
}
