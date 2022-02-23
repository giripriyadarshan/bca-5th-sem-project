using System.ComponentModel.DataAnnotations;

namespace PYQ_Papers.Models
{
    internal class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
