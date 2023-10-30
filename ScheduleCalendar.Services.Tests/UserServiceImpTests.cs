using Moq;
using ScheduleCalendar.Repositories;
using ScheduleCalender.Data;

namespace ScheduleCalendar.Services.Tests
{
    public class UserServiceImpTests
    {
        [Fact]
        public async Task GetAllUsersAsync_WhenCalled_ReturnsUsers()
        {
            var expectedUsers = new List<User>() { new() { Email = "a"} };
            var userRepositoryMock = new Mock<UserRepository>();
            var userService = new UserServiceImp(userRepositoryMock.Object);

            userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedUsers);
            var result = await userService.GetAllUsersAsync();

            Assert.Equal(expectedUsers, result);
        }
    }
}