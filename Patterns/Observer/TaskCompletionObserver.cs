using GoodGoals.Models;
using GoodGoals.Services;

namespace GoodGoals.Patterns.Observer
{
    // Observador concreto: cuando una Meta se completa,
    // automáticamente marca todas sus Tareas como completadas.
    public class TaskCompletionObserver : IGoalObserver
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskCompletionObserver> _logger;

        public TaskCompletionObserver(ITaskService taskService, ILogger<TaskCompletionObserver> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        public async Task OnGoalCompletedAsync(Goal goal)
        {
            _logger.LogInformation(
                "[Observer] Meta '{Title}' completada — marcando tareas asociadas como completadas",
                goal.Title);

            var tasks = await _taskService.GetUserTasksAsync(goal.UserId);
            var relatedTasks = tasks.Where(t => t.GoalId == goal.Id && !t.IsCompleted);

            foreach (var task in relatedTasks)
            {
                await _taskService.ToggleCompletedAsync(task.Id, goal.UserId);
                _logger.LogInformation("[Observer] Tarea '{Title}' marcada como completada", task.Title);
            }
        }
    }
}
