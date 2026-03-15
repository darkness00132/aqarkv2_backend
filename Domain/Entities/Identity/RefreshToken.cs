    using Domain.Identity;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace Domain.Identity
    {
        public class RefreshToken
        {
            public int Id { get; set; }

            [ForeignKey(nameof(User))]
            public required Guid UserId { get; set; }
            public User? User { get; set; }
            public required string TokenHash { get; set; }
            public required DateTime ExpiresAt { get; set; }
            public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public DateTime? RevokedAt { get; set; }
            public required string IpAddress { get; set; }
        }
    }
