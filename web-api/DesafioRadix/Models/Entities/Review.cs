using DesafioRadix.Models.DTOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioRadix.Models.Entities
{
    /*
    public enum Evaluation
    {
        Terrible, Bad, Ok, Good, Marvelous
    }
    */

    public class Review
    {
        [Key]
        [Required]
        public long ReviewID { get; set; }

        [Required]
        //[ForeignKey("BookID")]
        public Book Book { get; set; }

        /*
        [Required]
        public Evaluation Evaluation { get; set; }
        */

        private int _evaluation;
        [Required]
        public int Evaluation
        {
            get
            {
                return _evaluation;
            }
            set
            {
                if (value >= 0 && value <= 10)
                {
                    _evaluation = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }

            }
        }

        public string ReviewText { get; set; }

        public string ReviewAuthor { get; set; }

        public ReviewDTO ConvertToDTO()
        {
            return new ReviewDTO
            {
                BookID = this.Book.BookID,
                Evaluation = this.Evaluation,
                ReviewText = this.ReviewText,
                ReviewAuthor = this.ReviewAuthor
            };
        }

        public void UpdateFromDTO(ReviewDTO dto, Book book)
        {
            this.Book = Book;
            this.Evaluation = dto.Evaluation;
            this.ReviewText = dto.ReviewText;
            this.ReviewAuthor = dto.ReviewAuthor;
        }

    }
}
