using Microsoft.EntityFrameworkCore;

namespace PromenaEmployeeManagement.Entities
{
    public class PromenaEmployeeManagementContext:DbContext
    {
        public PromenaEmployeeManagementContext(DbContextOptions<PromenaEmployeeManagementContext> options)
            : base(options)
        {
        }
        public DbSet<EmployeeDetails> EmployeeDetails { get; set; }
        public DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
        //public DbSet<User> Users { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<EmployeeDetails>().ToTable("EmployeeDetails");
        //    //modelBuilder.Entity<EmployeeAttendance>().ToTable("Attendance");
        //}
    }
}
