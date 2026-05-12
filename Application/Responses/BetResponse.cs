using Domain;
using System.Text.Json.Serialization;

namespace Application
{
    public class BetResponse
    {
        //JsonConverter(typeof(JsonStringEnumConverter))]
        public SlotDefinition.Symbol[][] Symbols { get; set; }
        public Prize[] Prizes { get; set; }
        public int TotalPayout { get; set; }
    }
}
