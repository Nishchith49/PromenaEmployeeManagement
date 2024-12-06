using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromenaEmployeeManagement.Entities;
using PromenaEmployeeManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromenaEmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDetailsController : ControllerBase
    {
        private readonly PromenaEmployeeManagementContext _context;

        public EmployeeDetailsController(PromenaEmployeeManagementContext context)
        {
            _context = context;
        }

        // CREATE: api/employees
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDetailsModel employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //if (employee == null)
            //    return BadRequest("Employee details are required.");

            // Check if the email already exists
            var existingEmployee = await _context.EmployeeDetails
                .FirstOrDefaultAsync(e => e.Email == employee.Email);

            var employee1 = new EmployeeDetails
            {
                FullName = employee.FullName,
                Email = employee.Email,
                Password = employee.Password,
                PhoneNumber = employee.PhoneNumber,
                HireDate = employee.HireDate,
                Designation = employee.Designation,
                IsActive = true,
                ImageUrl = employee.ImageUrl,
                CreatedDate = DateTime.Now
            };

            if (existingEmployee != null)
                return BadRequest("Email is already in use.");

            _context.EmployeeDetails.Add(employee1);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployeeById), new { employeeId = employee1.EmployeeId }, employee);
        }

        // READ (single): api/employees/{employeeId}
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeById(int employeeId)
        {
            var employee = await _context.EmployeeDetails
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null)
                return NotFound("Employee not found.");

            return Ok(employee);
        }

        // READ (all): api/employees
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _context.EmployeeDetails.ToListAsync();
            return Ok(employees);
        }

        // UPDATE: api/employees/{employeeId}
        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, [FromBody] EmployeeDetails employee)
        {
            if (employee == null)
                return BadRequest("Employee details are required.");

            var existingEmployee = await _context.EmployeeDetails
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (existingEmployee == null)
                return NotFound("Employee not found.");

            existingEmployee.FullName = employee.FullName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Password = employee.Password;
            existingEmployee.PhoneNumber = employee.PhoneNumber;
            existingEmployee.HireDate = employee.HireDate;
            existingEmployee.Designation = employee.Designation;
            existingEmployee.IsActive = employee.IsActive;
            existingEmployee.ImageUrl = employee.ImageUrl;
            existingEmployee.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(existingEmployee);
        }

        // DELETE: api/employees/{employeeId}
        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            var employee = await _context.EmployeeDetails
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null)
                return NotFound("Employee not found.");

            _context.EmployeeDetails.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
