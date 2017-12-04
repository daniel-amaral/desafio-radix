using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DesafioRadix.Models;
using Microsoft.EntityFrameworkCore;
using DesafioRadix.Models.Entities;
using DesafioRadix.Models.DTOs;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace DesafioRadix.Controllers
{
    /// <summary>
    /// Review Controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/review")]
    public class ReviewController : Controller
    {
        private readonly DesafioRadixContext _context;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="context"></param>
        public ReviewController(DesafioRadixContext context) => _context = context;

        /// <summary>
        /// Get all reviews, with pagination
        /// </summary>
        /// <param name="count"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("count/{count}/page/{page}")]
        public IActionResult GetAll(int count, int page)
        {
            if (count <= 0 || page <= 0)
                return BadRequest();

            int totalOfItensPersisted = _context.Reviews.Count();
            int numOfItensToBeReturned = count > totalOfItensPersisted ? totalOfItensPersisted : count;
            int pageOffset = page - 1;
            int totalOfPages = numOfItensToBeReturned == 0 ?
                0 : (int)Math.Ceiling((double)totalOfItensPersisted / numOfItensToBeReturned);
            IEnumerable<Review> paginatedQuery =
                _context.Reviews
                    .OrderBy(r => r.ReviewID)
                    .Skip(pageOffset * numOfItensToBeReturned)
                    .Take(numOfItensToBeReturned)
                    .Include(r => r.Book)
                    .ToList();

            PaginatedResult<Review> paginatedResponse = new PaginatedResult<Review>
                (paginatedQuery, totalOfItensPersisted, page,
                    paginatedQuery.Count(), totalOfPages);
            return new ObjectResult(paginatedResponse);
        }


        /// <summary>
        /// Get a review by ID
        /// </summary>
        /// <param name="reviewId"></param>
        [HttpGet("{reviewId}", Name = "GetReviewById")]
        public IActionResult GetReviewById(long reviewId)
        {
            var persistedReview = _context.Reviews
                .Include(r => r.Book)
                .FirstOrDefault(r => r.ReviewID == reviewId);
            if (persistedReview == null)
                return NotFound();
            return new ObjectResult(persistedReview);
        }


        /// <summary>
        /// Find all reviews from a specific book, with pagination
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="count"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("book-id/{bookId}/count/{count}/page/{page}")]
        public IActionResult GetReviewsByBookId(long bookId, int count, int page)
        {

            if (count <= 0 || page <= 0)
                return BadRequest();

            int totalOfItensPersisted = _context.Reviews.Where(r => r.Book.BookID == bookId).Count();
            int numOfItensToBeReturned = count > totalOfItensPersisted ? totalOfItensPersisted : count;
            int pageOffset = page - 1;
            int totalOfPages = numOfItensToBeReturned == 0 ?
                0 : (int)Math.Ceiling((double)totalOfItensPersisted / numOfItensToBeReturned);
            IEnumerable<Review> paginatedQuery =
                _context.Reviews
                    .OrderBy(r => r.ReviewID)
                    .Where(r => r.Book.BookID == bookId)
                    .Skip(pageOffset * numOfItensToBeReturned)
                    .Take(numOfItensToBeReturned)
                    .Include(r => r.Book)
                    .ToList();

            PaginatedResult<Review> paginatedResponse = new PaginatedResult<Review>
                (paginatedQuery, totalOfItensPersisted, page,
                    paginatedQuery.Count(), totalOfPages);

            return new ObjectResult(paginatedResponse);
        }


        /// <summary>
        /// Find reviews by review author's name, with pagination
        /// </summary>
        [HttpGet("review-author/{reviewAuthorName}/count/{count}/page/{page}")]
        public IActionResult GetReviewsByReviewAuthorName(string reviewAuthorName, int count, int page)
        {
            if (count <= 0 || page <= 0)
                return BadRequest();

            int totalOfItensPersisted = _context.Reviews.Where(r => r.ReviewAuthor.Contains(reviewAuthorName)).Count();
            int numOfItensToBeReturned = count > totalOfItensPersisted ? totalOfItensPersisted : count;
            int pageOffset = page - 1;
            int totalOfPages = numOfItensToBeReturned == 0 ?
                0 : (int)Math.Ceiling((double)totalOfItensPersisted / numOfItensToBeReturned);
            IEnumerable<Review> paginatedQuery =
                _context.Reviews
                    .OrderBy(r => r.ReviewID)
                    .Where(r => r.ReviewAuthor.Contains(reviewAuthorName))
                    .Skip(pageOffset * numOfItensToBeReturned)
                    .Include(r => r.Book)
                    .Take(numOfItensToBeReturned)
                    .ToList();

            PaginatedResult<Review> paginatedResponse = new PaginatedResult<Review>
                (paginatedQuery, totalOfItensPersisted, page,
                    paginatedQuery.Count(), totalOfPages);
            return new ObjectResult(paginatedResponse);
        }


        /// <summary>
        /// Find reviews by books author's name, with pagination
        /// </summary>
        [HttpGet("book-author/{bookAuthorName}/count/{count}/page/{page}")]
        public IActionResult GetReviewsByBookAuthorName(string bookAuthorName, int count, int page)
        {
            if (count <= 0 || page <= 0)
                return BadRequest();

            int totalOfItensPersisted = 
                _context.Reviews.Where(r => r.Book.Authors.Contains(bookAuthorName)).Count();
            int numOfItensToBeReturned = count > totalOfItensPersisted ? totalOfItensPersisted : count;
            int pageOffset = page - 1;
            int totalOfPages = numOfItensToBeReturned == 0 ?
                0 : (int)Math.Ceiling((double)totalOfItensPersisted / numOfItensToBeReturned);
            IEnumerable<Review> paginatedQuery =
                _context.Reviews
                    .OrderBy(r => r.ReviewID)
                    .Where(r => r.Book.Authors.Contains(bookAuthorName))
                    .Skip(pageOffset * numOfItensToBeReturned)
                    .Include(r => r.Book)
                    .Take(numOfItensToBeReturned)
                    .ToList();

            PaginatedResult<Review> paginatedResponse = new PaginatedResult<Review>
                (paginatedQuery, totalOfItensPersisted, page,
                    paginatedQuery.Count(), totalOfPages);
            return new ObjectResult(paginatedResponse);
        }


        /// <summary>
        /// Find reviews by books ISBN, with pagination
        /// </summary>
        [HttpGet("book-isbn/{bookISBN}/count/{count}/page/{page}")]
        public IActionResult GetReviewsByBookISBN(string bookISBN, int count, int page)
        {
            if (count <= 0 || page <= 0)
                return BadRequest();

            int totalOfItensPersisted =
                _context.Reviews.Where(r => r.Book.ISBN.Contains(bookISBN)).Count();
            int numOfItensToBeReturned = count > totalOfItensPersisted ? totalOfItensPersisted : count;
            int pageOffset = page - 1;
            int totalOfPages = numOfItensToBeReturned == 0 ?
                0 : (int)Math.Ceiling((double)totalOfItensPersisted / numOfItensToBeReturned);
            IEnumerable<Review> paginatedQuery =
                _context.Reviews
                    .OrderBy(r => r.ReviewID)
                    .Where(r => r.Book.ISBN.Contains(bookISBN))
                    .Skip(pageOffset * numOfItensToBeReturned)
                    .Include(r => r.Book)
                    .Take(numOfItensToBeReturned)
                    .ToList();

            PaginatedResult<Review> paginatedResponse = new PaginatedResult<Review>
                (paginatedQuery, totalOfItensPersisted, page,
                    paginatedQuery.Count(), totalOfPages);
            return new ObjectResult(paginatedResponse);
        }


        /// <summary>
        /// Create a new review
        /// </summary>
        /// <param name="requestReviewDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] ReviewDTO requestReviewDTO)
        {
            if (requestReviewDTO == null)
                return BadRequest();

            Book reviewBook = _context.Books
                .FirstOrDefault(b => b.BookID == requestReviewDTO.BookID);
            if (reviewBook == null)
                return BadRequest();

            try
            {
                Review createdReview = requestReviewDTO.ConvertToReview(reviewBook);
                _context.Reviews.Add(createdReview);
                _context.SaveChanges();
                return CreatedAtRoute("GetReviewById", new { id = createdReview.ReviewID }, createdReview);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return BadRequest(e.Message);
            }

        }


        /// <summary>
        /// Update an existing review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="requestReviewDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(long reviewId, [FromBody] ReviewDTO requestReviewDTO)
        {
            if (requestReviewDTO == null)
                return BadRequest();

            var persistedReview = _context.Reviews.FirstOrDefault(r => r.ReviewID == reviewId);
            if (persistedReview == null)
                return NotFound();

            Book reviewBook = _context.Books
                .FirstOrDefault(b => b.BookID == requestReviewDTO.BookID);
            if (reviewBook == null)
                return BadRequest();

            try
            {
                persistedReview.UpdateFromDTO(requestReviewDTO, reviewBook);
                _context.Reviews.Update(persistedReview);
                _context.SaveChanges();
                return new NoContentResult();
            }
            catch (ArgumentOutOfRangeException e)
            {
                return BadRequest(e.Message);
            }

        }


        /// <summary>
        /// Delete a review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(long reviewId)
        {
            var persistedReview = _context.Reviews.FirstOrDefault(r => r.ReviewID == reviewId);
            if (persistedReview == null)
                return NotFound();

            _context.Remove(persistedReview);
            _context.SaveChanges();

            return new NoContentResult();
        }
    }
}
