﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
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
                // new ApiResource("hesstoytrucks","Hess Toys Apis")
                // {
                //     Scopes = {"hesstoytrucks.fullaccess"}
                // }
                 new ApiResource("catalog","Hess Toys catalog Apis")
                {
                    Scopes = {"catalog.fullaccess"}
                },
                 new ApiResource("basket","Hess Toys basket Apis")
                {
                    Scopes = {"basket.fullaccess"}
                }
            };
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                // new ApiScope("hesstoytrucks.fullaccess"),
                  new ApiScope("catalog.fullaccess"),
                  new ApiScope("basket.fullaccess"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
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

                new Client
                {
                    ClientId = "hesstoytrucks",
                    ClientName = "Hess Toys Client",
                    ClientSecrets = { new Secret("3322cccf-b6ff-4558-aefb-6c159cd566a0".Sha256()) },
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RedirectUris={"https://localhost:6501/signin-oidc"},
                    PostLogoutRedirectUris={"https://localhost:6501/signout-callback-oidc"},
                    AllowedScopes = {"openid","profile" , "basket.fullaccess", "catalog.fullaccess"}
                },
            };
    }
}