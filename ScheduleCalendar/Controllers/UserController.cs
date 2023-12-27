using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleCalender.Data;

namespace ScheduleCalendar.Controllers
{
    [ApiController, Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ScheduleCalendarDbContext _dbContext;

        public UserController(ScheduleCalendarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _dbContext.Users.ToListAsync();

            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(User user)
        {
            var userInDb = await _dbContext.Users.FindAsync(user.Id);

            if (userInDb is not null)
            {
                userInDb.PhotoUrl = user.PhotoUrl;
                userInDb.Name = user.Name;
                await _dbContext.SaveChangesAsync();
                return Ok();
            }

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return Created("/", null);
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userInDb = await _dbContext.Users.FindAsync(id);

            if (userInDb is null) return Ok();

            _dbContext.Users.Remove(userInDb);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
