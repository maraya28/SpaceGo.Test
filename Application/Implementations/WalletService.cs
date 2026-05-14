using Infrastructure.Contracts;

namespace Application.Implementations
{
    public class WalletService(IWalletStore repository) : IWallet
    {
        public long Balance { get; private set; }
          
        public async Task<long> Credit(string playerId, long amount)
        {
            Balance =+ amount;
            return await Task.FromResult(Balance);
        }

        public async Task<long> Debit(string playerId, long amount)
        {
            var wallet = await repository.GetWallet(playerId);
            wallet.Balance -= amount;

            await repository.Update();

            return wallet.Balance;

        }

        public async Task<long> GetBalance(string playerId)
        {
            var wallet = await repository.GetWallet(playerId);
            var balance = wallet.Balance;
            return balance;
        }
    }
}
