using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Implementations
{
    public class WalletService : IWallet
    {
        public long Balance { get; private set; }

        public WalletService()
        {
            var rnd = new Random();
            Balance = rnd.Next(1000, 2000);
        }

        public async Task<long> Credit(string playerId, long amount)
        {
            Balance = Balance + amount;
            return await Task.FromResult(Balance);
        }

        public async Task<long> Debit(string playerId, long amount)
        {
            Balance = Balance - amount;
            return await Task.FromResult(Balance);

        }

        public async Task<long> GetBalance(string playerId)
        {
            return await Task.FromResult(Balance);
        }
    }
}
