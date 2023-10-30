using ScheduleCalender.Data;

namespace ScheduleCalendar.Repositories
{
    public interface UserRepository
    {
        Task<List<User>> GetAllAsync();
    }
}
