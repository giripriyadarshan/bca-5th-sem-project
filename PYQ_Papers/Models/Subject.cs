using System.ComponentModel.DataAnnotations;

namespace PYQ_Papers.Models
{
    internal class Subject
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SubjectName { get; set; }

        public Semester Semester { get; set; }
    }
}
