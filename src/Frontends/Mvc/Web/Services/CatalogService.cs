using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models.Api;
using System.Net.Http;
using Web.Extensions;

namespace Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<Truck>> GetTrucksByCategoryId(int categoryId)
        {
            var response = await _client.GetAsync("api/trucks/?categoryId={categoryid}");
            return await response.ReadContentAs<List<Truck>>();
        }
        public async Task<Truck> GetTruckById(Guid truckId)
        {
            var response = await _client.GetAsync("api/trucks/?truckId={truckId}");
            return await response.ReadContentAs<Truck>();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var response = await _client.GetAsync("/api/categories");
            return await response.ReadContentAs<List<Category>>();
        }
    }
}