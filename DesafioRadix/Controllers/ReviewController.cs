using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DesafioRadix.Models;
using Microsoft.EntityFrameworkCore;
using DesafioRadix.Models.Entities;
using DesafioRadix.Models.DTOs;

namespace DesafioRadix.Controllers
{
    [Produces("application/json")]
    [Route("api/review")]
    public class ReviewController : Controller
    {
        private readonly DesafioRadixContext _context;

        public ReviewController(DesafioRadixContext context)
        {
            _context = context;

            if (!_context.Reviews.Any())
            {
                long bookId = 1;
                var book = _context.Books.FirstOrDefault(b => b.BookID == bookId);
                _context.Reviews.Add(new Review
                {
                    Evaluation = 10,
                    Book = book
                });
                _context.Reviews.Add(new Review
                {
                    Evaluation = 9,
                    Book = book
                });

                _context.SaveChanges();
            }
        }

        [HttpGet("count/{count}/page/{page}")]
        public IActionResult GetAll(int count, int page)
        {
            if (count <= 0 || page <= 0)
                return BadRequest();

            int totalOfItensPersisted = _context.Reviews.Count();
            if (totalOfItensPersisted > 0)
            {
                int numOfItensToBeReturned = count > totalOfItensPersisted ? totalOfItensPersisted : count;
                int pageOffset = page - 1;
                int totalOfPages = (int)Math.Ceiling((double)totalOfItensPersisted / numOfItensToBeReturned);
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
            return NoContent();
        }

        [HttpGet("{id}", Name = "GetReviewById")]
        public IActionResult GetReviewById(long id)
        {
            var persistedReview = _context.Reviews
                .Include(r => r.Book)
                .FirstOrDefault(r => r.ReviewID == id);
            if (persistedReview == null)
                return NotFound();
            return new ObjectResult(persistedReview);
        }

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

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] ReviewDTO requestReviewDTO)
        {
            if (requestReviewDTO == null)
                return BadRequest();

            var persistedReview = _context.Reviews.FirstOrDefault(r => r.ReviewID == id);
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

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var persistedReview = _context.Reviews.FirstOrDefault(r => r.ReviewID == id);
            if (persistedReview == null)
                return NotFound();

            _context.Remove(persistedReview);
            _context.SaveChanges();

            return new NoContentResult();
        }

        [HttpGet("{bookId}/reviews/count/{count}/page/{page}")]
        public IActionResult GetReviewsByBookId(long bookId, int count, int page)
        {

            if (count <= 0 || page <= 0)
                return BadRequest();

            int totalOfItensPersisted = _context.Reviews.Where(r => r.Book.BookID == bookId).Count();
            if (totalOfItensPersisted > 0)
            {
                int numOfItensToBeReturned = count > totalOfItensPersisted ? totalOfItensPersisted : count;
                int pageOffset = page - 1;
                int totalOfPages = (int)Math.Ceiling((double)totalOfItensPersisted / numOfItensToBeReturned);
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

            return NoContent();
        }
    }
}
