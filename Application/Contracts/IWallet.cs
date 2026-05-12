namespace Application
{
    /// <summary>
    /// Abstraction of an external service to manage a user's wallet.
    /// </summary>
    public interface IWallet
    {
        /// <summary>
        /// Gets the current wallet balance for a specific player.
        /// </summary>
        public Task<long> GetBalance(string playerId);

        /// <summary>
        /// Credits the player's wallet with a given amount.
        /// </summary>
        public Task<long> Credit(string playerId, long amount);

        /// <summary>
        /// Debits the player's wallet by a given amount.
        /// </summary>
        public Task<long> Debit(string playerId, long amount);
    }
}
