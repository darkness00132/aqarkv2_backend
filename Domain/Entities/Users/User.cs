using Domain.Entities.Brokers;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.UsersEnities
{
    public class User : IdentityUser<Guid>
    {
        
        public required string Name { get; set; }

        public string? ProfilePhoto { get; set; }

        public bool IsProfileCompleted { get; set; } = false;

        public bool IsBlocked { get; set; } = false;

        public UserAccountSecurity? Security { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<BrokerReview> Reviews { get; set; } = new List<BrokerReview>();
        public ICollection<BrokerReport> Reports { get; set; } = new List<BrokerReport>();

        public BrokerProfile? BrokerProfile { get; set; }
    }
}
