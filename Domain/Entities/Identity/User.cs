using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

    namespace Domain.Identity
    {
        public class User : IdentityUser<Guid>
        {
            public string? Slug { get; set; }

            public required string Name { get; set; }

            public string? ProfilePhoto { get; set; }

            public int Credits { get; set; } = 0;

            public bool IsProfileCompleted { get; set; } = false;

            public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

            public bool IsBlocked { get; set; } = false;

            public UserSecurity? Security { get; set; }

            public DateTime CreatedAt { get; set; }
        }
    }
