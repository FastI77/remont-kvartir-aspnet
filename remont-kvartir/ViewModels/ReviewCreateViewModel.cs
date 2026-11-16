using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
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
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "От 10 до 1000 символов")]
        public string Text { get; set; } = string.Empty;

        [Display(Name = "Фотографии (до 5 шт., необязательно)")]
        public List<IFormFile>? ImageFiles { get; set; }

        public List<RepairRequest> AvailableRequests { get; set; } = new List<RepairRequest>();
    }
}