
using Application.Contracts;
using Domain;
using static Domain.SlotDefinition;

namespace Application.Implementations
{
    public class SlotService(IWallet walletService) : ISlotService
    {
        public async Task<BetResponse> Spin(string playerId, int bet)
        {
            var balance = await walletService.GetBalance(playerId);

            ValidateBalance(bet, balance);

            ValidateBet(bet);

            var debit = await walletService.Debit(playerId, bet);

            var (reel1, lastStop1) = SpinAndLastStop(Reel0Strip);
            var (reel2, lastStop2) = SpinAndLastStop(Reel1Strip);
            var (reel3, lastStop3) = SpinAndLastStop(Reel2Strip);
            var (reel4, lastStop4) = SpinAndLastStop(Reel3Strip);
            var (reel5, lastStop5) = SpinAndLastStop(Reel4Strip);

            Symbol[][] grid = [reel1, reel2, reel3, reel4, reel5];

            var slot = new Slot() { PlayerId = playerId, LastStop = [lastStop1, lastStop2, lastStop3, lastStop4, lastStop5] };

            var (totalPayout, prizes) = Calculate(grid, bet);

            await walletService.Credit(playerId, totalPayout);

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
        /// Spin and return the last Stop
        /// </summary>
        private static (Symbol[], int) SpinAndLastStop(Symbol[] reel)
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

            return (symbols, startIndex);
        }

        private void ValidateBalance(int bet, long balance)
        {
            if (bet > balance)
            {
                throw new ApplicationException("You do not have sufficient funds to place this bet.");
            }
        }

        private void ValidateBet(int bet)
        {
            if (!AvailableBets.Contains(bet))
            {
                throw new ApplicationException("The selected bet is not avaibled.");
            }
        }

        // TODO REFACTOR LIST of SPINS
    }
}
