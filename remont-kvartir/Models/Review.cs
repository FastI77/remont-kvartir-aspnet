using System.ComponentModel.DataAnnotations;

namespace remont_kvartir.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string AuthorName { get; set; } = string.Empty;

        [Required]
        [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5")]
        [Display(Name = "Оценка")]
        public int Rating { get; set; }

        [Required]
        [Display(Name = "Текст отзыва")]
        public string Text { get; set; } = string.Empty;

        [Display(Name = "Фотография (необязательно)")]
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsApproved { get; set; } = false;

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }


        public int? RepairRequestId { get; set; }
        public RepairRequest? RepairRequest { get; set; }
    }
}