using GoodGoals.Models;
using GoodGoals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoodGoals.Controllers
{
    [Authorize]
    public class RemindersController : Controller
    {
        private readonly IReminderService _reminderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RemindersController(IReminderService reminderService, UserManager<ApplicationUser> userManager)
        {
            _reminderService = reminderService;
            _userManager = userManager;
        }

        private string CurrentUserId => _userManager.GetUserId(User)!;

        public async Task<IActionResult> Index()
        {
            var reminders = await _reminderService.GetUserRemindersAsync(CurrentUserId);
            return View(reminders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var reminder = await _reminderService.GetByIdAsync(id, CurrentUserId);
            if (reminder == null) return NotFound();
            return View(reminder);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,RemindAt")] Reminder reminder)
        {
            if (!ModelState.IsValid) return View(reminder);
            reminder.UserId = CurrentUserId;
            await _reminderService.CreateAsync(reminder);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reminder = await _reminderService.GetByIdAsync(id, CurrentUserId);
            if (reminder == null) return NotFound();
            return View(reminder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,RemindAt,IsSent")] Reminder reminder)
        {
            if (id != reminder.Id) return NotFound();
            if (!ModelState.IsValid) return View(reminder);

            var ok = await _reminderService.UpdateAsync(reminder, CurrentUserId);
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var reminder = await _reminderService.GetByIdAsync(id, CurrentUserId);
            if (reminder == null) return NotFound();
            return View(reminder);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _reminderService.DeleteAsync(id, CurrentUserId);
            return RedirectToAction(nameof(Index));
        }
    }
}
