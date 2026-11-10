using System.ComponentModel.DataAnnotations;

namespace remont_kvartir.Models
{
    public class PortfolioItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название проекта")]
        [Display(Name = "Название проекта")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите описание")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Обложка")]
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<PortfolioImage> Gallery { get; set; } = new();
    }
}