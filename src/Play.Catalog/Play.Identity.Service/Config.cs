using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Play.Identity.Service
{
    public static class Config
    {
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId="frontend",
                    AllowedGrantTypes=[GrantType.AuthorizationCode],
                    RequireClientSecret=false,
                    RequirePkce = true,
                    RedirectUris=["http://localhost:3000"],
                    PostLogoutRedirectUris=["http://ocalhost:300/authentication/logout"],
                    AlwaysIncludeUserClaimsInIdToken=true,
                    AllowedScopes=[
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        "catalog.fullaccess",
                        "catalog.readaccess",
                        "catalog.writeacces",
                        "inventory.fullaccess",
                        "IdentityServerApi",
                        "trading.fullaccess",
                        "roles"
                        ]
                },
                new Client
                {
                    ClientId="postman",
                    AllowedGrantTypes=[GrantType.AuthorizationCode],
                    RequireClientSecret=false,
                    RequirePkce = true,
                    RedirectUris=["urn:ietf:wg:oauth:2.0:oob"],
                    //PostLogoutRedirectUris=["http://ocalhost:300/authentication/logout"],
                    AlwaysIncludeUserClaimsInIdToken=true,
                       AllowedScopes=[
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        "catalog.fullaccess",
                        "catalog.readaccess",
                        "catalog.writeaccess",
                        "inventory.fullaccess",
                        "IdentityServerApi",
                        "trading.fullaccess",
                        "roles"
                        ]
                }
            };
        public static  IEnumerable<ApiScope> ApiScopes =>
           new ApiScope[]
           {
               new ApiScope("catalog.fullaccess", "catalog.fullaccess"),
               new ApiScope("catalog.readaccess", "catalog.readaccess"),
               new ApiScope("catalog.writeaccess", "catalog.writeaccess"),
               new ApiScope("inventory.fullaccess", "inventory.fullaccess"),
               new ApiScope("IdentityServerApi", "IdentityServerApi"),
               new ApiScope("trading.fullaccess", "trading.fullaccess")
           };
        public static IEnumerable<ApiResource> ApiResources =>
         new ApiResource[]
         {              
                new ApiResource
                {
                    Name="catalog",
                    Scopes=["catalog.fullaccess","catalog.readaccess","catalog.writeaccess"],
                    UserClaims=["role"]
                },
                new ApiResource
                {
                    Name="inventory",
                    Scopes=["inventory.fullaccess"],
                    UserClaims=["role"]
                },
                 new ApiResource
                {
                    Name="Identity",
                    Scopes=["IdentityServerApi"],
                    UserClaims=["role"]
                },
                 new ApiResource
                {
                    Name="trading",
                    Scopes=["trading.fullaccess"],
                    UserClaims=["role"]
                }
         };
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]{
                 new IdentityResources.OpenId(),
                 new IdentityResources.Profile(),
                   new IdentityResources.Email(),
                     new IdentityResources.Address(),
                 new IdentityResource("roles",new []{"role"})
            };
    }
}
