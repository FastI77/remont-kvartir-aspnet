using Microsoft.AspNetCore.Identity;

namespace remont_kvartir.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();
    }
}