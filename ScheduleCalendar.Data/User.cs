using System.ComponentModel.DataAnnotations;

namespace ScheduleCalender.Data
{
    public class User
    {
        [Key]
        public string Email { get; set; }

        public string Name { get; set; }
        public string PhotoUrl { get; set; }


    }
}
