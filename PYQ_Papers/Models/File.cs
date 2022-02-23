using System.ComponentModel.DataAnnotations;

namespace PYQ_Papers.Models
{
    internal class File
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; }

        public int NumberOfTimesOpened { get; set; }

        public Year Year { get; set; }
    }
}
