using remont_kvartir.Models;

namespace remont_kvartir.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Service> LatestServices { get; set; } = new List<Service>();
        public IEnumerable<Review> LatestReviews { get; set; } = new List<Review>();
    }
}