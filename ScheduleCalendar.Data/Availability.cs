namespace ScheduleCalender.Data
{
    public class Availability
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public User? User { get; set; }
        public string UserId { get; set; }
    }
}
