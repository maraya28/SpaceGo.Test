namespace Domain
{
    /// <summary>
    /// Represents a payline definition with an identifier and
    /// the ordered grid positions that form the line.
    /// </summary>
    /// <param name="Id">Unique identifier of the line</param>
    /// <param name="GridPositions">Ordered positions across the reels</param>
    public sealed record Line(int Id, GridPosition[] GridPositions);
}