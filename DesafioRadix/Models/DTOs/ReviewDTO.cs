﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioRadix.Models.DTOs
{
    public class ReviewDTO
    {
        /*
         * Omits the 'ReviewId' atribute 
         */

        public long BookID { get; set; }
        public int Evaluation { get; set; }
        public string ReviewText { get; set; }
        public string ReviewAuthor { get; set; }

        public Review ConvertToReview(Book reviewBook)
        {
            return new Review
            {
                Book = reviewBook,
                Evaluation = this.Evaluation,
                ReviewText = this.ReviewText,
                ReviewAuthor = this.ReviewAuthor
            };
        }
    }
}
