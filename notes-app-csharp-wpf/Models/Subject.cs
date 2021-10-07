using System.ComponentModel.DataAnnotations;

namespace notes_app_csharp_wpf.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SubjectName { get; set; }

        public Semester Semester { get; set; }
    }
}
