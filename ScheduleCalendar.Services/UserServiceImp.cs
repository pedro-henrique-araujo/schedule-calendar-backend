using ScheduleCalendar.Repositories;
using ScheduleCalender.Data;

namespace ScheduleCalendar.Services
{
    public class UserServiceImp
    {
        private UserRepository _userRepository;

        public UserServiceImp(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var list = await _userRepository.GetAllAsync();

            return list;
        }
    }
}