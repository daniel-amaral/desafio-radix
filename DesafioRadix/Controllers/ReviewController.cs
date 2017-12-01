using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DesafioRadix.Models;
using Microsoft.EntityFrameworkCore;
using DesafioRadix.Models.DTOs;

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

        [HttpGet]
        public IEnumerable<Review> GetAll()
        {
            IEnumerable<Review> allPersistedReviews =
                _context.Reviews
                    .Include(r => r.Book)
                    .ToList();
            return allPersistedReviews;
        }

        [HttpGet("{id:long}", Name = "GetReviewById")]
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

            Review createdReview = requestReviewDTO.ConvertToReview(reviewBook);
            _context.Reviews.Add(createdReview);
            _context.SaveChanges();

            return CreatedAtRoute("GetReviewById", new { id = createdReview.ReviewID }, createdReview);
        }

        [HttpPut("{id:long}")]
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

            persistedReview.UpdateFromDTO(requestReviewDTO, reviewBook);
            _context.Reviews.Update(persistedReview);
            _context.SaveChanges();

            return new NoContentResult();
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            var persistedReview = _context.Reviews.FirstOrDefault(r => r.ReviewID == id);
            if (persistedReview == null)
                return NotFound();

            _context.Remove(persistedReview);
            _context.SaveChanges();

            return new NoContentResult();
        }

        [HttpGet("{bookId:long}/reviews")]
        public IActionResult GetReviewsByBookId(long bookId)
        {
            IEnumerable<Review> persistedReviews =
                _context.Reviews
                .Where(r => r.Book.BookID == bookId)
                .Include(r => r.Book);

            if (persistedReviews.Any())
                return new ObjectResult(persistedReviews);

            return NoContent();
        }
    }
}
