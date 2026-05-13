using Infrastructure.Persistance;

namespace Infrastructure.Contracts
{
    public interface IWalletRepository
    {
        public Task<Wallet> GetWallet(string playerId);

        public Task Update();
    }
}
