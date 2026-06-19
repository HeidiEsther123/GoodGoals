using GoodGoals.Models;

namespace GoodGoals.Services
{
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetUserNotesAsync(string userId);
        Task<Note?> GetByIdAsync(int id, string userId);
        Task CreateAsync(Note note);
        Task<bool> UpdateAsync(Note note, string userId);
        Task<bool> DeleteAsync(int id, string userId);
    }
}
