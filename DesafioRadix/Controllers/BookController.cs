using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DesafioRadix.Models;
using DesafioRadix.Models.DTOs;

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

            if (!_context.Books.Any())
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

        [HttpGet("{id:long}", Name = "GetBookById")]
        public IActionResult GetById(long id)
        {
            var persistedBook = _context.Books.FirstOrDefault(b => b.BookID == id);

            if (persistedBook == null)
                return NotFound();

            return new ObjectResult(persistedBook);
        }

        [HttpPost]
        public IActionResult Create([FromBody] BookDTO bookDTO)
        {
            if (bookDTO == null)
                return BadRequest();

            Book createdBook = bookDTO.ConvertToBook();
            _context.Books.Add(createdBook);
            _context.SaveChanges();

            return CreatedAtRoute("GetBookById", new { id = createdBook.BookID }, createdBook);
        }

        [HttpPut("{id:long}")]
        public IActionResult Update(long id, [FromBody] BookDTO requestBookDTO)
        {
            if (requestBookDTO == null)
                return BadRequest();

            var persistedBook = _context.Books.FirstOrDefault(b => b.BookID == id);

            if (persistedBook == null)
                return NotFound();

            persistedBook.UpdateFromDTO(requestBookDTO);
            _context.Books.Update(persistedBook);
            _context.SaveChanges();

            return new NoContentResult();
        }

        [HttpDelete("{id:long}")]
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
