using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PromenaEmployeeManagement.Entities
{
    [Table("employee_details")]
    public class EmployeeDetails
    {
        [Key]
        [Column("employee_id")]
        public long EmployeeId { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name can't be longer than 100 characters.")]
        [Column("full_name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email can't be longer than 100 characters.")]
        [Column("email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [Column("password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [StringLength(15, ErrorMessage = "Phone Number can't be longer than 15 characters.")]
        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Hire Date is required.")]
        [DataType(DataType.Date)]
        [Column("hire_date")]
        public DateTime HireDate { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        [StringLength(100, ErrorMessage = "Designation can't be longer than 100 characters.")]
        [Column("designation")]
        public string Designation { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("image_url")]
        [StringLength(500, ErrorMessage = "Image URL can't be longer than 500 characters.")]
        public string ImageUrl { get; set; }

        [Required]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Column("updated_date")]
        public DateTime? UpdatedDate { get; set; }

        public ICollection<EmployeeAttendance> EmployeeAttendances { get; set; }
    }
}
