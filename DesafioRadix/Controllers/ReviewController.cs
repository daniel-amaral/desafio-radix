using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DesafioRadix.Models;

namespace DesafioRadix.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ReviewController : Controller
    {
        private readonly DesafioRadixContext _context;

        public ReviewController(DesafioRadixContext context)
        {
            _context = context;

            if (_context.Reviews.Count() == 0)
            {
                long bookId = 1;
                var book = _context.Books.FirstOrDefault(b => b.BookID == bookId);
                _context.Reviews.Add(new Review
                {
                    Evaluation = 10,
                    Book = book
                });

                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Review> GetAll()
        {
            return _context.Reviews.ToList();
        }
    }
}
