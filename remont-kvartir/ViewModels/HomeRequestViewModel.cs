using System.ComponentModel.DataAnnotations;

namespace remont_kvartir.ViewModels
{
    public class HomeRequestViewModel
    {
        [Required(ErrorMessage = "Введите ваше имя")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 50 символов")]
        [RegularExpression(@"^[А-Яа-яЁёA-Za-z\s\-]+$", ErrorMessage = "Имя должно содержать только буквы, пробелы и дефисы")]

        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "Введите ваш телефон")]
        [RegularExpression(@"^\+7\s?\(\d{3}\)\s?\d{3}-\d{2}-\d{2}$", ErrorMessage = "Неверный формат телефона. Используйте +7 (999) 123-45-67")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Выберите тип ремонта")]
        public string RepairType { get; set; } = string.Empty;
    }
}