using GoodGoals.Models;
using GoodGoals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoodGoals.Controllers.Api
{
    [Route("api/notes")]
    [ApiController]
    [Authorize]
    public class NotesApiController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotesApiController(INoteService noteService, UserManager<ApplicationUser> userManager)
        {
            _noteService = noteService;
            _userManager = userManager;
        }

        private string CurrentUserId => _userManager.GetUserId(User)!;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetAll()
        {
            var notes = await _noteService.GetUserNotesAsync(CurrentUserId);
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetById(int id)
        {
            var note = await _noteService.GetByIdAsync(id, CurrentUserId);
            if (note == null) return NotFound(new { message = "Nota no encontrada." });
            return Ok(note);
        }

        [HttpPost]
        public async Task<ActionResult<Note>> Create([FromBody] Note note)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            note.UserId = CurrentUserId;
            await _noteService.CreateAsync(note);
            return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Note note)
        {
            if (id != note.Id) return BadRequest(new { message = "El id de la ruta no coincide con el del cuerpo." });
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _noteService.UpdateAsync(note, CurrentUserId);
            if (!ok) return NotFound(new { message = "Nota no encontrada." });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _noteService.DeleteAsync(id, CurrentUserId);
            if (!ok) return NotFound(new { message = "Nota no encontrada." });
            return NoContent();
        }
    }
}
