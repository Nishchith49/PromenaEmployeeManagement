using PromenaEmployeeManagement.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromenaEmployeeManagement.Models;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using PromenaEmployeeManagement.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using PromenaEmployeeManagement.Repository.Service;
using Amazon.S3;

namespace PromenaEmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly PromenaEmployeeManagementContext _context;
        private readonly IConfiguration configuration;
        private readonly S3Service _s3Service;

        public AccountController(PromenaEmployeeManagementContext context, IConfiguration configuration,S3Service s3Service)
        {
            this._context = context;
            this.configuration = configuration;
            _s3Service = s3Service;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginModel request)
        {
            try
            {
                // Validate user
                var employeeDetails = _context.EmployeeDetails.FirstOrDefault(u => u.Email == request.Email);
                if (employeeDetails == null)
                {
                    return NotFound("User not found.");
                }

                if (employeeDetails.Password != request.Password)
                {
                    return BadRequest("Wrong password.");
                }

                // Upload image to S3 (if provided)
                string fileUrl = null;
                if (request.EmployeeImage != null)
                {
                    try
                    {
                        fileUrl = await _s3Service.UploadFileAsync(
                            request.EmployeeImage.OpenReadStream(),
                            request.EmployeeImage.FileName,
                            request.EmployeeImage.ContentType
                        );
                    }
                    catch (AmazonS3Exception s3Ex)
                    {
                        // Log the specific S3 exception
                        return StatusCode(500, new { Message = "Failed to upload image to S3.", Error = s3Ex.Message });
                    }
                    catch (Exception ex)
                    {
                        // General error handler
                        return StatusCode(500, new { Message = "Failed to upload image.", Error = ex.Message });
                    }
                }

                // Define expected login time (e.g., 9:00 AM)
                TimeSpan expectedLoginTime = new TimeSpan(9, 0, 0);

                // Get current login time
                TimeSpan currentLoginTime = DateTime.Now.TimeOfDay;

                // Calculate late hours
                TimeSpan lateHours = currentLoginTime > expectedLoginTime
                    ? currentLoginTime - expectedLoginTime
                    : TimeSpan.Zero;

                // Determine status based on late hours
                string status;
                string remarks;
                if (lateHours == TimeSpan.Zero)
                {
                    status = "Present";
                    remarks = "On time";
                }
                else
                {
                    status = lateHours.TotalMinutes <= 60 ? "Late" : "Absent";

                    // Format the late time in hours and minutes
                    int hoursLate = (int)lateHours.TotalHours;
                    int minutesLate = lateHours.Minutes;

                    // Prompt for reason if late
                    if (status == "Late")
                    {
                        if (string.IsNullOrEmpty(request.Reason))
                        {
                            return BadRequest("Reason for late login is required.");
                        }
                        remarks = $"Late by {hoursLate} hour{(hoursLate > 1 ? "s" : "")} {minutesLate} minute{(minutesLate > 1 ? "s" : "")}. Reason: {request.Reason}";
                    }
                    else
                    {
                        remarks = $"Absent due to login after {hoursLate} hour{(hoursLate > 1 ? "s" : "")} {minutesLate} minute{(minutesLate > 1 ? "s" : "")} past expected time.";
                    }
                }

                // Record attendance
                var employeeAttendance = new EmployeeAttendance
                {
                    EmployeeId = employeeDetails.EmployeeId,
                    ImageUrl = fileUrl,
                    Location = request.Location,
                    Date = DateTime.Now,
                    LoginTime = currentLoginTime,
                    LateTime = lateHours,
                    Status = status,
                    Remarks = remarks
                };

                _context.EmployeeAttendances.Add(employeeAttendance);
                await _context.SaveChangesAsync();

                // Format late time in hours and minutes for the response
                int hours = (int)lateHours.TotalHours;
                int minutes = lateHours.Minutes;


                return Ok(new
                {
                    AttendanceId = employeeAttendance.AttendanceId,
                    Message = "Login successful, attendance recorded.",
                    LateByhoursWithMinutes = $"{hours} hour{(hours > 1 ? "s" : "")} {minutes} minute{(minutes > 1 ? "s" : "")}",
                    Status = status,
                    Remarks = remarks,
                    FileUrl = fileUrl
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Error = ex.Message });
            }
        }



    }

    //private string CreateToken(EmployeeDetails user)
    //{
    //    List<Claim> claims = new List<Claim>
    //    {
    //        new Claim(ClaimTypes.Name, user.Email)
    //    };
    //    var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"] ?? throw new ArgumentNullException("JwtSettings:Key is not configured.")));
    //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
    //    var token = new JwtSecurityToken(
    //        claims: claims,
    //        //expires: DateTime.Now.AddMinutes(59),
    //        signingCredentials: creds
    //    );
    //    return new JwtSecurityTokenHandler().WriteToken(token);
    //}
}
