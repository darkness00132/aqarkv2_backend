using Domain.Entities.UsersEnities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Brokers
{
    [Index(nameof(UserId))]
    public class BrokerProfile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string? LicenseNumber { get; set; }
        public DateTimeOffset? LicenseExpiryDate { get; set; }
        public string? Bio { get; set; }
        public int? YearsOfExperience { get; set; }
        public string Phone { get; set; }
        public string WhatsAppNumber { get; set; }
        public string? CoverPhoto { get; set; }
        public string? Address { get; set; }
        public bool IsVerified { get; set; } = false;
        public bool IsFeatured { get; set; } = false;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
