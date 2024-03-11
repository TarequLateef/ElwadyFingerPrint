using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ElwadyFingerPrint.Core.Models.EmpSchema;
using General;

namespace EmpSchema
{
    
    [Table("Employees",Schema ="Emp"),
        /*Index("NationalNo", IsUnique = true, Name = "Emp_Uniqu_National")*/]
    public class Employees:GeneralFields
    {
        [Key]
        public int EmpID { get; set; }

        [Required]
        [MinLength(5),
            MaxLength(100)]
        public string EmpName { get; set; } = "";

        [Required]
        [StringLength(14)]
        public string NationalNo { get; set; } = "";

        [StringLength(11)]
        public string Phone { get; set; } = "";

        [Required]
        [MinLength(2),
            MaxLength(50)]
        public string InsuranceStatus { get; set; } = "";

            [MaxLength(10)]
        public string InsuranceNo { get; set; } = "";

        [DataType(DataType.Date)]
        public DateTime? InsuranceDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? InsuranceEndDate { get; set; }

        [StringLength(100)]
        public string? Address { get; set; }

        [Required]
        [ForeignKey("Cert")]
        public int CertID { get; set; }



        public CertificateTbl? Cert { get; set; }
    }
}
