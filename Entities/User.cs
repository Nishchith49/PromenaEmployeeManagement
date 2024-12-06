//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PromenaEmployeeManagement.Entities
//{
//    [Table("users")] // Table name should follow plural form for consistency in naming conventions
//    public class User
//    {
//        // Primary Key for User
//        [Key]
//        public int Id { get; set; }

//        // Email of the User (must be unique and properly validated)
//        [Required(ErrorMessage = "Email is required.")]
//        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
//        [StringLength(100, ErrorMessage = "Email can't be longer than 100 characters.")]
//        public string Email { get; set; } = string.Empty;

//        // Password Hash (the hashed version of the user's password)
//        [Required]
//        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

//        // Password Salt (used in the hashing process for added security)
//        [Required]
//        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

//        // Token for Password Reset (optional, can be used to verify reset requests)
//        public string? PasswordResetToken { get; set; }

//        // The Date and Time when the user was created
//        [Required]
//        public DateTime Created { get; set; } = DateTime.Now;

//        // The Date and Time when the user's account will expire (or the token expiration if used for JWT)
//        [Required]
//        public DateTime Expires { get; set; } = DateTime.Now.AddMinutes(59); // Can be adjusted based on your expiration logic

//        // Optional: Can be added for soft delete or account status (e.g., 'is active' flag)
//        public bool IsActive { get; set; } = true; // Default is 'true' for active users

//        // Optional: Added for tracking when the user was last updated
//        public DateTime? Updated { get; set; }
//    }
//}
