namespace Infrastructure.Persistance
{
    public class Wallet
    {
        public Guid Id { get; set; }

        public required string PlayerId { get; set; }

        public long Balance { get; set; }
    }
}
