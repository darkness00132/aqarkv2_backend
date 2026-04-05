using Domain.Entities.UsersEnities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Brokers
{
    [Index(nameof(BrokerUserId), nameof(UserId), IsUnique = true)]
    public class BrokerReview
    {
        public int Id { get; set; }

        public Guid BrokerUserId { get; set; }
        public BrokerProfile BrokerProfile { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public required string Comment { get; set; }

        [Range(1, 5)]
        public required int Rating { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
