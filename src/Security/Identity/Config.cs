// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
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

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
              new Client
                {
                    ClientId = "hesstoytrucks",
                    ClientName = "Hess Toys Client",
                    ClientSecrets = { new Secret("3322cccf-b6ff-4558-aefb-6c159cd566a0".Sha256()) },
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RedirectUris={"https://localhost:6501/signin-oidc"},
                    PostLogoutRedirectUris={"https://localhost:6501/signout-callback-oidc"},
                    AllowedScopes = {"openid","profile" ,
                        "basket.fullaccess",
                        "catalog.read", "catalog.write",
                        "hesstoysgateway.fullaccess",
                        "orders.fullaccess"
                        }
                },
                 new Client
                {
                    ClientId = "hesstoytrucks_baskets_to_discount_tokenexchange",
                    ClientName = "Hess Toys Discount",
                    ClientSecrets = { new Secret("b438b4c0-9963-444d-882f-74a754e667d1".Sha256()) },
                    AllowedGrantTypes =new[]{"urn:ietf:params:oauth:grant-type:token-exchange"},

                    AllowedScopes = {"openid","profile" ,"discount.fullaccess"}
                },
                   // new Client
                // {
                //     ClientId = "hesstoytrucksm2m",
                //     ClientName = "Hess Toys Machine 2 Machine Client",
                //     ClientSecrets = { new Secret("4f3765a1-052b-498a-bcb1-ac3997b37c4c".Sha256()) },
                //     AllowedGrantTypes = GrantTypes.ClientCredentials,
                //     AllowedScopes = { "catalog.fullaccess" }
                // },
                // new Client
                // {
                //     ClientId = "hesstoytrucksinteractive",
                //     ClientName = "Hess Toys Interactive Client",
                //     ClientSecrets = { new Secret("c35569bc-1666-4f11-93c1-5793dc5491a6".Sha256()) },
                //     AllowedGrantTypes = GrantTypes.Code,

                //     RedirectUris={"https://localhost:6501/signin-oidc"},
                //     PostLogoutRedirectUris={"https://localhost:6501/signout-callback-oidc"},
                //     AllowedScopes = {"openid","profile" , "basket.fullaccess"}
                // },
            };
    }
}