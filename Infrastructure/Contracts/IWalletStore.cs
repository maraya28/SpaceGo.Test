namespace Infrastructure.Contracts
{
    public interface IWalletStore
    {
        public Task<Wallet> GetWallet(string playerId);

        public Task Update();
    }
}