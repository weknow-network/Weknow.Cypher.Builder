#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.  


namespace Weknow.CypherBuilder.Declarations;

public class RelationsVariableDeclaration<T> : PathRelatedVariableDeclaration<T>
{
    /// <summary>
    /// Default (and only) way to get cypher parameter.
    /// It use under expression and don't need a real implementation;
    /// </summary>
    internal static readonly new RelationsVariableDeclaration<T> Default = new RelationsVariableDeclaration<T>();
}
