using Microsoft.AspNetCore.Mvc;
using PromenaEmployeeManagement.Entities;
using PromenaEmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace PromenaEmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAttendanceController : ControllerBase
    {
        private readonly PromenaEmployeeManagementContext _context;

        public EmployeeAttendanceController(PromenaEmployeeManagementContext context)
        {
            _context = context;
        }

        // CREATE: Add a new attendance record
        [HttpPost]
        public async Task<IActionResult> CreateAttendance(EmployeeAttendanceModel attendance)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //if (attendance == null)
            //{
            //    return BadRequest("Attendance data is null.");
            //}
            var attendance1 = new EmployeeAttendance
            {
                EmployeeId = attendance.EmployeeId,
                Date = attendance.Date,
                LoginTime = attendance.LoginTime,
                Remarks = attendance.Remarks,
                Location = attendance.Location,
                ImageUrl = attendance.ImageUrl
            };

            _context.EmployeeAttendances.Add(attendance1);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAttendance), new { id = attendance1.AttendanceId }, attendance);
        }

        // READ: Get all attendance records
        [HttpGet]
        public async Task<IActionResult> GetAllAttendances()
        {
            var attendances = await _context.EmployeeAttendances.ToListAsync();
            return Ok(attendances);
        }

        // READ: Get a specific attendance record by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttendance(int id)
        {
            var attendance = await _context.EmployeeAttendances.FindAsync(id);

            if (attendance == null)
            {
                return NotFound($"Attendance record with ID {id} not found.");
            }

            return Ok(attendance);
        }

        // UPDATE: Update an existing attendance record
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttendance(int id, EmployeeAttendance attendance)
        {
            if (id != attendance.AttendanceId)
            {
                return BadRequest("Attendance ID mismatch.");
            }

            var existingAttendance = await _context.EmployeeAttendances.FindAsync(id);
            if (existingAttendance == null)
            {
                return NotFound($"Attendance record with ID {id} not found.");
            }

            existingAttendance.Date = attendance.Date;
            existingAttendance.LoginTime = attendance.LoginTime;
            existingAttendance.Remarks = attendance.Remarks;
            existingAttendance.Location = attendance.Location;
            existingAttendance.ImageUrl = attendance.ImageUrl;

            _context.Entry(existingAttendance).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content - update successful
        }

        // DELETE: Delete an attendance record by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            var attendance = await _context.EmployeeAttendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound($"Attendance record with ID {id} not found.");
            }

            _context.EmployeeAttendances.Remove(attendance);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content - delete successful
        }
    }
}
