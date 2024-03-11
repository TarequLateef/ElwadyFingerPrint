using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using General;

namespace EmpSchema
{
    [Table("Department", Schema ="Emp")]
    public class Department:GeneralFields
    {
        [Key]
        public int DeptID { get; set; }

        [Required]
        [MinLength(5),
            MaxLength(20)]
        public string DeptName { get; set; } = "";
    }
}
