using System.ComponentModel.DataAnnotations;
using remont_kvartir.Models;

namespace remont_kvartir.ViewModels
{
    public class ReviewCreateViewModel
    {
        [Required(ErrorMessage = "Выберите ремонт, который хотите оценить")]
        [Display(Name = "Какой ремонт вы хотите оценить?")]
        public int RepairRequestId { get; set; }

        [Required(ErrorMessage = "Поставьте оценку")]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Напишите текст отзыва")]
        public string Text { get; set; } = string.Empty;

        public List<RepairRequest> AvailableRequests { get; set; } = new List<RepairRequest>();
    }
}