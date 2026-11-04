using System.ComponentModel.DataAnnotations;

namespace remont_kvartir.ViewModels
{
    public class HomeRequestViewModel
    {
        [Required(ErrorMessage = "Введите ваше имя")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите ваш телефон")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Выберите тип ремонта")]
        public string RepairType { get; set; } = string.Empty;
    }
}