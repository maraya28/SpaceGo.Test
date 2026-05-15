using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations
{
    public class WalletStore(SpaceGoDbContext dbContext) : IWalletStore
    {
        public async Task<Wallet> GetWallet(string playerId)
        {
            var wallet = await dbContext.Wallets.SingleAsync(_ => _.PlayerId == playerId);
            return wallet;
        }

        public async Task Update()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}