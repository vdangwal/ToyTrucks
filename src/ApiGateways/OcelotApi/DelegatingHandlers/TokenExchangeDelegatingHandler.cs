using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.AspNetCore.AccessTokenManagement;
namespace OcelotApi.DelegatingHandlers
{
    public class TokenExchangeDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;
        private const string CLIENT_ID_FOR_TOKEN_EXCHANGE = "hesstoytrucks_gateway_to_apis_tokenexchange";

        public TokenExchangeDelegatingHandler(IHttpClientFactory httpClientFactory, IClientAccessTokenCache clientAccessTokenCache)
        {
            _httpClientFactory = httpClientFactory;
            _clientAccessTokenCache = clientAccessTokenCache;
        }

        private async Task<string> GetAccessToken(string incomingToken)
        {
            var item = await _clientAccessTokenCache.GetAsync(CLIENT_ID_FOR_TOKEN_EXCHANGE, null);
            if (item != null)
            {
                return item.AccessToken;
            }
            if (string.IsNullOrWhiteSpace(incomingToken))
            {
                throw new ArgumentNullException(nameof(incomingToken));
            }
            var (access_token, expiresIn) = await ExchangeToken(incomingToken);
            await _clientAccessTokenCache.SetAsync(CLIENT_ID_FOR_TOKEN_EXCHANGE, access_token, expiresIn, null);
            return access_token;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // extract the current token
            var incomingToken = request.Headers.Authorization.Parameter;

            var newToken = await GetAccessToken(incomingToken);

            // exchange it
            // var (newToken, expiresIn) = await ExchangeToken(incomingToken);

            // replace the incoming bearer token with our new one
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newToken);

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<(string, int)> ExchangeToken(string incomingToken)
        {
            if (string.IsNullOrWhiteSpace(incomingToken))
            {
                throw new ArgumentNullException(nameof(incomingToken));
            }
            var client = _httpClientFactory.CreateClient();

            var discoveryDocumentResponse = await client
                .GetDiscoveryDocumentAsync("https://localhost:3520/");
            if (discoveryDocumentResponse.IsError)
            {
                throw new Exception(discoveryDocumentResponse.Error);
            }

            var customParams = new Dictionary<string, string>
                {
                    { "subject_token_type", "urn:ietf:params:oauth:token-type:access_token"},
                    { "subject_token", incomingToken},
                    { "scope", "openid profile catalog.read basket.fullaccess" }
                };

            var tokenResponse = await client.RequestTokenAsync(new TokenRequest()
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                GrantType = "urn:ietf:params:oauth:grant-type:token-exchange",
                Parameters = Parameters.FromObject(customParams),
                ClientId = CLIENT_ID_FOR_TOKEN_EXCHANGE,
                ClientSecret = "775e5143-2eff-476e-9986-576557877d15",
            });

            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            return (tokenResponse.AccessToken, tokenResponse.ExpiresIn);

        }
    }
}


