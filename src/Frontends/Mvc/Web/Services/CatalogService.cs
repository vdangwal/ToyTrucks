using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;
using System.Net.Http;
using Web.Extensions;

namespace NewWeb.Services
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
    }
}