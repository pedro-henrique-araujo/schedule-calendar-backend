using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleCalendar.Data.Dto;
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
        public async Task<IActionResult> Get([FromQuery] DateRangeWithEmailDto dateRangeWithEmail)
        {
            var availabilitiesQueryable = _dbContext.Availabilities.AsQueryable();

            if (dateRangeWithEmail.Start.HasValue)
            {
                availabilitiesQueryable = availabilitiesQueryable.Where(a => a.Date.Date >= dateRangeWithEmail.Start.Value.Date);
            }

            if (dateRangeWithEmail.End.HasValue)
            {
                availabilitiesQueryable = availabilitiesQueryable.Where(a => a.Date.Date <= dateRangeWithEmail.End.Value.Date);
            }

            if (dateRangeWithEmail.Email is not null)
            {
                availabilitiesQueryable = availabilitiesQueryable.Where(a => a.UserEmail == dateRangeWithEmail.Email);
            }

            availabilitiesQueryable = availabilitiesQueryable.Include(a => a.User);

            var list = await availabilitiesQueryable.ToListAsync();

            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Availability availability)
        {

            var userInDb = await _dbContext.Users.FindAsync(availability.UserEmail);

            if (userInDb is null) return BadRequest(new { message = "User does not exist" });

            var existingAvailability = await _dbContext.Availabilities.FirstOrDefaultAsync(a => a.UserEmail == availability.UserEmail && a.Date == availability.Date);

            if (existingAvailability is not null) return Ok();

            await _dbContext.Availabilities.AddAsync(availability);

            await _dbContext.SaveChangesAsync();

            return Created("/", null);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUserAvailabilities(UserAvailabilitiesDto userAvailability)
        {
            var userInDb = await _dbContext.Users.FindAsync(userAvailability.User.Email);

            if (userInDb is not null)
            {
                userInDb.PhotoUrl = userAvailability.User.PhotoUrl;
                userInDb.Name = userAvailability.User.Name;
            }
            else
            {
                await _dbContext.Users.AddAsync(userAvailability.User);
            }

            var availabilityInDb = await _dbContext.Availabilities
                .Where(a => a.Date.Date >= userAvailability.DateRange.Start.Date
                    && a.Date.Date <= userAvailability.DateRange.End.Date
                    && a.UserEmail == userInDb.Email)
                .ToListAsync();

            var availabilityDatesInDb = availabilityInDb.Select(a => a.Date);

            var addedAvailabilities = userAvailability.Availabilities
                .Where(a => availabilityDatesInDb.Contains(a) == false)
                .Select(a => new Availability
                {
                    Date = a.Date,
                    User = userInDb
                });

            _dbContext.Availabilities.AddRange(addedAvailabilities);

            var removedAvailabilities = availabilityInDb
                .Where(a => userAvailability.Availabilities.Contains(a.Date) == false);

            _dbContext.Availabilities.RemoveRange(removedAvailabilities);

            await _dbContext.SaveChangesAsync();
            return Ok();
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
