using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.Models;
using Web.Services;

namespace Web
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _environment;
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            _config = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            var requireAuthenticatedUser = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            var builder = services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AuthorizeFilter(requireAuthenticatedUser));
            });

            if (_environment.IsDevelopment())
            {
                builder.AddRazorRuntimeCompilation();
            }
            services.AddSession(config =>
            {
                config.IdleTimeout = TimeSpan.FromSeconds(20);
                //config.Cookie.HttpOnly=true;
                config.Cookie.IsEssential = true;
            });
            services.AddHttpClient<ICatalogService, CatalogService>(c =>
            {
                c.BaseAddress = new Uri(_config["TruckCatalogUri"]);
            });
            services.AddHttpClient<IBasketService, BasketService>(c =>
            {
                c.BaseAddress = new Uri(_config["BasketUri"]);
                //c.DefaultRequestHeaders.Add("api-version", "2.0");

            });
            services.AddHttpClient<IOrderService, OrderService>(c =>
            {
                c.BaseAddress = new Uri(_config["OrdersUri"]);
            });


            services.AddSingleton<Settings>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = "https://localhost:3520/";
                options.ClientId = "hesstoytrucksinteractive";
                options.ClientSecret = "c35569bc-1666-4f11-93c1-5793dc5491a6";
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=TruckCatalog}/{action=Index}/{id?}");
            });
        }
    }
}
