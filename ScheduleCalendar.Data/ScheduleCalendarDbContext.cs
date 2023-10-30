using Microsoft.EntityFrameworkCore;

namespace ScheduleCalender.Data
{
    public class ScheduleCalendarDbContext : DbContext
    {
        public ScheduleCalendarDbContext(DbContextOptions<ScheduleCalendarDbContext> options) :base(options) { }
        public DbSet<User> Users { get; set; }

        public DbSet<Availability> Availabilities { get; set; }
    }
}