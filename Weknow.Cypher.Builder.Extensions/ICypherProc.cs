﻿using static Weknow.CypherBuilder.CypherDelegates;

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
    public Fluent If(
        bool condition,
        Fluent action) => throw new NotImplementedException();
}