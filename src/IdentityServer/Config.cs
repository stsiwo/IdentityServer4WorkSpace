// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

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
            return new IdentityResource[]
            {
                new IdentityResources.OpenId()
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
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client> 
            {
                new Client
                {
                    ClientId = "client",
                    // there are severals grant types like password, client,  
                    // and you can define teh type here
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                }
            };                                   
        }
        // go to Startup.cs for step 4
    }
}