using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Models.Api;

using Web.Extensions;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private string _accessToken;

        public CatalogService(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }
        private async Task<string> GetToken()
        {
            if (!string.IsNullOrWhiteSpace(_accessToken))
            {
                return _accessToken;
            }

            var discoveryDocumentResponse =
                await _client.GetDiscoveryDocumentAsync("https://localhost:5010/");
            if (discoveryDocumentResponse.IsError)
            {
                throw new Exception(discoveryDocumentResponse.Error);
            }

            var tokenResponse =
                await _client.RequestClientCredentialsTokenAsync(
                    new ClientCredentialsTokenRequest
                    {
                        Address = discoveryDocumentResponse.TokenEndpoint,
                        ClientId = "hesstoytrucks",
                        ClientSecret = "3322cccf-b6ff-4558-aefb-6c159cd566a0",
                        Scope = "catalog.read"
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
            var response = await _client.GetAsync("api/categories");
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
            System.Console.WriteLine($"access token: {_accessToken}");
            var response = await _client.GetAsync($"api/trucks/inventory/{truckId}");
            return await response.ReadContentAs<TruckInventory>();
        }
    }
}