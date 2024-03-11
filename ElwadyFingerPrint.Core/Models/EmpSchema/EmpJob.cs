using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using General;
namespace EmpSchema
{
    [Table("EmpJob",Schema ="Emp")]
    public class EmpJob:GeneralFields
    {
        [Key]
        public int EmpJobID { get; set; }

        [Required]
        [ForeignKey("Employees")]
        public int EmpID { get; set; }

        [Required]
        [ForeignKey("Jobs")]
        public int JobID { get; set; }

        public Employees? Employees { get; set; }
        public Jobs? Jobs { get; set; }
    }
}
