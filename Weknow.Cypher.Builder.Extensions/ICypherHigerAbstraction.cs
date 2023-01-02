using Weknow.CypherBuilder.Declarations;

using static Weknow.CypherBuilder.CypherDelegates;

namespace Weknow.CypherBuilder;

/// <summary>
/// Cypher Extensions
/// </summary>
public interface ICypherHigerAbstraction
{
    public static ICypherProc Proc() => throw new NotImplementedException();

 //   #region _If

 //   /// <summary>
 //   /// Conditional query.
 //   /// </summary>
 //   /// <param name="condition">The condition.</param>
 //   /// <param name="action">The action to execute when condition is true.</param>
 //   /// <returns></returns>
 //   [Cypher("&FOREACH ($auto-var$ IN CASE WHEN $0 THEN [1] ELSE [] END |\r\n\t$1)")]
	//[CypherClause]
 //   public static Fluent _If(
 //       bool condition,
 //       Fluent action) => throw new NotImplementedException();

	//#endregion // _If
}
