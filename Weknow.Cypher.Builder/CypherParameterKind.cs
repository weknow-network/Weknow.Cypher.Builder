namespace Weknow.CypherBuilder;

public enum CypherParameterKind
{
    /// <summary>
    /// Normal parameter
    /// </summary>
    Normal,
    /// <summary>
    /// The parameter will be embed directly into the cypher string
    /// </summary>
    Embed
}
