using Infrastructure.Contracts;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations
{
    public class WalletRepository(SpaceGoDbContext dbContext) : IWalletRepository
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
