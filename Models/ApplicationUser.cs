using Microsoft.AspNetCore.Identity;

namespace GoodGoals.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;

        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public ICollection<Note> Notes { get; set; } = new List<Note>();
        public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
    }
}
