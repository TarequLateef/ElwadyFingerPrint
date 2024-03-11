using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using General;

namespace EmpSchema;

[Table("Jobs",Schema ="Emp")]
public class Jobs:GeneralFields
{
    [Key]
    public int JobID { get; set; }

    [Required]
    [MaxLength(50),
        MinLength(5)]
    public string JobName { get; set; } = "";
}
