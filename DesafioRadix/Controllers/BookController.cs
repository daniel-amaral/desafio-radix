using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DesafioRadix.Models;
using DesafioRadix.Models.Entities;
using DesafioRadix.Models.DTOs;
using System;

namespace DesafioRadix.Controllers
{
    /// <summary>
    /// Book Controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/book")]
    public class BookController : Controller
    {
        private readonly DesafioRadixContext _context;

        /// <summary>
        /// class constructor
        /// </summary>
        /// <param name="context"></param>
        public BookController(DesafioRadixContext context)
        {
            _context = context;            
        }

        /// <summary>
        /// Get all Books, with pagination
        /// </summary>
        /// <param name="count"></param>
        /// <param name="page"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get a Book by ID
        /// </summary>
        [HttpGet("{bookId}", Name = "GetBookById")]
        public IActionResult GetById(long bookId)
        {
            var persistedBook = _context.Books.FirstOrDefault(b => b.BookID == bookId);

            if (persistedBook == null)
                return NotFound();

            return new ObjectResult(persistedBook);
        }

        /// <summary>
        /// Create a new Book
        /// </summary>
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

        /// <summary>
        /// Update a Book
        /// </summary>
        [HttpPut("{bookId}")]
        public IActionResult Update(long bookId, [FromBody] BookDTO requestBookDTO)
        {
            if (requestBookDTO == null)
                return BadRequest();

            var persistedBook = _context.Books.FirstOrDefault(b => b.BookID == bookId);

            if (persistedBook == null)
                return NotFound();

            persistedBook.UpdateFromDTO(requestBookDTO);
            _context.Books.Update(persistedBook);
            _context.SaveChanges();

            return new NoContentResult();
        }

        /// <summary>
        /// Delete a Book
        /// </summary>
        [HttpDelete("{bookId}")]
        public IActionResult Delete(long bookId)
        {
            var persistedBook = _context.Books.FirstOrDefault(b => b.BookID == bookId);

            if (persistedBook == null)
                return NotFound();

            _context.Books.Remove(persistedBook);
            _context.SaveChanges();

            return new NoContentResult();
        }
    }
}
