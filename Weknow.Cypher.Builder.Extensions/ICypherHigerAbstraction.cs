using Weknow.Cypher.Builder.Fluent;
using Weknow.CypherBuilder.Declarations;

using static Weknow.CypherBuilder.CypherDelegates;

namespace Weknow.CypherBuilder;

/// <summary>
/// Cypher Extensions
/// </summary>
public interface ICypherHigerAbstraction
{
    public static ICypherProc Proc() => throw new NotImplementedException();
}
