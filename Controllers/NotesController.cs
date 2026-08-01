using GoodGoals.Models;
using GoodGoals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoodGoals.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly INoteService _noteService;
        private readonly IGoalService _goalService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotesController(INoteService noteService, IGoalService goalService, UserManager<ApplicationUser> userManager)
        {
            _noteService = noteService;
            _goalService = goalService;
            _userManager = userManager;
        }

        private string CurrentUserId => _userManager.GetUserId(User)!;

        public async Task<IActionResult> Index()
        {
            var notes = await _noteService.GetUserNotesAsync(CurrentUserId);
            return View(notes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var note = await _noteService.GetByIdAsync(id, CurrentUserId);
            if (note == null) return NotFound();
            return View(note);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Goals = await _goalService.GetUserGoalsAsync(CurrentUserId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,GoalId")] Note note)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Goals = await _goalService.GetUserGoalsAsync(CurrentUserId);
                return View(note);
            }
            note.UserId = CurrentUserId;
            await _noteService.CreateAsync(note);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var note = await _noteService.GetByIdAsync(id, CurrentUserId);
            if (note == null) return NotFound();
            ViewBag.Goals = await _goalService.GetUserGoalsAsync(CurrentUserId);
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,GoalId")] Note note)
        {
            if (id != note.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Goals = await _goalService.GetUserGoalsAsync(CurrentUserId);
                return View(note);
            }

            var ok = await _noteService.UpdateAsync(note, CurrentUserId);
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var note = await _noteService.GetByIdAsync(id, CurrentUserId);
            if (note == null) return NotFound();
            return View(note);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _noteService.DeleteAsync(id, CurrentUserId);
            return RedirectToAction(nameof(Index));
        }
    }
}
