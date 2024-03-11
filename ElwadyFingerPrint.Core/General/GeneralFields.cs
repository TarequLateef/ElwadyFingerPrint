

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace General
{
    public class GeneralFields
    {

        [Required]
        [DataType(DataType.DateTime)]
        [DefaultValue(typeof(DateTime?))]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }

        public bool Avaliable => !EndDate.HasValue || EndDate.Value>=DateTime.Now;

        public int? OldID { get; set; }
    }
}
