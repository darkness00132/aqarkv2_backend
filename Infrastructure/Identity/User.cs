using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Identity
{
    public class User : IdentityUser<Guid>
    {
        public required string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int Credits { get; set; } = 0;

        public bool IsProfileCompleted { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
