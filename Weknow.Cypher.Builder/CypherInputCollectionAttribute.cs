namespace Weknow.CypherBuilder
{
    /// <summary>
    /// decorate object collection which are part of the Cypher Builder API,
    /// in order to avoid handling it as collection in the cypher output.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class CypherInputCollectionAttribute : Attribute
    {

    }
}
