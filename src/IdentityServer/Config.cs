// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

/**
 * step1) 
 *  - set up project using template
 *      - intall IdentityServer4.Templates ( dotnet new -i IdentityServer4.Templates )
 *      - create project folder (e.g., quickstart)
 *      - create src folder inside project folder (e.g., src)
 *      - place the template inside src folder (e.g., dotnet new is$empty -n IdentityServer)
 *          - this create necessary files for the template 
 *      - create sln for the project and associate with it (e.g., dotnet new sln -n Quickstart => dotnet sln add .\src.\IdentityServer.\IdentityServer.csproj)
 *      - also you can change settings like http protocol and port in Properties\launchSettings.json
 **/

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
        
        // step 2) define your resource server (protected resource by this identity server and its token) here
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource> 
            { 
                new ApiResource("api1", "My API")

            };
        }

        // step 3) define your client (app who request token and request protected resource with token)
        /**
         * caveat: 
         *  1. if encouver error of "unauthorized_client redirect_url_invalid", make sure RedirectUris value exactly match with your client one
         *      -> might disable ssl (sslPort: 0) and change default port (applicationUrls: "localhost:5002/...") in launchSettings.json in your client
         **/
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    // there are severals grant types like password, client,  
                    // and you can define teh type here
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    // use this to retrive the access token on the back channel
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },

                    // this allows requesting refresh tokens for long lived API access;
                    AllowOfflineAccess = true,
                }
            };                                   
        }

        // step 4) define users who use client
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",

                    Claims = new []
                    {
                        new Claim("name", "Alice"),
                        new Claim("website", "https://alice.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password",

                    // add any claim you want to provide to client as profile
                    // client receive this profile including login info (above) and claims (below) 
                    Claims = new []
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    }
                }
            };
        }
    }
}