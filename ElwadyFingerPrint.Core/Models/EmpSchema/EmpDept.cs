using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using General;
namespace EmpSchema
{
    [Table("EmpDept",Schema ="Emp")]
    public class EmpDept:GeneralFields
    {
        [Key]
        public int EmpDeptID { get; set; }
        
        [Required]
        [ForeignKey("Employees")]
        public int EmpID { get; set; }
        
        [Required]
        [ForeignKey("Department")]
        public int DeptID { get; set; }

        public Employees? Employees { get; set; }
        public Department? Department { get; set; }
    }
}
