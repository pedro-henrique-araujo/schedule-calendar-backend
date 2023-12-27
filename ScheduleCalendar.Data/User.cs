using System.ComponentModel.DataAnnotations;

namespace ScheduleCalender.Data
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
    }
}
