using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using General;

namespace EmpSchema
{
    [Table("EmpShift", Schema ="Shifts")]
    public class EmpShift:GeneralFields
    {
        [Key]
        public int EmpShiftID { get; set; }

        [Required,
            ForeignKey("Employees")]
        public int EmpID { get; set; }

        [Required,
            ForeignKey("Shifts")]
        public int ShiftID { get; set; }

        
        public Employees? Employees { get; set; }
        public Shifts? Shifts { get; set; }
    }
}
