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
                new ApiResource("hesstoytrucks","Hess Toys Apis")
                {
                    Scopes = {"hesstoytrucks.fullaccess"}
                }
            };
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("hesstoytrucks.fullaccess"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "hesstoytrucksm2m",
                    ClientName = "Hess Toys Machine 2 Machine Client",
                    ClientSecrets = { new Secret("4f3765a1-052b-498a-bcb1-ac3997b37c4c".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "hesstoytrucks.fullaccess" }
                },
                new Client
                {
                    ClientId = "hesstoytrucksinteractive",
                    ClientName = "Hess Toys Interactive Client",
                    ClientSecrets = { new Secret("c35569bc-1666-4f11-93c1-5793dc5491a6".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    //RedirectUris = { "https://localhost:5000/signin-oidc" },
                   // PostLogoutRedirectUris = { "https://localhost:5000/signout-callback-oidc" },
                    RedirectUris={"https://localhost:6501/signin-oidc"},  //6501?
                    PostLogoutRedirectUris={"https://localhost:6501/signout-callback-oidc"},
                    AllowedScopes = {"openid","profile" }
                },



                // // m2m client credentials flow client
                // new Client
                // {
                //     ClientId = "m2m.client",
                //     ClientName = "Client Credentials Client",

                //     AllowedGrantTypes = GrantTypes.ClientCredentials,
                //     ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                //     AllowedScopes = { "scope1" }
                // },

                // // interactive client using code flow + pkce
                // new Client
                // {
                //     ClientId = "interactive",
                //     ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                //     AllowedGrantTypes = GrantTypes.Code,

                //     RedirectUris = { "https://localhost:44300/signin-oidc" },
                //     FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                //     PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                //     AllowOfflineAccess = true,
                //     AllowedScopes = { "openid", "profile", "scope2" }
                // },
            };
    }
}