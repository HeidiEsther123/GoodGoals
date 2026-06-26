using GoodGoals.Models;

namespace GoodGoals.Patterns.Factory
{
    // Patrón Factory: centraliza la creación de objetos Reminder.
    // El controlador no necesita saber cómo se configura cada tipo —
    // solo pide "dame un recordatorio diario" y la fábrica lo construye.
    public class ReminderFactory : IReminderFactory
    {
        public Reminder Create(string title, string userId, ReminderType type)
        {
            var reminder = new Reminder
            {
                Title = title,
                UserId = userId,
                CreatedAt = DateTime.Now,
                IsSent = false
            };

            // Según el tipo, la fábrica configura la fecha del recordatorio
            reminder.RemindAt = type switch
            {
                ReminderType.OneTime => DateTime.Now.AddHours(1),
                ReminderType.Daily => DateTime.Now.AddDays(1),
                ReminderType.Weekly => DateTime.Now.AddDays(7),
                _ => DateTime.Now.AddHours(1)
            };

            // El título refleja el tipo de recordatorio
            reminder.Title = type switch
            {
                ReminderType.Daily => $"[Diario] {title}",
                ReminderType.Weekly => $"[Semanal] {title}",
                _ => title
            };

            return reminder;
        }
    }
}