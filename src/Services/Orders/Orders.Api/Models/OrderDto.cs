using System.Collections.Generic;

namespace Orders.Api.Models
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        private readonly List<OrderItemDto> _orderItems;
        // BillingAddress
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        // Payment
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }
    }
}