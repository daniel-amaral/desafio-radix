using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DesafioRadix.Models;

namespace DesafioRadix.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly DesafioRadixContext _context;

        public BookController(DesafioRadixContext context)
        {
            _context = context;

            if (_context.Books.Count() == 0)
            {
                _context.Books.Add(new Book
                {
                    ISBN = 111,
                    Title = "Test title",
                    Price = 123.34,
                    // Authors = "Test authors"
                });
                _context.Books.Add(new Book
                {
                    ISBN = 222,
                    Title = "Test title2",
                    Price = 234.35,
                    // Authors = "Test authors"
                });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Book> GetAll()
        {
            return _context.Books.ToList();
        }

        [HttpGet("{id}", Name = "GetBook")]
        public IActionResult GetById(long id)
        {
            var persistedBook = _context.Books.FirstOrDefault(b => b.BookID == id);
            if (persistedBook == null)
            {
                return NotFound();
            }
            return new ObjectResult(persistedBook);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Book book)
        {
            if (book == null)
                return BadRequest();

            _context.Books.Add(book);
            _context.SaveChanges();

            return CreatedAtRoute("GetBook", new { id = book.BookID }, book);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Book requestBook)
        {
            if (requestBook == null || requestBook.BookID != id)
                return BadRequest();

            var persistedBook = _context.Books.FirstOrDefault(b => b.BookID == id);
            if (persistedBook == null)
                return NotFound();

            persistedBook.ISBN = requestBook.ISBN;
            persistedBook.Title = requestBook.Title;
            persistedBook.Authors = requestBook.Title;
            persistedBook.Publisher = requestBook.Publisher;
            persistedBook.Price = requestBook.Price;

            _context.Books.Update(persistedBook);
            _context.SaveChanges();

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var persistedBook = _context.Books.FirstOrDefault(b => b.BookID == id);
            if (persistedBook == null)
                return NotFound();

            _context.Books.Remove(persistedBook);
            _context.SaveChanges();

            return new NoContentResult();
        }
    }
}
