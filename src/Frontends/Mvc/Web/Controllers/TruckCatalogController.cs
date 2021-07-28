using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.Extensions;
using Web.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models.View;

namespace Web.Controllers
{
    public class TruckCatalogController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly Settings _settings;
        public TruckCatalogController(ICatalogService service, IBasketService basketService, Settings settings)
        {
            _catalogService = service;
            _basketService = basketService;
            _settings = settings;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var currentBasketId = Request.Cookies.GetCurrentBasketId(_settings);
            var getTrucks = (categoryId.HasValue) ? _catalogService.GetTrucksByCategoryId(categoryId.Value) : _catalogService.GetTrucks();
            var getBasket = _basketService.GetBasket(currentBasketId);
            var getCategories = _catalogService.GetCategories();

            await Task.WhenAll(new Task[] { getTrucks, getCategories, getBasket });
            var numberOfItems = getBasket.Result?.NumberOfItems ?? 0;
            return View(
                            new TruckListModel
                            {
                                Trucks = getTrucks.Result,
                                Categories = getCategories.Result,
                                NumberOfItems = numberOfItems,
                                SelectedCategory = categoryId.HasValue ? categoryId.Value : null
                            }
                        );
        }

        [HttpPost]
        public IActionResult SelectCategory(int? selectedCategory)
        {
            return RedirectToAction("Index", new { categoryId = selectedCategory });
        }

        public async Task<IActionResult> Detail(Guid truckId)
        {
            var truck = await _catalogService.GetTruckById(truckId);
            return View(truck);
        }
    }
}