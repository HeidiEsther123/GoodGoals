using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoodGoals.Models
{
    public class Goal
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título de la meta es obligatorio.")]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        private DateTime? _targetDate;
        public DateTime? TargetDate
        {
            get => _targetDate;
            set => _targetDate = value?.ToUniversalTime();
        }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
