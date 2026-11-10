using System.ComponentModel.DataAnnotations;

namespace remont_kvartir.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название услуги")]
        [Display(Name = "Название")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите краткое описание")]
        [Display(Name = "Краткое описание")]
        public string ShortDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите полное описание")]
        [Display(Name = "Полное описание")]
        public string FullDescription { get; set; } = string.Empty;

        [Display(Name = "Цена (текстом)")]
        public string? Price { get; set; }

        [Display(Name = "Категория (например, Квартиры, Коттеджи)")]
        public string? Category { get; set; }

        [Display(Name = "Картинка (URL или файл)")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Длительность (например, от 2 месяцев)")]
        public string? Duration { get; set; }
    }
}