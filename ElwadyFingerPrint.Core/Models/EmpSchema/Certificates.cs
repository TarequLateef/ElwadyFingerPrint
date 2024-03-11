using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using General;

namespace ElwadyFingerPrint.Core.Models.EmpSchema 
{
    [Table("Certificates", Schema ="Emp")]
    public class CertificateTbl:GeneralFields
    {
        [Key]
        public int CertID { get; set; }

        [Required]
        [MinLength(5),
            MaxLength(100)]
        public string CertName { get; set; } = "";
    }
}
