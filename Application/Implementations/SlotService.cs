
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

            var c = await walletService.Debit(playerId, bet);

            var (strip1, lastStop1) = SpinAndLastStop(Reel0Strip);
            var (strip2, lastStop2) = SpinAndLastStop(Reel1Strip);
            var (strip3, lastStop3) = SpinAndLastStop(Reel2Strip);
            var (strip4, lastStop4) = SpinAndLastStop(Reel3Strip);


            var slot = new Slot() { PlayerId = playerId, LastStop = [lastStop1, lastStop2, lastStop3, lastStop4] };

            var response = new BetResponse()
            {
                Symbols = [strip1, strip1, strip3, strip4],
            };
            return await Task.FromResult(response);
        }





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
