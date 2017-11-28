using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioRadix.Models
{
    public enum Evaluation
    {
        Terrible=1, Bad, Ok, Good, Marvelous
    }

    public class Review
    {
        [Key]
        [Required]
        public long ReviewID { get; set; }

        [Required]
        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }

        [Required]
        public Evaluation Evaluation { get; set; }

        public string ReviewText { get; set; }

        public string ReviewAuthor { get; set; }
    }
}
