using System;
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

        public async Task<IActionResult> Index(int categoryId)
        {


            var getTrucks = _service.GetTrucksByCategoryId(1);
            var getCategories = _service.GetCategories();
            var numberOfItems = 5;
            await Task.WhenAll(new Task[] { getTrucks, getCategories });

            return View(
                            new TruckListModel
                            {
                                Trucks = getTrucks.Result,
                                Categories = getCategories.Result,
                                NumberOfItems = numberOfItems,
                                SelectedCategory = categoryId
                            }
                        );
        }
    }
}