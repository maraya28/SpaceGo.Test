using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{

    /// <summary>
    /// Represents a slot machine for a specific player.
    /// </summary>
    public class Slot
    {
        /// <summary>
        /// The unique identifier of the player associated with this slot.
        /// </summary>
        public required string PlayerId { get; init; }

        /// <summary>
        /// Stores the last stop index for each reel.
        /// </summary>
        public required int[] LastStop { get; set; }

        public (SlotDefinition.Symbol[][], Prize[]) Spin(int bet)
        {
            throw new NotImplementedException();
        }
    }
}
