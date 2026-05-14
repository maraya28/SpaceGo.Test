namespace Domain
{
    /// <summary>
    /// Represents a fixed position on the slot machine grid,
    /// defined by reel index and row index.
    /// </summary>
    /// <param name="Reel">Index of the reel (column)</param>
    /// <param name="Row">Index of the row (horizontal position)</param>
    public record struct GridPosition(int Reel, int Row);
}
