using GoodGoals.Models;
using GoodGoals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoodGoals.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IGoalService _goalService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TasksController(ITaskService taskService, IGoalService goalService, UserManager<ApplicationUser> userManager)
        {
            _taskService = taskService;
            _goalService = goalService;
            _userManager = userManager;
        }

        private string CurrentUserId => _userManager.GetUserId(User)!;

        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetUserTasksAsync(CurrentUserId);
            return View(tasks);
        }

        public async Task<IActionResult> Details(int id)
        {
            var task = await _taskService.GetByIdAsync(id, CurrentUserId);
            if (task == null) return NotFound();
            return View(task);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Goals = await _goalService.GetUserGoalsAsync(CurrentUserId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,DueDate,GoalId")] TaskItem task)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Goals = await _goalService.GetUserGoalsAsync(CurrentUserId);
                return View(task);
            }

            task.UserId = CurrentUserId;
            await _taskService.CreateAsync(task);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskService.GetByIdAsync(id, CurrentUserId);
            if (task == null) return NotFound();
            ViewBag.Goals = await _goalService.GetUserGoalsAsync(CurrentUserId);
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,DueDate,IsCompleted,GoalId")] TaskItem task)
        {
            if (id != task.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Goals = await _goalService.GetUserGoalsAsync(CurrentUserId);
                return View(task);
            }

            var ok = await _taskService.UpdateAsync(task, CurrentUserId);
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskService.GetByIdAsync(id, CurrentUserId);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _taskService.DeleteAsync(id, CurrentUserId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleCompleted(int id)
        {
            await _taskService.ToggleCompletedAsync(id, CurrentUserId);
            return RedirectToAction(nameof(Index));
        }
    }
}
