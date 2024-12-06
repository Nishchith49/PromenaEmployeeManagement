using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PromenaEmployeeManagement.Entities
{
    [Table("Attendance")]
    public class EmployeeAttendance
    {
        [Key]
        [Column("attendance_id")]
        public long AttendanceId { get; set; }

        [Required(ErrorMessage = "Employee ID is required.")]
        [Column("employee_id")]
        public long EmployeeId { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        [Column("attendance_date")]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        [Column("login_time")]
        public TimeSpan LoginTime { get; set; }

        [DataType(DataType.Time)]
        [Column("late_time")]
        public TimeSpan? LateTime { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50, ErrorMessage = "Status can't exceed 50 characters.")]
        [Column("status")]
        public string? Status { get; set; }

        [StringLength(255, ErrorMessage = "Remarks can't exceed 255 characters.")]
        [Column("remarks")]
        public string? Remarks { get; set; }

        [StringLength(255, ErrorMessage = "Location can't exceed 255 characters.")]
        [Column("location")]
        public string? Location { get; set; }

        [StringLength(255, ErrorMessage = "ImageUrl can't exceed 255 characters.")]
        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [ForeignKey("EmployeeId")]
        public EmployeeDetails EmployeeDetails { get; set; }
    }

    //public enum AttendanceStatus
    //{
    //    Present,
    //    Absent,
    //    Pending
    //}
}
