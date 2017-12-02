using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using DesafioRadix.Models;
using DesafioRadix.Models.Entities;
using Microsoft.Extensions.Configuration;
using DesafioRadix.Models.DTOs;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Microsoft.Extensions.PlatformAbstractions;

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

            LoadMockedJsonDataToDatabase(context);
        }

        private static void LoadMockedJsonDataToDatabase(DesafioRadixContext context)
        {
            int numOfBooksInDatabase = context.Books.Count();
            if (numOfBooksInDatabase > 0)
            {
                Console.WriteLine("Database contains data. No mocking required.");
                return;
            }

            Console.WriteLine("Database contains no data. Starting mocking...");

            StreamReader streamReader;
            List<Book> books;
            List<ReviewDTO> reviewDTOs;

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            streamReader = new StreamReader("daniel_mocked_data/books_mocked_data.json");
            string json = streamReader.ReadToEnd();
            books = JsonConvert.DeserializeObject<List<Book>>(json, settings);

            streamReader = new StreamReader("daniel_mocked_data/reviews_mocked_data.json");
            json = streamReader.ReadToEnd();
            reviewDTOs = JsonConvert.DeserializeObject<List<ReviewDTO>>(json, settings);
            
            foreach (Book book in books)
            {
                context.Add<Book>(book);
            }
            context.SaveChanges();

            foreach (ReviewDTO reviewDTO in reviewDTOs)
            {
                Book reviewBook = context.Books.FirstOrDefault(b => b.BookID == reviewDTO.BookID);
                Review review = reviewDTO.ConvertToReview(reviewBook);
                context.Add<Review>(review);
            }
            context.SaveChanges();

            Console.WriteLine("Mocking finished!");
        }
    }
}
