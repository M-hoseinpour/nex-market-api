namespace market.Models.Enum;

/// <summary>
///     The state in which an entity is being tracked by a context.
/// </summary>
public enum UpdateState
{
    /// <summary>
    ///     The entity is being tracked by the context and exists in the database. It has been marked
    ///     for deletion from the database.
    /// </summary>
    Deleted = 1,

    /// <summary>
    ///     The entity is being tracked by the context but does not yet exist in the database.
    /// </summary>
    Added = 2
}