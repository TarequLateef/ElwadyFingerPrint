using ElwadyFingerPrint.Core.Models.EmpSchema;
using EmpSchema;
using Microsoft.EntityFrameworkCore;

namespace HrCodeFirstDB
{
    public class Hr_Db_CFContext:DbContext
    {
        public Hr_Db_CFContext(DbContextOptions<Hr_Db_CFContext> options)
            : base(options){}

        public DbSet<Department> Departments { get; set; }
        public DbSet<Jobs> Jobs { get; set; }
        public DbSet<CertificateTbl> Certificates { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<EmpDept> EmpDepts { get; set; }
        public DbSet<EmpJob> EmpJobs { get; set; }
        public DbSet<Shifts> Shifts { get; set; }
        public DbSet<EmpShift> EmpShifts { get; set; }
    }
}
