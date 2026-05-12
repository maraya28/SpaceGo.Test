using Domain;

namespace Application
{
    /// <summary>
    /// Abstraction for reading and updating the persistent slot state.
    /// </summary>
    public interface ISlotStore
    {
        /// <summary>
        /// Retrieves the current slot state.
        /// </summary>
        Task<Slot> Get();

        /// <summary>
        /// Persists an updated slot state.
        /// </summary>
        Task Set(Slot slot);
    }
}
