using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DesafioRadix.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace DesafioRadix
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DesafioRadixContext>(opt => opt.UseInMemoryDatabase("DesafioRadix"));
            services.AddMvc();

            // Register the Swagger generator:
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {
                    Title = "DesafioRadix",
                    Version = "v1",
                    Contact = new Contact { Name = "Daniel Amaral"}
                });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DesafioRadix - API v1");
            });

            app.UseMvc();
        }
    }
}
