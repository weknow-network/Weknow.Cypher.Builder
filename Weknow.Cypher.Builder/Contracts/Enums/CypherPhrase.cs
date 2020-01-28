namespace Weknow
{
    /// <summary>
    /// Cypher Phrase
    /// </summary>
    public enum CypherPhrase
    {
        /// <summary>
        /// The none
        /// </summary>
        None,
        /// <summary>
        /// The node / relation pattern
        /// </summary>
        Pattern,
        /// <summary>
        /// The property
        /// </summary>
        Property,
        /// <summary>
        /// The property scope
        /// </summary>
        PropertyScope,
        /// <summary>
        /// Arbitrary statement (non-structured)
        /// </summary>
        Dynamic,
        /// <summary>
        /// The match
        /// </summary>
        Match,
        /// <summary>
        /// The create
        /// </summary>
        Create,
        /// <summary>
        /// The merge
        /// </summary>
        Merge,
        /// <summary>
        /// The delete
        /// </summary>
        Delete,
        /// <summary>
        /// The where
        /// </summary>
        Where,
        /// <summary>
        /// The optional match
        /// </summary>
        OptionalMatch,
        /// <summary>
        /// The remove
        /// </summary>
        Remove,
        /// <summary>
        /// The detach delete
        /// </summary>
        DetachDelete,
        /// <summary>
        /// The unwind
        /// </summary>
        Unwind,
        /// <summary>
        /// The with
        /// </summary>
        With,
        /// <summary>
        /// The return
        /// </summary>
        Return,
        /// <summary>
        /// The Project
        /// </summary>
        Project,
        /// <summary>
        /// The union
        /// </summary>
        Union,
        /// <summary>
        /// The union all
        /// </summary>
        UnionAll,
        /// <summary>
        /// The call
        /// </summary>
        Call,
        /// <summary>
        /// For each
        /// </summary>
        ForEach,
        /// <summary>
        /// The on create
        /// </summary>
        OnCreate,
        /// <summary>
        /// The on match
        /// </summary>
        OnMatch,
        /// <summary>
        /// The set
        /// </summary>
        Set,
        /// <summary>
        /// The and
        /// </summary>
        And,
        /// <summary>
        /// The or
        /// </summary>
        Or,
        /// <summary>
        /// The return distinct
        /// </summary>
        ReturnDistinct,
        /// <summary>
        /// The order by
        /// </summary>
        OrderBy,
        /// <summary>
        /// The order by desc
        /// </summary>
        OrderByDesc,
        /// <summary>
        /// The skip
        /// </summary>
        Skip,
        /// <summary>
        /// The limit
        /// </summary>
        Limit,
        /// <summary>
        /// The count
        /// </summary>
        Count
    }
}
