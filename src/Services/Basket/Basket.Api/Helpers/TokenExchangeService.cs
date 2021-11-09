using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
namespace Basket.Api.Helpers
{
    public class TokenExchangeService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;

        public TokenExchangeService(IHttpClientFactory httpClientFactory, IClientAccessTokenCache clientAccessTokenCache)
        {
            _clientAccessTokenCache = clientAccessTokenCache;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> GetToken(string incomingToken, string apiScope)
        {
            var item = await _clientAccessTokenCache.GetAsync($"hesstoytrucks_baskets_to_downstream_tokenexchange_{apiScope}", null);
            if (item != null)
            {
                return item.AccessToken;
            }
            var client = _httpClientFactory.CreateClient();
            var discoveryDocResponse = await client.GetDiscoveryDocumentAsync("https://localhost:3520");
            if (discoveryDocResponse.IsError)
            {
                throw new Exception(discoveryDocResponse.Error);
            }
            var customParams = new Dictionary<string, string>{
                {"subject_token_type","urn:ietf:params:oauth:token-type:access_token"},
                {"subject_token", incomingToken},
                {"scope", $"openid profile {apiScope}"}
            };

            var tokenResponse = await client.RequestTokenAsync(new TokenRequest()
            {
                Address = discoveryDocResponse.TokenEndpoint,

                GrantType = "urn:ietf:params:oauth:grant-type:token-exchange",
                Parameters = Parameters.FromObject(customParams),
                ClientId = "hesstoytrucks_baskets_to_downstream_tokenexchange",
                ClientSecret = "b438b4c0-9963-444d-882f-74a754e667d1"
            });
            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }
            await _clientAccessTokenCache.SetAsync($"hesstoytrucks_baskets_to_downstream_tokenexchange_{apiScope}",
                tokenResponse.AccessToken, tokenResponse.ExpiresIn, null);

            return tokenResponse.AccessToken;
        }
    }
}