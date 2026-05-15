
using Application.Contracts;
using Domain;
using Infrastructure.Contracts;
using Microsoft.Extensions.Logging;
using static Domain.SlotDefinition;

namespace Application.Implementations
{
    public class SlotService(IWallet walletService, ISlotStore slotStore, ILogger<SlotService> logger) : ISlotService
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

            var (prizes, totalPayout) = GetPrizesAndPayout(grid, bet);

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

        /// <summary>
        /// Return Prizes and Payout
        /// </summary>
        public (List<Prize> prizes, int total) GetPrizesAndPayout(Symbol[][] grid, int bet)
        {
            int totalPayout = 0;
            var prizes = new List<Prize>();

            var paylineIndex = 0;
            foreach (var line in Lines)
            {
                var payline = GetPayline(line, grid, paylineIndex);
                
                Symbol firstSymbolToMatch = payline[0];
                logger.LogInformation("Checking first Symbol: {symbol} from Payline {paylineIndex} to match..", firstSymbolToMatch, paylineIndex);
          
                paylineIndex++;
            }

            return (prizes, totalPayout);
        }

        /// <summary>
        /// Returns symbols according to the defined paylines
        /// </summary>
        private Symbol[] GetPayline(Line line, Symbol[][] grid, int payline) 
        {
            var symbols = line.GridPositions
                          .Select(p => grid[p.Reel][p.Row])
                          .ToArray();

            logger.LogInformation("[PayLine {payline}-9]: {symbols[0]}, {symbols[1]}, {symbols[2]}, {symbols[3]}, {symbols[4]}", payline, symbols[0], symbols[1], symbols[2], symbols[3], symbols[4]);
            return symbols;

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
