using DesafioRadix.Models;
using DesafioRadix.Models.DTOs;
using DesafioRadix.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioRadix
{
    public class DbInitializer
    {
        private static DesafioRadixContext _context;

        public DbInitializer(DesafioRadixContext context)
        {
            _context = context;

        }

        public void Run()
        {
            int numOfBooksInDatabase = _context.Books.Count();
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
                _context.Add<Book>(book);
            }
            _context.SaveChanges();

            foreach (ReviewDTO reviewDTO in reviewDTOs)
            {
                Book reviewBook = _context.Books.FirstOrDefault(b => b.BookID == reviewDTO.BookID);
                Review review = reviewDTO.ConvertToReview(reviewBook);
                _context.Add<Review>(review);
            }
            _context.SaveChanges();

            Console.WriteLine("Mocking finished!");
        }
    }
}
