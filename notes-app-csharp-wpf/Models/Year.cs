using System.ComponentModel.DataAnnotations;

namespace notes_app_csharp_wpf.Models
{
    public class Year
    {
        [Key] public int Id { get; set; }
        [Required] public int YearNumber { get; set; }

        public Subject Subject { get; set; }
    }
}
