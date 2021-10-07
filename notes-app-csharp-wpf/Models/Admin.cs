using System.ComponentModel.DataAnnotations;

namespace notes_app_csharp_wpf.Models
{
    public class Admin
    {
        [Key]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
