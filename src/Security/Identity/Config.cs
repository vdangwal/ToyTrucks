﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using IdentityServer4.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Identity
{
    public static class Config
    {
        private static IConfiguration _config;

        public static void AddConfiguration(IConfiguration config)
        {
            _config = config != null ? config : throw new ArgumentNullException(nameof(config));
        }
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("basket.fullaccess"),
                  new ApiScope("catalog.read"),
                  new ApiScope("catalog.write"),
                  new ApiScope("discount.fullaccess"),
                  new ApiScope("orders.fullaccess"),
                  new ApiScope("hesstoysgateway.fullaccess")
            };
        public static IEnumerable<ApiResource> ApiResources =>
           new ApiResource[]
           {

                new ApiResource("catalog","Hess Toys catalog Apis")
                {
                    Scopes = {"catalog.read", "catalog.write"}
                },
                 new ApiResource("basket","Hess Toys basket Apis")
                {
                    Scopes = {"basket.fullaccess"}
                },
                 new ApiResource("discount","Hess Toys discount Apis")
                {
                    Scopes = {"discount.fullaccess"}
                },
                 new ApiResource("hesstoysgateway","Hess Toys  Apis")
                {
                    Scopes = {"hesstoysgateway.fullaccess"}
                },
                  new ApiResource("orders","Hess Toys orders Apis")
                {
                    Scopes = {"orders.fullaccess"}
                },
           };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
              new Client
                {
                    ClientId = "hesstoytrucks",
                    ClientName = "Hess Toys Client",
                    ClientSecrets = { new Secret("3322cccf-b6ff-4558-aefb-6c159cd566a0".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,// GrantTypes.CodeAndClientCredentials,
                    RedirectUris={ $"{_config["FrontEndUri"]}signin-oidc"},
                    PostLogoutRedirectUris={$"{_config["FrontEndUri"]}signout-callback-oidc"},
                    //PostLogoutRedirectUris={ "https://localhost:6501/signout-callback-oidc"},

                    // RequireConsent = false,
                    // AllowOfflineAccess = true, //refresh token
                    // AccessTokenLifetime = 60,
                    AllowedScopes = {"openid","profile" 
                        // "basket.fullaccess",
                        // "hesstoysgateway.fullaccess",
                        // // "orders.fullaccess"
                        }
                },
                new Client{
                    ClientId = "hesstoytrucksm2m",
                    ClientName = "Hess Toys Loginless",
                    ClientSecrets ={new Secret("661efc22-efff-42b9-a2c1-3844a4a810ff".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes={"hesstoysgateway.fullaccess"}
                },
                 new Client
                {
                    ClientId = "hesstoytrucks_gateway_to_apis_tokenexchange",
                    ClientName = "Hess Toys Gateway to downstream Token exchange Client",
                    ClientSecrets = { new Secret("775e5143-2eff-476e-9986-576557877d15".Sha256()) },
                    AllowedGrantTypes = new[] { "urn:ietf:params:oauth:grant-type:token-exchange" },
                    RequireConsent = false,
                    AllowedScopes = {
                         "openid", "profile", "catalog.read", "basket.fullaccess" }
                },
                 new Client
                {
                    ClientId = "hesstoytrucks_baskets_to_downstream_tokenexchange",
                    ClientName = "Hess Toys Discount",
                    ClientSecrets = { new Secret("b438b4c0-9963-444d-882f-74a754e667d1".Sha256()) },
                    AllowedGrantTypes =new[]{"urn:ietf:params:oauth:grant-type:token-exchange"},

                    AllowedScopes = {"openid","profile" ,"discount.fullaccess","orders.fullaccess"}
                },
            };
    }
}