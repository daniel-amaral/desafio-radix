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
        /// Get all books, with pagination
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
            int numOfItensToBeReturned = count > totalOfItensPersisted ? totalOfItensPersisted : count;
            int pageOffset = page - 1;
            int totalOfPages = numOfItensToBeReturned == 0 ?
                0 : (int)Math.Ceiling((double)totalOfItensPersisted / numOfItensToBeReturned);
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

        /// <summary>
        /// Get a book by ID
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
        /// Find books by its title, with pagination
        /// </summary>
        [HttpGet("title/{bookTitle}/count/{count}/page/{page}")]
        public IActionResult GetBooksByTitle(string bookTitle, int count, int page)
        {
            if (count <= 0 || page <= 0)
                return BadRequest();

            int totalOfItensPersisted = _context.Books.Where(b => b.Title.Contains(bookTitle)).Count();
            int numOfItensToBeReturned = count > totalOfItensPersisted ? totalOfItensPersisted : count;
            int pageOffset = page - 1;
            int totalOfPages = numOfItensToBeReturned == 0 ?
                0 : (int)Math.Ceiling((double)totalOfItensPersisted / numOfItensToBeReturned);
            IEnumerable<Book> paginatedQuery =
                _context.Books
                    .OrderBy(b => b.BookID)
                    .Where(b => b.Title.Contains(bookTitle))
                    .Skip(pageOffset * numOfItensToBeReturned)
                    .Take(numOfItensToBeReturned)
                    .ToList();

            PaginatedResult<Book> paginatedResponse = new PaginatedResult<Book>
                (paginatedQuery, totalOfItensPersisted, page,
                    paginatedQuery.Count(), totalOfPages);
            return new ObjectResult(paginatedResponse);
        }

        /// <summary>
        /// Find books by author's name, with pagination
        /// </summary>
        [HttpGet("author/{authorName}/count/{count}/page/{page}")]
        public IActionResult GetBooksByAuthorName(string authorName, int count, int page)
        {
            if (count <= 0 || page <= 0)
                return BadRequest();

            int totalOfItensPersisted = _context.Books.Where(b => b.Authors.Contains(authorName)).Count();
            int numOfItensToBeReturned = count > totalOfItensPersisted ? totalOfItensPersisted : count;
            int pageOffset = page - 1;
            int totalOfPages = numOfItensToBeReturned == 0 ?
                0 : (int)Math.Ceiling((double)totalOfItensPersisted / numOfItensToBeReturned);
            IEnumerable<Book> paginatedQuery =
                _context.Books
                    .OrderBy(b => b.BookID)
                    .Where(b => b.Authors.Contains(authorName))
                    .Skip(pageOffset * numOfItensToBeReturned)
                    .Take(numOfItensToBeReturned)
                    .ToList();

            PaginatedResult<Book> paginatedResponse = new PaginatedResult<Book>
                (paginatedQuery, totalOfItensPersisted, page,
                    paginatedQuery.Count(), totalOfPages);
            return new ObjectResult(paginatedResponse);
        }

        /// <summary>
        /// Find books by publisher, with pagination
        /// </summary>
        [HttpGet("publisher/{publisher}/count/{count}/page/{page}")]
        public IActionResult GetBooksByPublisher(string publisher, int count, int page)
        {
            if (count <= 0 || page <= 0)
                return BadRequest();

            int totalOfItensPersisted = _context.Books.Where(b => b.Publisher.Contains(publisher)).Count();
            int numOfItensToBeReturned = count > totalOfItensPersisted ? totalOfItensPersisted : count;
            int pageOffset = page - 1;
            int totalOfPages = numOfItensToBeReturned == 0 ?
                0 : (int)Math.Ceiling((double)totalOfItensPersisted / numOfItensToBeReturned);
            IEnumerable<Book> paginatedQuery =
                _context.Books
                    .OrderBy(b => b.BookID)
                    .Where(b => b.Publisher.Contains(publisher))
                    .Skip(pageOffset * numOfItensToBeReturned)
                    .Take(numOfItensToBeReturned)
                    .ToList();

            PaginatedResult<Book> paginatedResponse = new PaginatedResult<Book>
                (paginatedQuery, totalOfItensPersisted, page,
                    paginatedQuery.Count(), totalOfPages);
            return new ObjectResult(paginatedResponse);
        }

        /// <summary>
        /// Find books by ISBN, with pagination
        /// </summary>
        [HttpGet("isbn/{isbn}/count/{count}/page/{page}")]
        public IActionResult GetBooksByISBN(string isbn, int count, int page)
        {
            if (count <= 0 || page <= 0)
                return BadRequest();

            int totalOfItensPersisted = _context.Books.Where(b => b.ISBN.Contains(isbn)).Count();
            int numOfItensToBeReturned = count > totalOfItensPersisted ? totalOfItensPersisted : count;
            int pageOffset = page - 1;
            int totalOfPages = numOfItensToBeReturned == 0 ?
                0 : (int)Math.Ceiling((double)totalOfItensPersisted / numOfItensToBeReturned);
            IEnumerable<Book> paginatedQuery =
                _context.Books
                    .OrderBy(b => b.BookID)
                    .Where(b => b.ISBN.Contains(isbn))
                    .Skip(pageOffset * numOfItensToBeReturned)
                    .Take(numOfItensToBeReturned)
                    .ToList();

            PaginatedResult<Book> paginatedResponse = new PaginatedResult<Book>
                (paginatedQuery, totalOfItensPersisted, page,
                    paginatedQuery.Count(), totalOfPages);
            return new ObjectResult(paginatedResponse);
        }

        /// <summary>
        /// Create a new book
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
        /// Update a book
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
        /// Delete a book
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
