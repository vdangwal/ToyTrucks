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
        // private string _accessToken;
        private IConfiguration _config;


        public CatalogService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }

        // private async Task<string> GetToken()
        // {
        //     if (!string.IsNullOrWhiteSpace(_accessToken))
        //     {
        //         System.Console.WriteLine($"catalog token: {_accessToken}");
        //         return _accessToken;
        //     }
        //     var discoveryResponse = await _client.GetDiscoveryDocumentAsync(_config["IdentityUri"]);
        //     if (discoveryResponse.IsError)
        //     {
        //         throw new Exception(discoveryResponse.Error);
        //     }

        //     var tokenResponse = await _client.RequestClientCredentialsTokenAsync(
        //         new ClientCredentialsTokenRequest
        //         {
        //             Address = discoveryResponse.TokenEndpoint,
        //             ClientId = "hesstoytrucks",
        //             ClientSecret = "3322cccf-b6ff-4558-aefb-6c159cd566a0",
        //             Scope = "catalog.read"
        //         });
        //     if (tokenResponse.IsError)
        //     {
        //         throw new Exception(tokenResponse.Error);
        //     }
        //     _accessToken = tokenResponse.AccessToken;
        //     return _accessToken;
        // }

        public async Task<IEnumerable<Truck>> GetTrucksByCategoryId(int categoryId)
        {
            // _client.SetBearerToken(await GetToken());
            // System.Console.WriteLine($"catalog token: {_accessToken}");
            var response = await _client.GetAsync($"api/trucks/{categoryId}");
            var trucks = await response.ReadContentAs<List<Truck>>();
            var orderedTrucks = trucks.OrderBy(t => t.Year);
            return orderedTrucks;
        }
        public async Task<Truck> GetTruckById(Guid truckId)
        {
            //    _client.SetBearerToken(await GetToken());
            var response = await _client.GetAsync($"api/trucks/{truckId}");
            return await response.ReadContentAs<Truck>();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            //   _client.SetBearerToken(await GetToken());
            var response = await _client.GetAsync("/api/categories");
            return await response.ReadContentAs<List<Category>>();
        }

        public async Task<IEnumerable<Truck>> GetTrucks()
        {
            //   _client.SetBearerToken(await GetToken());
            var response = await _client.GetAsync("api/trucks");
            var trucks = await response.ReadContentAs<List<Truck>>();
            var orderedTrucks = trucks.OrderBy(t => t.Year);
            return orderedTrucks;
        }

        public async Task<TruckInventory> GetTruckInventory(Guid truckId)
        {
            //  _client.SetBearerToken(await GetToken());
            var response = await _client.GetAsync($"api/trucks/inventory/{truckId}");
            return await response.ReadContentAs<TruckInventory>();
        }
    }
}