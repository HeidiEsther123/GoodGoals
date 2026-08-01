using System;
using System.ComponentModel.DataAnnotations;

namespace GoodGoals.Models
{
    public class Reminder
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título del recordatorio es obligatorio.")]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        private DateTime _remindAt;
        [Required(ErrorMessage = "La fecha/hora del recordatorio es obligatoria.")]
        public DateTime RemindAt
        {
            get => _remindAt;
            set => _remindAt = value.ToUniversalTime();
        }

        public bool IsSent { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
    }
}