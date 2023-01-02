using Weknow.CypherBuilder.Declarations;

using static Weknow.CypherBuilder.CypherDelegates;

namespace Weknow.CypherBuilder;


/// <summary>
/// Cypher Extensions
/// </summary>
public static  class CypherHigerAbstractionExtensions
{
    [Cypher("$0\r\n")]
    public static ICypherProc Proc(this Fluent prev) => throw new NotImplementedException();

 //   #region _If

 //   /// <summary>
 //   /// Conditional query.
 //   /// </summary>
 //   /// <param name="prev">The previous.</param>
 //   /// <param name="condition">The condition.</param>
 //   /// <param name="action">The action to execute when condition is true.</param>
 //   /// <returns></returns>
 //   [Cypher("$0\r\n&FOREACH ($auto-var$ IN CASE WHEN $1 THEN [1] ELSE [] END |\r\n\t$2)")]
	//[CypherClause]
	//public static Fluent _If(
	//	this Fluent prev,
	//	bool condition,
	//	Fluent action) => throw new NotImplementedException();

	//#endregion // _If
}
