
namespace Application.DTOs.User
{
    public class BrokerItemList
    {
        public required string Slug { get; set; }

        public required string Name { get; set; }

        public string? ProfilePhoto { get; set; }
    }
}
