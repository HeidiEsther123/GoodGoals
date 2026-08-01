using System;
using System.ComponentModel.DataAnnotations;

namespace GoodGoals.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título de la nota es obligatorio.")]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
    }
}