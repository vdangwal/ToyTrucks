using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Discount.Grpc.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _service;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository service, ILogger<DiscountService> logger, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
            _mapper = mapper;
        }

        public override async Task<GetDiscountResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {


            var coupon = await _service.GetDiscount(request.ProductName);
            GetDiscountResponse response = new GetDiscountResponse();
            if (coupon == null)
                _logger.LogWarning($"Discount with Product name ={request.ProductName} was not found");
            else
                response.Coupon = _mapper.Map<Protos.Coupon>(coupon);

            return response;
        }

        public override async Task<GetDiscountsResponse> GetDiscounts(GetDiscountsRequest request, ServerCallContext context)
        {
            var coupons = await _service.GetDiscounts();
            if (coupons == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discounts not found"));

            var grpcCoupons = _mapper.Map<IEnumerable<Protos.Coupon>>(coupons);

            var response = new GetDiscountsResponse();
            response.Coupons.AddRange(grpcCoupons);

            return response;
        }

        public override async Task<Coupon> CreateDiscount(Coupon request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Dtos.Coupon>(request);
            var createdCoupon = await _service.CreateDiscount(coupon);
            if (createdCoupon == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with Product name ={request.ProductName} was not created"));

            var grpcCoupon = _mapper.Map<Protos.Coupon>(createdCoupon);
            return grpcCoupon;
        }

        public override async Task<UpdateDiscountResponse> UpdateDiscount(Coupon request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Dtos.Coupon>(request);
            var created = await _service.UpdateDiscount(coupon);
            if (created == false)
                _logger.LogWarning($"Coupon with productName= {coupon.ProductName} was not updated");

            var response = new UpdateDiscountResponse
            {
                Created = created
            };

            return response;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {

            var deleted = await _service.DeleteDiscount(request.ProductName);
            if (deleted == false)
                _logger.LogWarning($"Coupon with ProductName= {request.ProductName} was not deleted");

            var response = new DeleteDiscountResponse
            {
                Deleted = deleted
            };

            return response;
        }

    }
}