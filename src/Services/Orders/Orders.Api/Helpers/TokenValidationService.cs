using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Orders.Api.Helpers
{
    public class TokenValidationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TokenValidationService> _logger;

        public TokenValidationService(IHttpClientFactory httpClientFactory, ILogger<TokenValidationService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<bool> ValidateToken(string tokenToValidate, DateTime receivedAt)
        {
            var _client = _httpClientFactory.CreateClient();
            var discoveryDocResponse = await _client.GetDiscoveryDocumentAsync("https://localhost:3520");
            if (discoveryDocResponse.IsError)
            {
                throw new Exception(discoveryDocResponse.Error);
            }

            try
            {
                var issuerSigningKeys = new List<SecurityKey>();
                foreach (var webKey in discoveryDocResponse.KeySet.Keys)
                {
                    var e = Base64Url.Decode(webKey.E);
                    var n = Base64Url.Decode(webKey.N);
                    var key = new RsaSecurityKey(new RSAParameters { Exponent = e, Modulus = n })
                    {
                        KeyId = webKey.Kid
                    };
                    issuerSigningKeys.Add(key);
                }

                var validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = "orders",
                    ValidIssuer = "https://localhost:3520",
                    IssuerSigningKeys = issuerSigningKeys,
                    LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                    {
                        return expires.Value.ToUniversalTime() > receivedAt.ToUniversalTime();
                    }

                };

                _ = new JwtSecurityTokenHandler().ValidateToken(
                    tokenToValidate, validationParameters, out var rawValidatedToken);
                return true;
            }
            catch (SecurityTokenValidationException ex)
            {
                _logger.LogError(ex, "Validation failed");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "");
                return false;
            }
        }
    }
}