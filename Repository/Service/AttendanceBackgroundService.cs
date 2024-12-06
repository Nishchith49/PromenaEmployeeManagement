using PromenaEmployeeManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace PromenaEmployeeManagement.Repository.Service
{
    public class AttendanceBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AttendanceBackgroundService> _logger;

        public AttendanceBackgroundService(IServiceProvider serviceProvider, ILogger<AttendanceBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Delay until 6:30 PM
                    var now = DateTime.Now;
                    var today630PM = now.Date.AddHours(18).AddMinutes(30);
                    var delay = today630PM > now ? today630PM - now : today630PM.AddDays(1) - now;
                    _logger.LogInformation("AttendanceBackgroundService sleeping until {time}", today630PM);
                    await Task.Delay(delay, stoppingToken);

                    if (stoppingToken.IsCancellationRequested)
                        break;

                    // Create a new scope for database operations
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<PromenaEmployeeManagementContext>();
                        var today = DateTime.Now.Date;

                        var employeesWithoutAttendance = await context.EmployeeDetails
                            .Where(e => !context.EmployeeAttendances
                                .Any(a => a.EmployeeId == e.EmployeeId && a.Date == today))
                            .ToListAsync(stoppingToken);

                        foreach (var employee in employeesWithoutAttendance)
                        {
                            var attendance = new EmployeeAttendance
                            {
                                EmployeeId = employee.EmployeeId,
                                Date = today,
                                Status = "Absent",
                                Remarks = "No login today",
                                LoginTime = TimeSpan.Zero,
                                LateTime = TimeSpan.Zero,
                            };
                            context.EmployeeAttendances.Add(attendance);
                        }

                        await context.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("Attendance marked for employees not logged in on {date}", today);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while marking attendance.");
                }

                // Wait until the next 6:30 PM
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}
