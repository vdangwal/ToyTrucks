using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Versioning;
using Inventory.Api.Services;
using Inventory.Api.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventory.Api", Version = "v1" });
            });
            services.AddPostgresDbContext(Configuration);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddApiVersioning(options =>
          {
              options.AssumeDefaultVersionWhenUnspecified = true;
              options.DefaultApiVersion = new ApiVersion(1, 0);
              options.ReportApiVersions = true;
              options.ApiVersionReader = new HeaderApiVersionReader("api-version");
          });

            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory.Api v1"));
            }

            //   app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
    public static class ServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddPostgresDbContext(this IServiceCollection services, IConfiguration config)
        {

            //Create  postgres container
            //docker run -e POSTGRES_DB=catalogdb -e POSTGRES_USER=marcus -e POSTGRES_PASSWORD=password -p 5432:5432 --name postgres_catalog -d postgres
            //run bash to access postgres container
            //docker exec -it 480c32e2bb53 "bash" //where 480c3.. is the container id

            //psql -h localhost -p 5432 -U marcus -d catalogdb
            // \l lists dbs 
            // \c <db> connects to db 
            // \d lists db objects
            var server = config["POSTGRES_SERVER"];// ?? "localhost";

            var port = config["POSTGRES_PORT"];// ?? "5432";
            var database = config["POSTGRES_DB"];// ?? "catalogdb";


            var user = config["POSTGRES_USER"];// ?? "marcus";
            var password = config["POSTGRES_PASSWORD"];// ?? "password";

            var connectionString = $"Host={server}; Port={port}; Database={database}; Username={user}; Password={password};";
            Console.WriteLine($"CONNECTION STRING Catalog: {connectionString}");

            //"User ID =postgres;Password=password;Server=localhost;Port=5432;Database=testDb;Integrated Security=true;Pooling=true;" //alternative
            services.AddDbContext<InventoryDbContext>(options =>
                options.UseNpgsql(connectionString)
                       .UseSnakeCaseNamingConvention()
                    );
            return services;
        }

    }
}
