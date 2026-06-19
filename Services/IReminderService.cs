using GoodGoals.Models;

namespace GoodGoals.Services
{
    public interface IReminderService
    {
        Task<IEnumerable<Reminder>> GetUserRemindersAsync(string userId);
        Task<Reminder?> GetByIdAsync(int id, string userId);
        Task CreateAsync(Reminder reminder);
        Task<bool> UpdateAsync(Reminder reminder, string userId);
        Task<bool> DeleteAsync(int id, string userId);
    }
}
