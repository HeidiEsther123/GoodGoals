using GoodGoals.Models;

namespace GoodGoals.Patterns.Factory
{
    // Interfaz de la fábrica: define el contrato para crear recordatorios
    public interface IReminderFactory
    {
        Reminder Create(string title, string userId, ReminderType type);
    }
}
