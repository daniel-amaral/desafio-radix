using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DesafioRadix.Models;
using DesafioRadix.Models.Entities;
using DesafioRadix.Models.DTOs;
using System;

namespace DesafioRadix.Controllers
{
    [Produces("application/json")]
    [Route("api/book")]
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
                    ISBN = "1111111-1",
                    Title = "Test title",
                    Price = 123.34,
                    // Authors = "Test authors"
                });
                _context.Books.Add(new Book
                {
                    ISBN = "22222222-2",
                    Title = "Test title2",
                    Price = 234.35,
                    // Authors = "Test authors"
                });
                _context.SaveChanges();
            }
        }

        [HttpGet("count/{count}/page/{page}")]
        public IActionResult GetAll(int count, int page)
        {

            if (count <= 0 || page <= 0)
                return BadRequest();

            int totalOfItensPersisted = _context.Books.Count();
            if (totalOfItensPersisted > 0)
            {
                int numOfItensToBeReturned = count > totalOfItensPersisted ? totalOfItensPersisted : count;
                int pageOffset = page - 1;
                int totalOfPages = (int)Math.Ceiling((double)totalOfItensPersisted / numOfItensToBeReturned);
                IEnumerable<Book> paginatedQuery =
                    _context.Books
                        .OrderBy(b => b.BookID)
                        .Skip(pageOffset * numOfItensToBeReturned)
                        .Take(numOfItensToBeReturned)
                        .ToList();

                PaginatedResult<Book> paginatedResponse = new PaginatedResult<Book>
                    (paginatedQuery, totalOfItensPersisted, page,
                     paginatedQuery.Count(), totalOfPages);
                return new ObjectResult(paginatedResponse);
            }
            return NoContent();
        }

        [HttpGet("{id}", Name = "GetBookById")]
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

        [HttpPut("{id}")]
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
