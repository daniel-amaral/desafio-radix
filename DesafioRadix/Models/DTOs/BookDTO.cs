using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioRadix.Models.Entities;

namespace DesafioRadix.Models.DTOs
{
    public class BookDTO
    {
        /*
         * Omits the 'BookId' atribute 
         */

        public long ISBN { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Publisher { get; set; }
        public double Price { get; set; }

        public Book ConvertToBook()
        {
            return new Book
            {
                ISBN = this.ISBN,
                Title = this.Title,
                Authors = this.Authors,
                Publisher = this.Publisher,
                Price = this.Price
            };
        }
    }
}
