
using Application.Contracts;
using static Domain.SlotDefinition;

namespace Application.Implementations
{
    public class SlotService(IWallet walletService) : ISlotService
    {
        public async Task<BetResponse> Spin(string playerId, int bet)
        {
            var balance = await walletService.GetBalance(playerId);

            ValidateBalance(bet, balance);

            var c = await walletService.Debit(playerId, bet);

            var strip1 = Spin(Reel0Strip);
            var strip2 = Spin(Reel1Strip);
            var strip3 = Spin(Reel2Strip);
            var strip4 = Spin(Reel3Strip);

            var response = new BetResponse()
            {
                Symbols = new[] { strip1, strip1, strip3, strip4 },
            };
            return await Task.FromResult(response);
        }



        private static Symbol[] Spin(Symbol[] reel)
        {
            var length = reel.Length;
            var startIndex = new Random().Next(0, length);

            // Pick next 4 Symbols to Display
            Symbol[] symbols = new Symbol[4];

            for (int i = 0; i < 4; i++)
            {
                var index = (startIndex + i) % reel.Length;
                var symbol = reel[index];
                symbols[i] = symbol;
            }

            return symbols;
        }

        private void ValidateBalance(int bet, long balance)
        {
            if (bet > balance)
            {
                throw new ApplicationException("You do not have sufficient funds to place this bet.");
            }
        }
    }
}
