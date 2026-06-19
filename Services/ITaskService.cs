using GoodGoals.Models;

namespace GoodGoals.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetUserTasksAsync(string userId);
        Task<TaskItem?> GetByIdAsync(int id, string userId);
        Task CreateAsync(TaskItem task);
        Task<bool> UpdateAsync(TaskItem task, string userId);
        Task<bool> DeleteAsync(int id, string userId);
        Task<bool> ToggleCompletedAsync(int id, string userId);
    }
}
