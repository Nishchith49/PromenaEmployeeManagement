using System.Text.Json.Serialization;

namespace PromenaEmployeeManagement.Models
{
    public class LoginModel
    {
        [JsonPropertyName("EmailAddress")]
        public string Email { get; set; } 

        [JsonPropertyName("Password")]
        public string Password { get; set; }

        [JsonPropertyName("EmployeeImage")]
        public IFormFile EmployeeImage { get; set; }

        [JsonPropertyName("Location")]
        public string Location { get; set; }

        [JsonPropertyName("Remarks")]
        public string? Reason { get; set; }
    }
}
