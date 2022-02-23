using System.ComponentModel.DataAnnotations;

namespace PYQ_Papers.Models
{
    internal class Year
    {
        [Key] public int Id { get; set; }
        [Required] public int YearNumber { get; set; }

        public Subject Subject { get; set; }
    }
}
