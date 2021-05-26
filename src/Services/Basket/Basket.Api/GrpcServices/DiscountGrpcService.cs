using Discount.Grpc.Protos;
using System;
using System.Threading.Tasks;
namespace Basket.Api.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService ?? throw new ArgumentNullException(nameof(discountProtoService));
        }

        public async Task<GetDiscountResponse> GetDiscount(string productId)
        {
            var discountRequest = new GetDiscountRequest { ProductId = productId };

            return await _discountProtoService.GetDiscountAsync(discountRequest);
        }

        public async Task<Coupon> CreateDiscount(Coupon coupon)
        {

            return await _discountProtoService.CreateDiscountAsync(coupon);
        }

        public async Task<DeleteDiscountResponse> DeleteDiscount(string productId)
        {
            DeleteDiscountRequest request = new DeleteDiscountRequest { ProductId = productId };
            return await _discountProtoService.DeleteDiscountAsync(request);
        }

        public async Task<UpdateDiscountResponse> UpdateDiscount(Coupon coupon)
        {
            //DeleteDiscountRequest request = new DeleteDiscountRequest { ProductId = productId };
            var response = await _discountProtoService.UpdateDiscountAsync(coupon);
            return response;
        }
    }
}