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
        private readonly UserManager<ApplicationUser> _userManager;

        public NotesController(INoteService noteService, UserManager<ApplicationUser> userManager)
        {
            _noteService = noteService;
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

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] Note note)
        {
            if (!ModelState.IsValid) return View(note);
            note.UserId = CurrentUserId;
            await _noteService.CreateAsync(note);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var note = await _noteService.GetByIdAsync(id, CurrentUserId);
            if (note == null) return NotFound();
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content")] Note note)
        {
            if (id != note.Id) return NotFound();
            if (!ModelState.IsValid) return View(note);

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
