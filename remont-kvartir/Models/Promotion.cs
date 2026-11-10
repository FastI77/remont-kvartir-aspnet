using System.ComponentModel.DataAnnotations;

namespace remont_kvartir.Models
{
    public class Promotion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите заголовок акции")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите описание")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Скидка")]
        public string? Badge { get; set; }

        [Display(Name = "Действует до")]
        public DateTime? ValidUntil { get; set; }

        public bool IsActive { get; set; } = true;
    }
}