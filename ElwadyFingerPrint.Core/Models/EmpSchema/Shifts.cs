using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using General;

namespace EmpSchema
{
    [Table("Shifts",Schema ="Emp")]
    public class Shifts:GeneralFields
    {
        [Key]
        public int ShiftID { get; set; }

        [Required,
            MinLength(5),
            MaxLength(22)]
        public string ShiftName { get; set; } = "";

    }
}
