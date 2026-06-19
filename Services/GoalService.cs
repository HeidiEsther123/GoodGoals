using GoodGoals.Models;
using GoodGoals.Repositories;

namespace GoodGoals.Services
{
    // Capa de Lógica de Negocio: reglas propias de Metas (Goals).
    public class GoalService : IGoalService
    {
        private readonly IGenericRepository<Goal> _repository;
        private readonly IGenericRepository<TaskItem> _taskRepository;

        public GoalService(IGenericRepository<Goal> repository, IGenericRepository<TaskItem> taskRepository)
        {
            _repository = repository;
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<Goal>> GetUserGoalsAsync(string userId)
        {
            var all = await _repository.GetAllAsync();
            return all.Where(g => g.UserId == userId).OrderByDescending(g => g.CreatedAt);
        }

        public async Task<Goal?> GetByIdAsync(int id, string userId)
        {
            var goal = await _repository.GetByIdAsync(id);
            return goal != null && goal.UserId == userId ? goal : null;
        }

        public async Task CreateAsync(Goal goal)
        {
            goal.CreatedAt = DateTime.Now;
            await _repository.AddAsync(goal);
            await _repository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Goal goal, string userId)
        {
            var existing = await GetByIdAsync(goal.Id, userId);
            if (existing == null) return false;

            existing.Title = goal.Title;
            existing.Description = goal.Description;
            existing.TargetDate = goal.TargetDate;
            existing.IsCompleted = goal.IsCompleted;

            _repository.Update(existing);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var existing = await GetByIdAsync(id, userId);
            if (existing == null) return false;

            // Como la FK Task->Goal es Restrict, primero desvinculamos
            // las tareas asociadas para que no quede una referencia rota.
            var allTasks = await _taskRepository.GetAllAsync();
            var relatedTasks = allTasks.Where(t => t.GoalId == id);
            foreach (var task in relatedTasks)
            {
                task.GoalId = null;
                _taskRepository.Update(task);
            }
            if (relatedTasks.Any())
            {
                await _taskRepository.SaveChangesAsync();
            }

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