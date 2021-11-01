using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models.Api;
using System.Net.Http;
using Web.Extensions;

using IdentityModel.Client;
using Microsoft.Extensions.Configuration;

namespace Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;
        private string _accessToken;
        private IConfiguration _config;


        public CatalogService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }

        private async Task<string> GetToken()
        {
            if (!string.IsNullOrWhiteSpace(_accessToken))
            {
                return _accessToken;
            }

            var discoveryResponse = await _client.GetDiscoveryDocumentAsync(_config["IdentityUri"]);
            if (discoveryResponse.IsError)
            {
                throw new Exception(discoveryResponse.Error);
            }

            var tokenResponse = await _client.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryResponse.TokenEndpoint,
                    ClientId = "hesstoytrucksm2m",
                    ClientSecret = "4f3765a1-052b-498a-bcb1-ac3997b37c4c",
                    Scope = "hesstoytrucks.fullaccess"
                });
            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }
            _accessToken = tokenResponse.AccessToken;
            return _accessToken;

        }

        public async Task<IEnumerable<Truck>> GetTrucksByCategoryId(int categoryId)
        {
            _client.SetBearerToken(await GetToken());
            var response = await _client.GetAsync($"api/trucks/{categoryId}");
            var trucks = await response.ReadContentAs<List<Truck>>();
            var orderedTrucks = trucks.OrderBy(t => t.Year);
            return orderedTrucks;
        }
        public async Task<Truck> GetTruckById(Guid truckId)
        {
            _client.SetBearerToken(await GetToken());
            var response = await _client.GetAsync($"api/trucks/{truckId}");
            return await response.ReadContentAs<Truck>();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            _client.SetBearerToken(await GetToken());
            var response = await _client.GetAsync("/api/categories");
            return await response.ReadContentAs<List<Category>>();
        }

        public async Task<IEnumerable<Truck>> GetTrucks()
        {
            _client.SetBearerToken(await GetToken());
            var response = await _client.GetAsync("api/trucks");
            var trucks = await response.ReadContentAs<List<Truck>>();
            var orderedTrucks = trucks.OrderBy(t => t.Year);
            return orderedTrucks;
        }

        public async Task<TruckInventory> GetTruckInventory(Guid truckId)
        {
            _client.SetBearerToken(await GetToken());
            var response = await _client.GetAsync($"api/trucks/inventory/{truckId}");
            return await response.ReadContentAs<TruckInventory>();
        }
    }
}