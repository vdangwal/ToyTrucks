using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models.View;
using Web.Extensions;
using Web.Models.Api;

namespace Web.Controllers
{
    public class ShoppingBasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly Settings _settings;

        public ShoppingBasketController(IBasketService basketService,
            Settings settings)
        {
            _basketService = basketService;
            _settings = settings;
        }

        public async Task<IActionResult> Index()
        {
            var basketViewModel = await CreateBasketViewModel();

            return View(basketViewModel);
        }

        private async Task<BasketViewModel> CreateBasketViewModel()
        {
            var basketId = Request.Cookies.GetCurrentBasketId(_settings);
            CustomerBasket basket = await _basketService.GetBasket(basketId);

            // var basketLines = await _basketService.GetLinesForBasket(basketId);

            var lineViewModels = basket.Items.Select(bl => new BasketLineViewModel
            {
                LineId = bl.Id,
                TruckId = bl.TruckId,
                TruckName = bl.TruckName,
                Price = bl.Price,
                Year = bl.Year,
                Quantity = bl.Quantity
            });


            var basketViewModel = new BasketViewModel
            {
                BasketLines = lineViewModels.ToList()
            };

            basketViewModel.ShoppingCartTotal = basketViewModel.BasketLines.Sum(bl => bl.Price * bl.Quantity);

            return basketViewModel;
        }

        // [HttpPost]
        // public async Task<IActionResult> AddLine(BasketLineForCreation basketLine)
        // {
        //     var basketId = Request.Cookies.GetCurrentBasketId(_settings);
        //     var newLine = await _basketService.AddToBasket(basketId, basketLine);
        //     Response.Cookies.Append(_settings.BasketIdCookieName, newLine.BasketId.ToString());

        //     return RedirectToAction("Index");
        // }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBasket(CustomerBasket customerBasket)
        {
            //var basketId = Request.Cookies.GetCurrentBasketId(_settings);
            await _basketService.UpdateBasket(customerBasket);
            return RedirectToAction("Index");
        }

        // public async Task<IActionResult> RemoveLine(Guid lineId)
        // {
        //     var basketId = Request.Cookies.GetCurrentBasketId(_settings);
        //     await _basketService.RemoveLine(basketId, lineId);
        //     return RedirectToAction("Index");
        // }

        public IActionResult Checkout()
        {
            return View();
        }
    }
}