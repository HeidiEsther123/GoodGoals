using GoodGoals.Models;

namespace GoodGoals.Services
{
    public interface IGoalService
    {
        Task<IEnumerable<Goal>> GetUserGoalsAsync(string userId);
        Task<Goal?> GetByIdAsync(int id, string userId);
        Task CreateAsync(Goal goal);
        Task<bool> UpdateAsync(Goal goal, string userId);
        Task<bool> DeleteAsync(int id, string userId);
        Task<bool> ToggleCompletedAsync(int id, string userId);
    }
}
