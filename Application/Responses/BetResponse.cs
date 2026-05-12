using Domain;

namespace Application
{
    public class BetResponse
    {
        public SlotDefinition.Symbol[][] Symbols { get; set; }
        public Prize[] Prizes { get; set; }
        public int TotalPayout { get; set; }
    }
}
