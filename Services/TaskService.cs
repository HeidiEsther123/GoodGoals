using GoodGoals.Models;
using GoodGoals.Repositories;

namespace GoodGoals.Services
{
    public class TaskService : ITaskService
    {
        private readonly IGenericRepository<TaskItem> _repository;

        public TaskService(IGenericRepository<TaskItem> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TaskItem>> GetUserTasksAsync(string userId)
        {
            var all = await _repository.GetAllAsync();
            return all.Where(t => t.UserId == userId).OrderBy(t => t.DueDate);
        }

        public async Task<TaskItem?> GetByIdAsync(int id, string userId)
        {
            var task = await _repository.GetByIdAsync(id);
            return task != null && task.UserId == userId ? task : null;
        }

        public async Task CreateAsync(TaskItem task)
        {
            task.CreatedAt = DateTime.Now;
            await _repository.AddAsync(task);
            await _repository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(TaskItem task, string userId)
        {
            var existing = await GetByIdAsync(task.Id, userId);
            if (existing == null) return false;

            existing.Title = task.Title;
            existing.DueDate = task.DueDate;
            existing.IsCompleted = task.IsCompleted;
            existing.GoalId = task.GoalId;

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

        public async Task<bool> ToggleCompletedAsync(int id, string userId)
        {
            var existing = await GetByIdAsync(id, userId);
            if (existing == null) return false;

            existing.IsCompleted = !existing.IsCompleted;
            _repository.Update(existing);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
