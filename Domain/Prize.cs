namespace Domain
{
    /// <summary>
    /// Represents a payout result for a specific line, including
    /// symbol count and payout amount.
    /// </summary>
    /// <param name="Line">The winning line</param>
    /// <param name="N">Number of consecutive matching symbols</param>
    /// <param name="Payout">Total payout for this result</param>
    public record Prize(Line Line, int N, int Payout);
}
