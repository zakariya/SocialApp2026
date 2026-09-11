namespace API.Entities
{
    public class Aggregator
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string ClientId { get; set; }
        public required string Url { get; set; }
        public required DateTime SubscriptionExpiry { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}