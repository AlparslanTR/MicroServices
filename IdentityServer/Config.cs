// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("resource_catalog"){Scopes={"catalog_full_permission"}},
                new ApiResource("resource_photo_stock"){Scopes={"photo_stock_full_permission"}},
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){Name="roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims=new []{"role"}}
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_full_permission","Catalog Api İçin Ful Erişim"),
                new ApiScope("photo_stock_full_permission", "Photo Stock Api İçin Ful Erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "WebMvcClient", // Client'ın benzersiz kimliği
                    ClientName = "Asp.Net Core MVC", // Client'ın adı
                    ClientSecrets = {new Secret("secret".Sha256())}, // Client'ın bilgilerini doğrulamak için kullanılan gizli anahtar
                    AllowedGrantTypes = GrantTypes.ClientCredentials, // Client Credentials Grant (Client Kimlik Bilgileri ile Doğrulama) kullanılıyor.
                    AllowedScopes = {"catalog_full_permission", // catalog_full_permission kapsamına erişim izni
                                     "photo_stock_full_permission", // photo_stock_full_permission kapsamına erişim izni
                                     IdentityServerConstants.LocalApi.ScopeName} // Local API kapsamına erişim izni
                },
                new Client
{
                    // Client'ın benzersiz kimliği
                    ClientId = "WebMvcClientForUser",

                    // Client'ın adı
                    ClientName = "Asp.Net Core MVC",

                    // OfflineAccess özelliği, kullanıcının çevrimdışı erişim (refresh token) almasına izin verir.
                    AllowOfflineAccess = true,

                    // Client'ın bilgilerini doğrulamak için kullanılan gizli anahtar
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    // Client'ın kullanabileceği OAuth 2.0 akış türleri. Burada Resource Owner Password Grant (Parola Sahibi Kimlik Doğrulama) kullanılıyor.
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    // Client'ın erişim izni isteyebileceği kapsamlar
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId, // OpenID Connect protokolü kapsamı
                        IdentityServerConstants.StandardScopes.Email,   // E-posta bilgilerine erişim
                        IdentityServerConstants.StandardScopes.Profile, // Profil bilgilerine erişim
                        IdentityServerConstants.StandardScopes.OfflineAccess, // Çevrimdışı erişim (refresh token) kapsamı
                        "roles", // Rol bilgilerine erişim
                        IdentityServerConstants.LocalApi.ScopeName // Local API kapsamı
                    },

                    // Access token'ın ömrü (saniye cinsinden)
                    AccessTokenLifetime = 1 * 60 * 60, // 1 saat

                    // Refresh token'ın geçerlilik süresi, TokenExpiration.Absolute kullanılarak belirtilmiştir.
                    RefreshTokenExpiration = TokenExpiration.Absolute,

                    // AbsoluteRefreshTokenLifetime, refresh token'ın tam geçerlilik süresini belirtir (saniye cinsinden)
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds, // 60 gün

                    // Refresh token'ın kullanımı
                    RefreshTokenUsage = TokenUsage.ReUse // Her zaman kullanılabilir
                },
            };
    }
}