
using Application.Contracts;
using Domain;
using Infrastructure.Contracts;
using static Domain.SlotDefinition;

namespace Application.Implementations
{
    public class SlotService(IWallet walletService, ISlotStore slotStore) : ISlotService
    {
        public async Task<BetResponse> Spin(string playerId, int bet)
        {
            var balance = await walletService.GetBalance(playerId);

            ValidateBet(bet);

            ValidateBalance(bet, balance);
        
            var debit = await walletService.Debit(playerId, bet);

            var strips = new List<Symbol[]>() { Reel0Strip, Reel1Strip, Reel2Strip, Reel3Strip, Reel4Strip };
            List<(Symbol[] symbols, int lastStop)> reelsLastStop = [];

            foreach (var strip in strips)
            {
                var (reel, lastStop) = GetReelAndLastStop(strip);
                reelsLastStop.Add((reel, lastStop));
            }

            Symbol[][] grid = reelsLastStop.Select(_ => _.symbols).ToArray();

            var slot = await slotStore.Get();
            slot.LastStop = reelsLastStop.Select(_ => _.lastStop).ToArray();

            var (totalPayout, prizes) = Calculate(grid, bet);

            await walletService.Credit(playerId, totalPayout);

            await slotStore.Set(slot);

            var response = new BetResponse()
            {
                Symbols = grid,
                Prizes = prizes.ToArray(),
                TotalPayout = totalPayout,
            };
            return await Task.FromResult(response);
        }

        public static (int total, List<Prize> prizes) Calculate(Symbol[][] grid, int bet)
        {
            int total = 0;
            var prizes = new List<Prize>();

            return (total, prizes);
        }

        /// <summary>
        /// Return Reel and LastStop from Strip
        /// </summary>
        private static (Symbol[], int) GetReelAndLastStop(Symbol[] strip)
        {
            var length = strip.Length;
            var startIndex = new Random().Next(0, length);

            // Pick next 4 Symbols to display
            Symbol[] symbols = new Symbol[4];

            for (int i = 0; i < 4; i++)
            {
                var index = (startIndex + i) % strip.Length;
                var symbol = strip[index];
                symbols[i] = symbol;
            }

            return (symbols, startIndex);
        }

        private static void ValidateBet(int bet)
        {
            if (!AvailableBets.Contains(bet))
            {
                throw new ApplicationException("The selected bet is not avaibled. The available bets are: 1, 2, 5, 10, 15, 20, 50.");
            }
        }

        private static void ValidateBalance(int bet, long balance)
        {
            if (bet > balance)
            {
                throw new ApplicationException("You do not have sufficient funds to place this bet.");
            }
        }
    }
}
