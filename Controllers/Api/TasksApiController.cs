using GoodGoals.Models;
using GoodGoals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoodGoals.Controllers.Api
{
    [Route("api/tasks")]
    [ApiController]
    [Authorize]
    public class TasksApiController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TasksApiController(ITaskService taskService, UserManager<ApplicationUser> userManager)
        {
            _taskService = taskService;
            _userManager = userManager;
        }

        private string CurrentUserId => _userManager.GetUserId(User)!;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
        {
            var tasks = await _taskService.GetUserTasksAsync(CurrentUserId);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(id, CurrentUserId);
            if (task == null) return NotFound(new { message = "Tarea no encontrada." });
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create([FromBody] TaskItem task)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            task.UserId = CurrentUserId;
            await _taskService.CreateAsync(task);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskItem task)
        {
            if (id != task.Id) return BadRequest(new { message = "El id de la ruta no coincide con el del cuerpo." });
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _taskService.UpdateAsync(task, CurrentUserId);
            if (!ok) return NotFound(new { message = "Tarea no encontrada." });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _taskService.DeleteAsync(id, CurrentUserId);
            if (!ok) return NotFound(new { message = "Tarea no encontrada." });
            return NoContent();
        }

        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleCompleted(int id)
        {
            var ok = await _taskService.ToggleCompletedAsync(id, CurrentUserId);
            if (!ok) return NotFound(new { message = "Tarea no encontrada." });
            return NoContent();
        }
    }
}
