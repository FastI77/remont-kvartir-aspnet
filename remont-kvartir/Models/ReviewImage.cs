namespace remont_kvartir.Models
{
    public class ReviewImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int ReviewId { get; set; }
        public Review? Review { get; set; }
    }
}