using GoodGoals.Models;
using GoodGoals.Patterns.Factory;
using GoodGoals.Patterns.Observer;
using GoodGoals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoodGoals.Controllers
{
    [Authorize]
    public class GoalsController : Controller
    {
        private readonly IGoalService _goalService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGoalSubject _goalSubject;
        private readonly TaskCompletionObserver _taskObserver;
        private readonly IReminderFactory _reminderFactory;
        private readonly IReminderService _reminderService;

        public GoalsController(
            IGoalService goalService,
            UserManager<ApplicationUser> userManager,
            IGoalSubject goalSubject,
            TaskCompletionObserver taskObserver,
            IReminderFactory reminderFactory,
            IReminderService reminderService)
        {
            _goalService = goalService;
            _userManager = userManager;
            _goalSubject = goalSubject;
            _taskObserver = taskObserver;
            _reminderFactory = reminderFactory;
            _reminderService = reminderService;

            // Registrar el observador de tareas en el gestor de eventos
            _goalSubject.Subscribe(_taskObserver);
        }

        private string CurrentUserId => _userManager.GetUserId(User)!;

        public async Task<IActionResult> Index()
        {
            var goals = await _goalService.GetUserGoalsAsync(CurrentUserId);
            return View(goals);
        }

        public async Task<IActionResult> Details(int id)
        {
            var goal = await _goalService.GetByIdAsync(id, CurrentUserId);
            if (goal == null) return NotFound();
            return View(goal);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,TargetDate")] Goal goal)
        {
            if (!ModelState.IsValid) return View(goal);

            goal.UserId = CurrentUserId;
            await _goalService.CreateAsync(goal);

            // Patrón Factory: crear automáticamente un recordatorio para la meta
            var reminder = _reminderFactory.Create(
                $"Revisa tu meta: {goal.Title}",
                CurrentUserId,
                ReminderType.Weekly);
            await _reminderService.CreateAsync(reminder);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var goal = await _goalService.GetByIdAsync(id, CurrentUserId);
            if (goal == null) return NotFound();
            return View(goal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,TargetDate,IsCompleted")] Goal goal)
        {
            if (id != goal.Id) return NotFound();
            if (!ModelState.IsValid) return View(goal);

            var ok = await _goalService.UpdateAsync(goal, CurrentUserId);
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var goal = await _goalService.GetByIdAsync(id, CurrentUserId);
            if (goal == null) return NotFound();
            return View(goal);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _goalService.DeleteAsync(id, CurrentUserId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleCompleted(int id)
        {
            var goal = await _goalService.GetByIdAsync(id, CurrentUserId);
            if (goal != null)
            {
                await _goalService.ToggleCompletedAsync(id, CurrentUserId);

                // Patrón Observer: si la meta se completó, notificar a los observadores
                if (!goal.IsCompleted) // era false, ahora se completó
                {
                    goal.IsCompleted = true;
                    await _goalSubject.NotifyGoalCompletedAsync(goal);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
