using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleCalender.Data;

namespace ScheduleCalendar.Controllers
{
    [ApiController, Route("[controller]")]
    public class AvailabilityController : ControllerBase
    {
        private readonly ScheduleCalendarDbContext _dbContext;

        public AvailabilityController(ScheduleCalendarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _dbContext.Availabilities.Include(a => a.User).ToListAsync();

            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Availability availability)
        {
            var userInDb = await _dbContext.Users.FindAsync(availability.UserEmail);

            if (userInDb is null) return BadRequest(new { message  =  "User does not exist"});

            var existingAvailability = await _dbContext.Availabilities.FirstOrDefaultAsync(a => a.UserEmail == availability.UserEmail && a.Date == availability.Date);

            if (existingAvailability is not null) return Ok();

            await _dbContext.Availabilities.AddAsync(availability);

            await _dbContext.SaveChangesAsync();

            return Created("/", null);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var availabilityInDb = await _dbContext.Availabilities.FindAsync(id);

            if (availabilityInDb is null) return Ok();

            _dbContext.Availabilities.Remove(availabilityInDb);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
