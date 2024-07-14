using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiAdvanced.Model
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }

        [Required]
        public DateOnly Jobdate { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
