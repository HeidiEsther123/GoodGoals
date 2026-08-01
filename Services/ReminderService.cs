using GoodGoals.Models;
using GoodGoals.Repositories;

namespace GoodGoals.Services
{
    public class ReminderService : IReminderService
    {
        private readonly IGenericRepository<Reminder> _repository;

        public ReminderService(IGenericRepository<Reminder> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Reminder>> GetUserRemindersAsync(string userId)
        {
            var all = await _repository.GetAllAsync();
            return all.Where(r => r.UserId == userId).OrderBy(r => r.RemindAt);
        }

        public async Task<Reminder?> GetByIdAsync(int id, string userId)
        {
            var reminder = await _repository.GetByIdAsync(id);
            return reminder != null && reminder.UserId == userId ? reminder : null;
        }

        public async Task CreateAsync(Reminder reminder)
        {
            // OJO: ya no pisamos CreatedAt con DateTime.Now (hora local).
            // El modelo Reminder ya trae CreatedAt = DateTime.UtcNow por defecto.
            // Si viniera sin setear, lo forzamos aquí también en UTC.
            if (reminder.CreatedAt == default)
            {
                reminder.CreatedAt = DateTime.UtcNow;
            }
            await _repository.AddAsync(reminder);
            await _repository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Reminder reminder, string userId)
        {
            var existing = await GetByIdAsync(reminder.Id, userId);
            if (existing == null) return false;

            existing.Title = reminder.Title;
            existing.RemindAt = reminder.RemindAt;
            existing.IsSent = reminder.IsSent;

            _repository.Update(existing);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var existing = await GetByIdAsync(id, userId);
            if (existing == null) return false;

            _repository.Delete(existing);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}