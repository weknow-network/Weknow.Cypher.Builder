using System.Linq.Expressions;

using Weknow.CypherBuilder.Declarations;

using static Weknow.CypherBuilder.CypherDelegates;

namespace Weknow.CypherBuilder;

/// <summary>
/// Cypher Extensions
/// </summary>
internal static partial class HelperExtensions
{

    #region IsAssignableTo

    /// <summary>
    /// Determines whether [is assignable to].
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t">The t.</param>
    /// <returns>
    ///   <c>true</c> if [is assignable to] [the specified t]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsAssignableTo<T>(this Type t) => t.IsAssignableTo(typeof(T));

    #endregion // IsAssignableTo

    #region IsOfType

    /// <summary>
    /// Check if expression is of type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expr"></param>
    /// <returns></returns>
    public static bool IsOfType<T>(this Expression expr)
    {
        Type t = typeof(T);
        bool result = expr.Type == t ||
                      expr is NewArrayExpression arrExp &&
                      arrExp.Expressions.FirstOrDefault()?.Type == t;
        return result;
    }

    #endregion // IsOfType

    #region IsArray

    /// <summary>
    /// Check if expression is array
    /// </summary>
    /// <param name="expr"></param>
    /// <returns></returns>
    public static bool IsArray(this Expression expr)
    {
        bool result = expr is NewArrayExpression &&
                                            expr.NodeType == ExpressionType.NewArrayInit;
        return result;
    }

    /// <summary>
    /// Check if expression is array
    /// </summary>
    /// <param name="expr">The expression.</param>
    /// <param name="length">The array length.</param>
    /// <returns>
    ///   <c>true</c> if the specified array expression is array; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsArray(this Expression expr, out int length)
    {
        bool isArray = false;
        length = 0;
        if (expr is NewArrayExpression naExp)
        {
            isArray = true;
            length = naExp.Expressions.Count;
        }
        bool result = isArray &&
                      expr.NodeType == ExpressionType.NewArrayInit;
        return result;
    }

    #endregion // IsArray
}
