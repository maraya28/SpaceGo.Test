namespace Application.Contracts
{
    public interface ISlotService
    {
        public Task<BetResponse> Spin(string playerId, int bet);
    }
}
