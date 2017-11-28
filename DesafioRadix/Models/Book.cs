﻿// using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DesafioRadix.Models
{
    public class Book
    {
        [Key]
        [Required]
        public long BookID { get; set; }

        [Required]
        public long ISBN { get; set; }

        [Required]
        public string Title { get; set; }

        public string Authors { get; set; }

        public string Publisher { get; set; }

        public double Price { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        /*
        [Required]
        public DateTime CreatedDate { get; set; }
        [DefaultValue(null)]
        public DateTime UpdatedDate { get; set; }
        [DefaultValue(false)]
        public bool IsExcluded { get; set; }
        */
    }
}
