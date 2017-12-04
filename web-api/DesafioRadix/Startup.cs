using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using DesafioRadix.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using Microsoft.Extensions.PlatformAbstractions;
using MySql.Data.MySqlClient;

/*
using MySqlDotnetCore.Data;
using MySqlDotnetCore.Models;
using MySqlDotnetCore.Services;
using MySQL.Data.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;
*/

namespace DesafioRadix
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString;

            string localConectionStr = Configuration.GetConnectionString("Local");
            string dockerFromExternalConectionStr = Configuration.GetConnectionString("DockerFromExternal");
            string withinDockerConectionStr = Configuration.GetConnectionString("WithinDocker");

            connectionString = withinDockerConectionStr;
            
            // in memory database:
            //services.AddDbContext<DesafioRadixContext>(opt => opt.UseInMemoryDatabase("DesafioRadix"));

            services.AddDbContext<DesafioRadixContext>(options =>
                options.UseMySql(connectionString));

            /*
            services.AddDbContext<DesafioRadixContext>(options =>
                        options.UseMySql(connectionString, mySqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 15,
                            maxRetryDelay: TimeSpan.FromSeconds(20),
                            errorNumbersToAdd: null);
                        }));
            */
            
            services.AddMvc();


            // Register the Swagger generator:
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "DesafioRadix",
                    Version = "v1",
                    Contact = new Contact { Name = "Daniel Amaral" }
                });

                // Set the comments path for the Swagger JSON and UI.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "DesafioRadix.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            // create database if not already created
            var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<DesafioRadixContext>();
            context.Database.Migrate();

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c
                => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DesafioRadix - API v1"));

            DbInitializer dbInitializer = new DbInitializer(context);
            dbInitializer.Run();
        }

       
    }
}
