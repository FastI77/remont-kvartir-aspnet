namespace remont_kvartir.Models
{
    public class PortfolioImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public int PortfolioItemId { get; set; }
        public PortfolioItem? PortfolioItem { get; set; }
    }
}