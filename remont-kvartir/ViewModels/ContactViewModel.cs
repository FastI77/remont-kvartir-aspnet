using System.ComponentModel.DataAnnotations;

namespace remont_kvartir.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Введите ваше имя")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 50 символов")]
        [RegularExpression(@"^[А-Яа-яЁёA-Za-z\s\-]+$", ErrorMessage = "Имя должно содержать только буквы")]
        [Display(Name = "Ваше имя")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите ваш Email")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        [Display(Name = "Ваш Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите сообщение")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Сообщение должно быть от 10 до 1000 символов")]
        [Display(Name = "Ваше сообщение")]
        public string Message { get; set; } = string.Empty;
    }
}