using GoodGoals.Models;
using GoodGoals.Services;

namespace GoodGoals.Patterns.Decorator
{
    // Patrón Decorator: envuelve el GoalService original y agrega
    // logging automático en cada operación importante, SIN modificar
    // el código del servicio original (principio Open/Closed).
    public class LoggingGoalService : IGoalService
    {
        private readonly IGoalService _inner;
        private readonly ILogger<LoggingGoalService> _logger;

        public LoggingGoalService(IGoalService inner, ILogger<LoggingGoalService> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public async Task<IEnumerable<Goal>> GetUserGoalsAsync(string userId)
        {
            _logger.LogInformation("[Goals] Listando metas del usuario {UserId}", userId);
            return await _inner.GetUserGoalsAsync(userId);
        }

        public async Task<Goal?> GetByIdAsync(int id, string userId)
        {
            return await _inner.GetByIdAsync(id, userId);
        }

        public async Task CreateAsync(Goal goal)
        {
            _logger.LogInformation("[Goals] Creando meta '{Title}' para usuario {UserId}",
                goal.Title, goal.UserId);
            await _inner.CreateAsync(goal);
            _logger.LogInformation("[Goals] Meta '{Title}' creada exitosamente (Id={Id})",
                goal.Title, goal.Id);
        }

        public async Task<bool> UpdateAsync(Goal goal, string userId)
        {
            _logger.LogInformation("[Goals] Actualizando meta Id={Id}", goal.Id);
            var result = await _inner.UpdateAsync(goal, userId);
            _logger.LogInformation("[Goals] Meta Id={Id} actualizada: {Result}", goal.Id, result);
            return result;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            _logger.LogInformation("[Goals] Eliminando meta Id={Id}", id);
            var result = await _inner.DeleteAsync(id, userId);
            _logger.LogInformation("[Goals] Meta Id={Id} eliminada: {Result}", id, result);
            return result;
        }

        public async Task<bool> ToggleCompletedAsync(int id, string userId)
        {
            _logger.LogInformation("[Goals] Cambiando estado de meta Id={Id}", id);
            var result = await _inner.ToggleCompletedAsync(id, userId);
            _logger.LogInformation("[Goals] Estado de meta Id={Id} cambiado: {Result}", id, result);
            return result;
        }
    }
}
