using Weknow.Cypher.Builder.Fluent;
using Weknow.CypherBuilder.Declarations;

using static Weknow.CypherBuilder.CypherDelegates;

namespace Weknow.CypherBuilder;

public interface ICypherProc
{ 
    /// <summary>
    /// Conditional query.
    /// </summary>
    /// <param name="condition">The condition.</param>
    /// <param name="action">The action to execute when condition is true.</param>
    /// <returns></returns>
    [Cypher("&FOREACH ($auto-var$ IN CASE WHEN $0 THEN [1] ELSE [] END |\r\n\t$1)")]
    //[Cypher("CALL apoc.when($0,\r\n\t'$1')", Flavor = CypherFlavor.Neo4j5)]
	[CypherClause]
    public ICypherStatement If(
        bool condition,
        ICypherStatement action) => throw new NotImplementedException();
}
