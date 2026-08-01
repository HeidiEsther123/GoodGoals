using GoodGoals.Models;
using GoodGoals.Repositories;

namespace GoodGoals.Services
{
    public class NoteService : INoteService
    {
        private readonly IGenericRepository<Note> _repository;

        public NoteService(IGenericRepository<Note> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Note>> GetUserNotesAsync(string userId)
        {
            var all = await _repository.GetAllAsync();
            return all.Where(n => n.UserId == userId).OrderByDescending(n => n.CreatedAt);
        }

        public async Task<Note?> GetByIdAsync(int id, string userId)
        {
            var note = await _repository.GetByIdAsync(id);
            return note != null && note.UserId == userId ? note : null;
        }

        public async Task CreateAsync(Note note)
        {
            note.CreatedAt = DateTime.UtcNow;
            await _repository.AddAsync(note);
            await _repository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Note note, string userId)
        {
            var existing = await GetByIdAsync(note.Id, userId);
            if (existing == null) return false;

            existing.Title = note.Title;
            existing.Content = note.Content;
            existing.GoalId = note.GoalId;

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
