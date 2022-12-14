
namespace Weknow.CypherBuilder
{

    /// <summary>
    /// Cypher Clause indicator
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CypherClauseAttribute : Attribute
    {
    }
}
