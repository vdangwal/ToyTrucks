using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Web.Models;
using Web.Models.Api;
using Web.Services;
using Web.Models.View;

namespace Web.Controllers
{

    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly Settings _settings;

        public OrderController(Settings settings, IOrderService orderService)
        {
            _settings = settings;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            // var username = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            // System.Console.WriteLine($"user: {username}");
            // var orders = await _orderService.GetOrdersForUser(
            //     Guid.Parse(username)
            //     );
            var orders = await _orderService.GetOrdersForUser(_settings.UserId);
            var numberOfMessages = (from order in orders
                                    where order.Message != null
                                    select order).Count();

            bool hasMessages = numberOfMessages > 0;

            var vm = new OrderListViewModel
            {
                HasMessage = hasMessages,
                Orders = orders
            };

            return View(vm);
        }

        public async Task<IActionResult> Detail(Guid orderId)
        {
            var ev = await _orderService.GetOrderDetails(orderId);
            return View(ev);
        }

    }
}