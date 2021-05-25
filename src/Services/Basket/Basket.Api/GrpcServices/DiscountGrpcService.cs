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
    }
}