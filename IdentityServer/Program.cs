// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;

namespace IdentityServer
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                // uncomment to write to Azure diagnostics stream
                //.WriteTo.File(
                //    @"D:\home\LogFiles\Application\identityserver.txt",
                //    fileSizeLimitBytes: 1_000_000,
                //    rollOnFileSizeLimit: true,
                //    shared: true,
                //    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            try
            {
                var host = CreateHostBuilder(args).Build();

                // Auto migrate database and create new user but use CreateAsync.
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<ApplicationDbContext>();
                        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                        context.Database.Migrate();

                        if (!context.Users.Any())
                        {
                            var user = new ApplicationUser
                            {
                                UserName = "Alparslan",
                                Email = "bybluestht@gmail.com"
                            };

                            var result = userManager.CreateAsync(user,"A.lparslan123").Result;

                            if (result.Succeeded)
                            {
                                var role = new IdentityRole
                                {
                                    Name = "Administrator"
                                };

                                var roleResult = roleManager.CreateAsync(role).Result;

                                if (roleResult.Succeeded)
                                {
                                    var roleAddResult = userManager.AddToRoleAsync(user, role.Name).Result;

                                    if (!roleAddResult.Succeeded)
                                    {
                                        throw new Exception("Kullanıcıya Rol Eklenemedi.!");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Role Oluşturulamadı.!");
                                }
                            }
                            else
                            {
                                throw new Exception("Kullanıcı Oluşturulamamıştır.!");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Database Migration Sırasıda Bir Hata Oluştu.!");
                    }
                }  

                Log.Information("Starting host...");
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}