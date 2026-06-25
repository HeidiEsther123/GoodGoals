using GoodGoals.Models;
using GoodGoals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoodGoals.Controllers.Api
{
    // Capa de Presentación (API): expone los datos de Metas como JSON.
    // Reutiliza la MISMA capa de Servicios que usa el controlador MVC (GoalsController),
    [Route("api/goals")]
    [ApiController]
    [Authorize]
    public class GoalsApiController : ControllerBase
    {
        private readonly IGoalService _goalService;
        private readonly UserManager<ApplicationUser> _userManager;

        public GoalsApiController(IGoalService goalService, UserManager<ApplicationUser> userManager)
        {
            _goalService = goalService;
            _userManager = userManager;
        }

        private string CurrentUserId => _userManager.GetUserId(User)!;

        // GET: api/goals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Goal>>> GetAll()
        {
            var goals = await _goalService.GetUserGoalsAsync(CurrentUserId);
            return Ok(goals);
        }

        // GET: api/goals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Goal>> GetById(int id)
        {
            var goal = await _goalService.GetByIdAsync(id, CurrentUserId);
            if (goal == null) return NotFound(new { message = "Meta no encontrada." });
            return Ok(goal);
        }

        // POST: api/goals
        [HttpPost]
        public async Task<ActionResult<Goal>> Create([FromBody] Goal goal)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            goal.UserId = CurrentUserId;
            await _goalService.CreateAsync(goal);
            return CreatedAtAction(nameof(GetById), new { id = goal.Id }, goal);
        }

        // PUT: api/goals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Goal goal)
        {
            if (id != goal.Id) return BadRequest(new { message = "El id de la ruta no coincide con el del cuerpo." });
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _goalService.UpdateAsync(goal, CurrentUserId);
            if (!ok) return NotFound(new { message = "Meta no encontrada." });
            return NoContent();
        }

        // DELETE: api/goals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _goalService.DeleteAsync(id, CurrentUserId);
            if (!ok) return NotFound(new { message = "Meta no encontrada." });
            return NoContent();
        }

        // PATCH: api/goals/5/toggle
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleCompleted(int id)
        {
            var ok = await _goalService.ToggleCompletedAsync(id, CurrentUserId);
            if (!ok) return NotFound(new { message = "Meta no encontrada." });
            return NoContent();
        }
    }
}
