using System.ComponentModel.DataAnnotations;

namespace remont_kvartir.Models
{
    public class RepairRequest
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        [Required]
        [Display(Name = "Тип ремонта")]
        public string RepairType { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Описание задачи")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Адрес")]
        public string? Address { get; set; }

        [Display(Name = "Площадь (м²)")]
        public double? Area { get; set; }

        [Display(Name = "Статус")]
        public RequestStatus Status { get; set; } = RequestStatus.New;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    public enum RequestStatus
    {
        New = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4
    }
}