using ScheduleCalender.Data;

namespace ScheduleCalendar.Data.Dto
{
    public class UserAvailabilitiesDto
    {
        public User User { get; set; }
        public List<DateTime> Availabilities { get; set; }
        public DateRangeDto DateRange { get; set; }
    }
}
