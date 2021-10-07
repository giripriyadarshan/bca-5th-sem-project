using System.ComponentModel.DataAnnotations;

namespace notes_app_csharp_wpf.Models
{
    public class File
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; }

        public Year Year { get; set; }


    }
}
