using System.ComponentModel.DataAnnotations;

namespace PromenaEmployeeManagement.Models
{
    public class EmployeeDetailsModel
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name can't be longer than 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email can't be longer than 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [StringLength(15, ErrorMessage = "Phone Number can't be longer than 15 characters.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Hire Date is required.")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        [StringLength(100, ErrorMessage = "Designation can't be longer than 100 characters.")]
        public string Designation { get; set; }

        //public bool IsActive { get; set; } = true;

        [StringLength(500, ErrorMessage = "Image URL can't be longer than 500 characters.")]
        public string ImageUrl { get; set; }
    }

    public class EmployeeAttendanceModel
    {
        [Required(ErrorMessage = "Employee ID is required.")]
        public long EmployeeId { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan LoginTime { get; set; }

        [StringLength(255, ErrorMessage = "Remarks can't exceed 255 characters.")]
        public string? Remarks { get; set; }

        [StringLength(255, ErrorMessage = "Location can't exceed 255 characters.")]
        public string? Location { get; set; }

        [StringLength(255, ErrorMessage = "Image URL can't exceed 255 characters.")]
        public string? ImageUrl { get; set; }
    }
}
