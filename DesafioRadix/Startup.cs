using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using DesafioRadix.Models;
using Microsoft.Extensions.Configuration;

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
            // in memory database:
            //services.AddDbContext<DesafioRadixContext>(opt => opt.UseInMemoryDatabase("DesafioRadix"));

            services.AddDbContext<DesafioRadixContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

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
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            // create database if not already created
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DesafioRadixContext>();
                context.Database.Migrate();
            }

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c 
                => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DesafioRadix - API v1"));
        }
    }
}
