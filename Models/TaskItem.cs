using System.ComponentModel.DataAnnotations;

namespace GoodGoals.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título de la tarea es obligatorio.")]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public int? GoalId { get; set; }
        public Goal? Goal { get; set; }
    }
}
