using remont_kvartir.Models;
using System.ComponentModel.DataAnnotations;

namespace remont_kvartir.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите имя")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите email")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Телефон")]
        [Phone(ErrorMessage = "Некорректный телефон")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(100, ErrorMessage = "Пароль должен содержать минимум {2} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите email")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }

    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Введите имя")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Телефон")]
        [Phone(ErrorMessage = "Некорректный телефон")]
        public string? Phone { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Введите текущий пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите новый пароль")]
        [StringLength(100, ErrorMessage = "Пароль должен содержать минимум {2} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class RepairRequestViewModel
    {
        [Required(ErrorMessage = "Выберите тип ремонта")]
        [Display(Name = "Тип ремонта")]
        public string RepairType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Опишите задачу")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Описание от 10 до 2000 символов")]
        [Display(Name = "Описание задачи")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Адрес")]
        public string? Address { get; set; }

        [Range(1, 10000, ErrorMessage = "Площадь должна быть от 1 до 10000 м²")]
        [Display(Name = "Площадь (м²)")]
        public double? Area { get; set; }
    }

    public class CabinetViewModel
    {
        public ApplicationUser User { get; set; } = null!;
        public List<RepairRequest> Requests { get; set; } = new();
        public int TotalRequests { get; set; }
        public int NewRequests { get; set; }
        public int InProgressRequests { get; set; }
        public int CompletedRequests { get; set; }
    }
}