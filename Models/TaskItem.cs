using System;
using System.ComponentModel.DataAnnotations;

namespace GoodGoals.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título de la tarea es obligatorio.")]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        // PostgreSQL exige DateTime en UTC para columnas "timestamp with time zone".
        // Usamos un campo privado + setter para convertir automáticamente.
        private DateTime? _dueDate;
        public DateTime? DueDate
        {
            get => _dueDate;
            set => _dueDate = value?.ToUniversalTime();
        }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public int? GoalId { get; set; }
        public Goal? Goal { get; set; }
    }
}
