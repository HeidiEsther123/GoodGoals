using GoodGoals.Models;
using GoodGoals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoodGoals.Controllers
{
    [Authorize] // Solo usuarios autenticados pueden gestionar sus metas
    public class GoalsController : Controller
    {
        private readonly IGoalService _goalService;
        private readonly UserManager<ApplicationUser> _userManager;

        public GoalsController(IGoalService goalService, UserManager<ApplicationUser> userManager)
        {
            _goalService = goalService;
            _userManager = userManager;
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
            await _goalService.ToggleCompletedAsync(id, CurrentUserId);
            return RedirectToAction(nameof(Index));
        }
    }
}
