// using System;
using DesafioRadix.Models.DTOs;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DesafioRadix.Models.Entities
{
    public class Book
    {
        [Key]
        [Required]
        public long BookID { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Title { get; set; }

        public string Authors { get; set; }

        public string Publisher { get; set; }

        public double Price { get; set; }

        //public virtual ICollection<Review> Reviews { get; set; }

        /*
        [Required]
        public DateTime CreatedDate { get; set; }
        [DefaultValue(null)]
        public DateTime UpdatedDate { get; set; }
        [DefaultValue(false)]
        public bool IsExcluded { get; set; }
        */

        public BookDTO ConvertToDTO()
        {
            return new BookDTO
            {
                ISBN = this.ISBN,
                Title = this.Title,
                Authors = this.Authors,
                Publisher = this.Publisher,
                Price = this.Price
            };
        }

        public void UpdateFromDTO(BookDTO dto)
        {
            this.ISBN = dto.ISBN;
            this.Title = dto.Title;
            this.Authors = dto.Authors;
            this.Publisher = dto.Publisher;
            this.Price = dto.Price;
        }
    }
}
