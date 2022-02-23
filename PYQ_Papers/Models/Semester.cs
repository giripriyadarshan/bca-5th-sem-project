using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PYQ_Papers.Models
{
    internal class Semester
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Sem { get; set; }
    }
}
