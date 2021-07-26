using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models.View;

namespace Web.Controllers
{
    public class TruckCatalogController : Controller
    {
        private readonly ICatalogService _service;

        public TruckCatalogController(ICatalogService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var getTrucks = (categoryId.HasValue) ? _service.GetTrucksByCategoryId(categoryId.Value) : _service.GetTrucks();
            var getCategories = _service.GetCategories();

            await Task.WhenAll(new Task[] { getTrucks, getCategories });

            return View(
                            new TruckListModel
                            {
                                Trucks = getTrucks.Result,
                                Categories = getCategories.Result,
                                NumberOfItems = getTrucks.Result.Count(),
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
            var truck = await _service.GetTruckById(truckId);
            return View(truck);
        }
    }
}