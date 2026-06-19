using GoodGoals.Models;
using GoodGoals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoodGoals.Controllers.Api
{
    [Route("api/reminders")]
    [ApiController]
    [Authorize]
    public class RemindersApiController : ControllerBase
    {
        private readonly IReminderService _reminderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RemindersApiController(IReminderService reminderService, UserManager<ApplicationUser> userManager)
        {
            _reminderService = reminderService;
            _userManager = userManager;
        }

        private string CurrentUserId => _userManager.GetUserId(User)!;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reminder>>> GetAll()
        {
            var reminders = await _reminderService.GetUserRemindersAsync(CurrentUserId);
            return Ok(reminders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reminder>> GetById(int id)
        {
            var reminder = await _reminderService.GetByIdAsync(id, CurrentUserId);
            if (reminder == null) return NotFound(new { message = "Recordatorio no encontrado." });
            return Ok(reminder);
        }

        [HttpPost]
        public async Task<ActionResult<Reminder>> Create([FromBody] Reminder reminder)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            reminder.UserId = CurrentUserId;
            await _reminderService.CreateAsync(reminder);
            return CreatedAtAction(nameof(GetById), new { id = reminder.Id }, reminder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Reminder reminder)
        {
            if (id != reminder.Id) return BadRequest(new { message = "El id de la ruta no coincide con el del cuerpo." });
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _reminderService.UpdateAsync(reminder, CurrentUserId);
            if (!ok) return NotFound(new { message = "Recordatorio no encontrado." });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _reminderService.DeleteAsync(id, CurrentUserId);
            if (!ok) return NotFound(new { message = "Recordatorio no encontrado." });
            return NoContent();
        }
    }
}
