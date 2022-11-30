using System.Linq.Expressions;

using Weknow.CypherBuilder.Declarations;

using static Weknow.CypherBuilder.CypherDelegates;

// https://neo4j.com/docs/cypher-refcard/current/

// TODO: [bnaya 2022-10-20] source code generator of the cypher

namespace Weknow.CypherBuilder;

/// <summary>
/// Entry point for constructing root level Cypher.
/// For fluent cypher check <see cref="CypherExtensions" />
/// </summary>
public partial interface ICypher
{
    #region HandleConfigInjection

    /// <summary>
    /// Handles the configuration injection.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="cfg">The CFG.</param>
    private static void HandleConfigInjection(
        Action<CypherConfig>? configuration, CypherConfig cfg)
    {
        CypherConfig.Scope.Value?.Invoke(cfg);
        configuration?.Invoke(cfg);
    }

    #endregion // HandleConfigInjection

    #region Init

    /// <summary>
    /// Initializes a builder.
    /// </summary>
    /// <param name="cfg">The CFG.</param>
    /// <param name="expression">The expression.</param>
    /// <returns></returns>
    internal static CypherCommand Init(CypherConfig cfg, Expression expression)
    {
        using var visitor = new CypherVisitor(cfg);

        visitor.Visit(expression);
        string cypher = visitor.Query.ToString().Replace("\r\n", Environment.NewLine);
        CypherParameters parameters = visitor.Parameters;

        return new CypherCommand(
                        cypher,
                        parameters);
    }

    #endregion // Init

    #region dash '_'

    /// <summary>
    /// Build cypher expression
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static CypherCommand _(
                        Expression<NoVariable> expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        HandleConfigInjection(configuration, cfg);
        CypherCommand result = Init(cfg, expression);
        return result;
    }


    /// <summary>
    /// Build cypher expression
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static CypherCommand _(
                        Expression<Fluent> expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        HandleConfigInjection(configuration, cfg);
        CypherCommand result = Init(cfg, expression);
        return result;
    }


    /// <summary>
    /// Build cypher expression
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static CypherCommand _(
                        Expression expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        HandleConfigInjection(configuration, cfg);
        CypherCommand result = Init(cfg, expression);
        return result;
    }

    #endregion // dash '_'

    #region IPattern N (Node)

    /// <summary>
    /// Specified node with label.
    /// </summary>
    /// <returns></returns>
    [Cypher("()")]
    public static INode N() => throw new NotImplementedException();
    /// <summary>
    /// Specified node with label.
    /// </summary>
    /// <param name="label">The label.</param>
    /// <returns></returns>
    [Cypher("(:$0)")]
    public static INode N(ILabel label) => throw new NotImplementedException();
    /// <summary>
    /// Specified node with variable.
    /// </summary>
    /// <param name="var">The variable.</param>
    /// <returns></returns>
    [Cypher("($0)")]
    public static INode N(VariableDeclaration var) => throw new NotImplementedException();
    /// <summary>
    /// Specified node with variable and label.
    /// </summary>
    /// <param name="var">The variable.</param>
    /// <param name="label">The label.</param>
    /// <returns></returns>
    [Cypher("($0$1)")]
    public static INode N(VariableDeclaration var, ILabel label) => throw new NotImplementedException();
    /// <summary>
    /// Specified node with variable, label and properties.
    /// </summary>
    /// <param name="var">The variable.</param>
    /// <param name="label">The label.</param>
    /// <param name="properties">The properties.</param>
    /// <returns></returns>
    [Cypher("($0$1 $2)")]
    public static INode N(VariableDeclaration var, ILabel label, object properties) => throw new NotImplementedException();
    [Cypher("($0 $1)")]
    public static INode N(VariableDeclaration var, object properties) => throw new NotImplementedException();
    [Cypher("($0 $1)")]
    public static INode N(ILabel label, object properties) => throw new NotImplementedException();

    #endregion // IPattern N (Node)

    #region IRelation R (Relation)

    /// <summary>
    /// Gets the Cypher relation.
    /// </summary>
    public static IRelation R => throw new NotImplementedException();

    #endregion // IRelation R (Relation)

    #region Rgx

    /// <summary>
    /// Use regular expression comparison under WHERE clause.
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    /// <example>
    /// WHERE n.name =~ 'Tim.*'
    /// </example>
    [Cypher("&$0")]
    public static bool Rgx(bool condition) => throw new NotImplementedException();
    /// <summary>
    /// Use regular expression comparison under WHERE clause.
    /// </summary>
    /// <param name="properties">The properties.</param>
    /// <returns></returns>
    /// <example>
    /// WHERE n.name =~ 'Tim.*'
    /// </example>
    [Cypher("&$0")]
    public static bool Rgx(object properties) => throw new NotImplementedException();

    #endregion // Rgx

    #region Profile

    /// <summary>
    /// Profile execution.
    /// </summary>
    /// <returns></returns>
    [Cypher("PROFILE")]
    public static Fluent Profile() => throw new NotImplementedException();

    #endregion // Profile

    #region Return

    /// <summary>
    /// RETURN phrase.
    /// </summary>
    /// <param name="var">The first variable.</param>
    /// <param name="vars">Rest of the variables.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// RETURN n
    /// </example>
    [Cypher("RETURN $0$1")]
    public static Fluent Return(ParamsFirst<object?> var, params object?[] vars) => throw new NotImplementedException();

    #endregion // Return

    #region Match

    /// <summary>
    /// Matches phrase.
    /// </summary>
    /// <param name="var">The variable.</param>
    /// <param name="n">The n.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// MATCH p = (n:Person)-[:KNOWS]-&gt;(m:Person)
    /// </example>
    [Cypher("MATCH $0 = $1")]
    public static Fluent Match(VariableDeclaration var, INode n) => throw new NotImplementedException();

    /// <summary>
    /// Matches phrase.
    /// </summary>
    /// <param name="n">The n.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// MATCH (n:Person)-[:KNOWS]-&gt;(m:Person)
    /// </example>
    [Cypher("MATCH $0")]
    public static Fluent Match(INode n) => throw new NotImplementedException();

    /// <summary>
    /// Matches phrase.
    /// </summary>
    /// <param name="n">The n.</param>
    /// <param name="rest">The rest.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// MATCH (n:Person)-[:KNOWS]-&gt;(m:Person)
    /// </example>
    [Cypher("MATCH $0, $1")]
    public static Fluent Match(INode n, params INode[] rest) => throw new NotImplementedException();

    /// <summary>
    /// Matches phrase.
    /// </summary>
    /// <param name="n">The n.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// MATCH (n:Person)-[:KNOWS]-&gt;(m:Person)
    /// </example>
    [Cypher("OPTIONAL MATCH $0")]
    public static Fluent OptionalMatch(INode n) => throw new NotImplementedException();

    /// <summary>
    /// Matches phrase.
    /// </summary>
    /// <param name="n">The n.</param>
    /// <param name="rest">The rest.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// MATCH (n:Person)-[:KNOWS]-&gt;(m:Person)
    /// </example>
    [Cypher("OPTIONAL MATCH $0,$1")]
    public static Fluent OptionalMatch(INode n, params INode[] rest) => throw new NotImplementedException();

    #endregion // Match

    #region Create

    /// <summary>
    /// Create phrase.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <returns></returns>
    /// <example>
    /// CREATE (n {name: $value})
    /// </example>
    [Cypher("CREATE $0")]
    public static Fluent Create(IPattern p) => throw new NotImplementedException();

    #endregion // Create

    #region Merge

    /// <summary>
    /// MERGE phrase.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <returns></returns>
    /// <example>
    /// MERGE (n:Person {name: $value})
    /// </example>
    [Cypher("MERGE $0")]
    public static Fluent Merge(IPattern p) => throw new NotImplementedException();

    #endregion // Merge

    #region Set

    /// <summary>
    /// SET label.
    /// </summary>
    /// <param name="var">The variable.</param>
    /// <param name="label">The label.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// SET n:Person:Manager
    /// </example>
    [Cypher("&SET $0$1")]
    public static Fluent Set(VariableDeclaration var, params ILabel[] label)
        => throw new NotImplementedException();

    /// <summary>
    /// SET phrase.
    /// </summary>
    /// <param name="var">The variable.</param>
    /// <param name="assignment">The complex.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// .Set(n, map)
    /// result in:
    /// SET n = map
    /// </example>
    [Cypher("&SET $0 = $1")]
    public static Fluent Set(VariableDeclaration var, VariableDeclaration assignment)
        => throw new NotImplementedException();

    /// <summary>
    /// SET phrase.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var"></param>
    /// <param name="assignment"></param>
    /// <returns></returns>
    /// <example>
    /// .Set(n, map)
    /// result in:
    /// SET n = $map
    /// </example>
    [Cypher("&SET $0 = $1")]
    public static Fluent Set(VariableDeclaration var, ParameterDeclaration assignment)
        => throw new NotImplementedException();

    /// <summary>
    /// SET  phrase.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var">The variable.</param>
    /// <param name="assignment">The complex.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    /// <example>
    /// .Set(n, new {prm._.Name, var._.Code})
    /// result in:
    /// SET n.Name = $Name, n.Code = prm.Code
    /// </example>
    [Cypher("&SET +00$1")]
    public static Fluent Set(VariableDeclaration var, object assignment)
        => throw new NotImplementedException();

    #endregion // Set

    #region Unwind

    /// <summary>
    /// UNWIND phrase.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="item">The item.</param>
    /// <param name="p">The p.</param>
    /// <returns></returns>
    /// <example>
    /// UNWIND $names AS name
    /// MATCH(n { name: name})
    /// RETURN avg(n.age)
    /// </example>
    [Cypher("&UNWIND \\$$[]0 AS $1\r\n$2")]
    public static Fluent Unwind<T>(IEnumerable<T> items, VariableDeclaration item, Fluent p) => throw new NotImplementedException();


    /// <summary>
    /// UNWIND phrase.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="item">The item.</param>
    /// <param name="p">The p.</param>
    /// <returns></returns>
    /// <example>
    /// UNWIND $names AS name
    /// MATCH(n { name: name})
    /// RETURN avg(n.age)
    /// </example>
    [Cypher("&UNWIND \\$$0 AS $1\r\n$2")]
    public static Fluent Unwind(VariableDeclaration items, VariableDeclaration item, Fluent p) => throw new NotImplementedException();

    /// <summary>
    /// Unwinds the specified items.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="item">The item.</param>
    /// <param name="p">The p.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [Cypher("&UNWIND $0 AS $1\r\n$2")]
    public static Fluent Unwind(ParameterDeclaration items, VariableDeclaration item, Fluent p) => throw new NotImplementedException();

    #endregion // Unwind

    #region Foreach

    /// <summary>
    /// FOREACH phrase.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="item">The item.</param>
    /// <param name="p">The p.</param>
    /// <returns></returns>
    /// <example>
    /// FOREACH $names AS name
    /// MATCH(n { name: name})
    /// RETURN avg(n.age)
    /// </example>
    [Cypher("&FOREACH ($0 IN \\$$[]1 |\r\n\t$2)")]
    public static Fluent Foreach<T>(VariableDeclaration item, IEnumerable<T> items, Fluent p) => throw new NotImplementedException();

    /// <summary>
    /// FOREACH phrase.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="item">The item.</param>
    /// <param name="p">The p.</param>
    /// <returns></returns>
    /// <example>
    /// FOREACH $names AS name
    /// MATCH(n { name: name})
    /// RETURN avg(n.age)
    /// </example>
    [Cypher("&FOREACH ($0 IN $1 |\r\n\t$2)")]
    public static Fluent Foreach(VariableDeclaration item, VariableDeclaration items, Fluent p) => throw new NotImplementedException();

    /// <summary>
    /// Foreachs the specified items.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="item">The item.</param>
    /// <param name="p">The p.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [Cypher("&FOREACH ($0 IN $1 |\r\n\t$2)")]
    public static Fluent Foreach(VariableDeclaration item, ParameterDeclaration items, Fluent p) => throw new NotImplementedException();

    #endregion // Foreach

    #region Exists

    /// <summary>
    /// EXISTS phrase.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <returns></returns>
    /// <example>
    /// exists(n.property)
    /// </example>
    [Cypher("EXISTS { $0 }")]
    public static bool Exists(Fluent p) => throw new NotImplementedException();
    /// <summary>
    /// EXISTS phrase.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <returns></returns>
    /// <example>
    /// exists(n.property)
    /// </example>
    [Cypher("EXISTS { $0 }")]
    public static bool Exists(NoVariable p) => throw new NotImplementedException();

    #endregion // Exists

    #region Reuse

    /// <summary>
    /// Reuses the specified expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static IRelationNode Reuse(
                        Expression<Func<IRelationNode>> expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        HandleConfigInjection(configuration, cfg);
        return new RelationNodePattern(expression.Body, cfg);
    }

    /// <summary>
    /// Reuses the specified expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static INodeRelation Reuse(
                        Expression<Func<INodeRelation>> expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        HandleConfigInjection(configuration, cfg);
        return new NodeRelationPattern(expression.Body, cfg);
    }

    /// <summary>
    /// Use for encapsulation of reusable expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static INode Reuse(
                        Expression<Func<INode>> expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        CypherConfig.Scope.Value?.Invoke(cfg);
        configuration?.Invoke(cfg);
        return new NodePattern(expression.Body, cfg);
    }

    /// <summary>
    /// Use for encapsulation of reusable expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static IRelation Reuse(
                        Expression<Func<IRelation>> expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        HandleConfigInjection(configuration, cfg);
        return new RelationPattern(expression.Body, cfg);
    }

    /// <summary>
    /// Use for encapsulation of reusable expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static INodeRelation Reuse(
                        Expression<Func<Func<VariableDeclaration, INodeRelation>>> expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        CypherConfig.Scope.Value?.Invoke(cfg);
        configuration?.Invoke(cfg);
        return new NodeRelationPattern(expression.Body, cfg);
    }

    /// <summary>
    /// Use for encapsulation of reusable expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static INode Reuse(
                        Expression<Func<Func<VariableDeclaration, INode>>> expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        HandleConfigInjection(configuration, cfg);
        return new NodePattern(expression.Body, cfg);
    }

    /// <summary>
    /// Use for encapsulation of reusable expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static IRelation Reuse(
                        Expression<Func<Func<VariableDeclaration, IRelation>>> expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        CypherConfig.Scope.Value?.Invoke(cfg);
        configuration?.Invoke(cfg);
        return new RelationPattern(expression.Body, cfg);
    }

    /// <summary>
    /// Use for encapsulation of reusable expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static INode Reuse(
                        Expression<Func<Func<VariableDeclaration, Func<VariableDeclaration, INode>>>> expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        HandleConfigInjection(configuration, cfg);
        return new NodePattern(expression.Body, cfg);
    }

    /// <summary>
    /// Use for encapsulation of reusable expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static IRelation Reuse(
                        Expression<Func<Func<VariableDeclaration, Func<VariableDeclaration, IRelation>>>> expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        CypherConfig.Scope.Value?.Invoke(cfg);
        configuration?.Invoke(cfg);
        return new RelationPattern(expression.Body, cfg);
    }

    /// <summary>
    /// Use for encapsulation of reusable expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static INode Reuse(
                        Expression<Func<Func<VariableDeclaration, Func<VariableDeclaration, Func<VariableDeclaration, INode>>>>> expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        HandleConfigInjection(configuration, cfg);
        return new NodePattern(expression.Body, cfg);
    }

    /// <summary>
    /// Use for encapsulation of reusable expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static IRelation Reuse(
                        Expression<Func<Func<VariableDeclaration, Func<VariableDeclaration, Func<VariableDeclaration, IRelation>>>>> expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        CypherConfig.Scope.Value?.Invoke(cfg);
        configuration?.Invoke(cfg);
        return new RelationPattern(expression.Body, cfg);
    }

    /// <summary>
    /// Use for encapsulation of reusable expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static INode Reuse(
                        Expression<Func<Func<VariableDeclaration, Func<VariableDeclaration, Func<VariableDeclaration, Func<VariableDeclaration, INode>>>>>> expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        HandleConfigInjection(configuration, cfg);
        return new NodePattern(expression.Body, cfg);
    }

    /// <summary>
    /// Use for encapsulation of reusable expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static IRelation Reuse(
                        Expression<Func<Func<VariableDeclaration, Func<VariableDeclaration, Func<VariableDeclaration, Func<VariableDeclaration, IRelation>>>>>> expression,
                        Action<CypherConfig>? configuration = null)
    {
        var cfg = new CypherConfig();
        HandleConfigInjection(configuration, cfg);
        return new RelationPattern(expression.Body, cfg);
    }

    #endregion // Reuse

    #region Timestamp / timestamp()

    /// <summary>
    /// Milliseconds since midnight, January 1, 1970 UTC.
    /// </summary>
    /// <returns></returns>
    /// <example>
    /// MATCH (n)
    /// RETURN timestamp()
    /// </example>
    [Cypher("timestamp()")]
    public static VariableDeclaration Timestamp() => throw new NotImplementedException();

    #endregion // Timestamp / timestamp()

    #region FromRawCypher

    /// <summary>
    /// Pure cypher injection.
    /// Should used for non-supported cypher extensions
    /// </summary>
    /// <param name="cypher">The cypher.</param>
    /// <returns></returns>
    [Obsolete("It's better to use the Cypher methods instead of clear text as log as it supported", false)]
    [Cypher("$0")]
    public static IRawCypher FromRawCypher(RawCypher cypher) => throw new NotImplementedException();

    #endregion // FromRawCypher

    #region RawCypher

    /// <summary>
    /// Pure cypher injection.
    /// Should used for non-supported cypher extensions
    /// </summary>
    /// <param name="cypher">The cypher.</param>
    /// <returns></returns>
    [Obsolete("It's better to use the Cypher methods instead of clear text as log as it supported", false)]
    [Cypher("$0")]
    public static Fluent RawCypher(RawCypher cypher) => throw new NotImplementedException();

    #endregion // RawCypher

    #region DropConstraint

    /// <summary>
    /// Drop a index phrase.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    [Cypher("DROP CONSTRAINT $0")]
    public static Expression DropConstraint(
        string name) => throw new NotImplementedException();

    #endregion // DropConstraint

    #region TryDropConstraint

    /// <summary>
    /// Drop a index phrase.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    [Cypher("DROP CONSTRAINT $0 IF EXISTS")]
    public static Fluent TryDropConstraint(
        string name) => throw new NotImplementedException();

    #endregion // TryDropConstraint

    #region CreateConstraint

    /// <summary>
    /// Create a index phrase.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="p">The p.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    [Cypher("CREATE CONSTRAINT $0\r\n\tFOR $1\r\n\tREQUIRE ($2$3)")]
    public static Fluent CreateConstraint(
        string name,
        IPattern p,
        ParamsFirst<object?> var, params object?[] vars) => throw new NotImplementedException();

    /// <summary>
    /// Create a index phrase.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="type">The type.</param>
    /// <param name="p">The p.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    [Cypher("CREATE CONSTRAINT $0\r\n\tFOR $2\r\n\tREQUIRE ($3$4) $1")]
    public static Fluent CreateConstraint(
        string name,
        ConstraintType type,
        IPattern p,
        ParamsFirst<object?> var, params object?[] vars) => throw new NotImplementedException();

    #endregion // CreateConstraint

    #region TryCreateConstraint

    /// <summary>
    /// Create a index phrase.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="p">The p.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    [Cypher("CREATE CONSTRAINT $0 IF NOT EXISTS\r\n\tFOR $1\r\n\tREQUIRE ($2$3)")]
    public static Fluent TryCreateConstraint(
        string name,
        IPattern p,
        ParamsFirst<object?> var, params object?[] vars) => throw new NotImplementedException();

    /// <summary>
    /// Create a index phrase.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="type">The type.</param>
    /// <param name="p">The p.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    [Cypher("CREATE CONSTRAINT $0 IF NOT EXISTS\r\n\tFOR $2\r\n\tREQUIRE ($3$4) $1")]
    public static Fluent TryCreateConstraint(
        string name,
        ConstraintType type,
        IPattern p,
        ParamsFirst<object?> var, params object?[] vars) => throw new NotImplementedException();

    #endregion // TryCreateConstraint

    #region DropIndex

    /// <summary>
    /// Drop a index phrase.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    [Cypher("DROP INDEX $0")]
    public static Fluent DropIndex(
        string name) => throw new NotImplementedException();

    #endregion // DropIndex

    #region TryDropIndex

    /// <summary>
    /// Drop a index phrase.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    [Cypher("DROP INDEX $0 IF EXISTS")]
    public static Fluent TryDropIndex(
        string name) => throw new NotImplementedException();

    #endregion // TryDropIndex

    #region Create..Index

    /// <summary>
    /// Create a index phrase.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="p">The p.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    [Cypher("CREATE INDEX $0\r\n\tFOR $1\r\n\tON ($2$3)")]
    public static Fluent CreateIndex(
        string name,
        IPattern p,
        ParamsFirst<object?> var, params object?[] vars) => throw new NotImplementedException();

    /// <summary>
    /// Create a TEXT index on nodes with label Person and property name. 
    /// The property value type should be a string for the TEXT index. 
    /// Other value types are ignored by the TEXT index.
    /// TEXT index is utilized if the predicate compares the property with a string. 
    /// Note that for example:
    /// toLower(n.name) = 'Example String' 
    /// does not use an index.
    /// TEXT index is utilized to check the IN list checks, 
    /// when all elements in the list are strings.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="p">The p.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// CREATE (n {name: $value})
    /// </example>
    [Cypher("CREATE TEXT INDEX $0\r\n\tFOR $1\r\n\tON ($2$3)")]
    public static Fluent CreateTextIndex(
        string name,
        IPattern p,
        ParamsFirst<object?> var, params object?[] vars) => throw new NotImplementedException();

    /// <summary>
    /// Create a full-text index on relationships with the name index_name and analyzer.
    /// Full-text indexes on relationships can only be used by from
    /// the procedure db.index.fulltext.queryRelationships.
    /// The other index settings will have their default values.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="p">The p.</param>
    /// <param name="analyzer">The analyzer.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    [Cypher("CREATE FULLTEXT INDEX $0\r\n\tFOR $1\r\n\tON EACH [$3$4]\r\n\tOPTIONS {\r\n\t\tindexConfig: {\r\n\t\t\t`fulltext.analyzer`: '$2'\r\n\t\t  }\r\n\t}")]
    public static Fluent CreateFullTextIndex(
        string name,
        IPattern p,
        FullTextAnalyzer analyzer,
        ParamsFirst<object?> var, params object?[] vars) => throw new NotImplementedException();

    ///// <summary>
    ///// Create a index phrase.
    ///// </summary>
    ///// <param name="name">The name.</param>
    ///// <param name="p">The p.</param>
    ///// <param name="var">The variable.</param>
    ///// <param name="vars">The vars.</param>
    ///// <returns></returns>
    ///// <exception cref="System.NotImplementedException"></exception>
    //[Cypher("CREATE LOOKUP INDEX $0\r\n\tFOR $1\r\n\tON ($2$3)")]
    //public static Fluent CreateLookupIndex(
    //    string name,
    //    IPattern p, 
    //    ParamsFirst<object?> var, params object?[] vars) => throw new NotImplementedException();

    #endregion // Create..Index

    #region TryCreate..Index

    /// <summary>
    /// Create a index phrase.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="p">The p.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    [Cypher("CREATE INDEX $0 IF NOT EXISTS\r\n\tFOR $1\r\n\tON ($2$3)")]
    public static Fluent TryCreateIndex(
        string name,
        IPattern p,
        ParamsFirst<object?> var, params object?[] vars) => throw new NotImplementedException();

    /// <summary>
    /// Create a index phrase.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="p">The p.</param>
    /// <param name="analyzer">The analyzer.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// CREATE (n {name: $value})
    /// </example>
    [Cypher("CREATE FULLTEXT INDEX $0 IF NOT EXISTS\r\n\tFOR $1\r\n\tON EACH [$3$4]\r\n\tOPTIONS {\r\n\t\tindexConfig: {\r\n\t\t\t`fulltext.analyzer`: '$2'\r\n\t\t  }\r\n\t}")]
    public static Fluent TryCreateFullTextIndex(
        string name,
        IPattern p,
        FullTextAnalyzer analyzer,
        ParamsFirst<object?> var, params object?[] vars) => throw new NotImplementedException();


    /// <summary>
    /// Create a TEXT index on nodes with label Person and property name.
    /// The property value type should be a string for the TEXT index.
    /// Other value types are ignored by the TEXT index.
    /// TEXT index is utilized if the predicate compares the property with a string.
    /// Note that for example:
    /// toLower(n.name) = 'Example String'
    /// does not use an index.
    /// TEXT index is utilized to check the IN list checks,
    /// when all elements in the list are strings.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="p">The p.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// CREATE (n {name: $value})
    /// </example>
    [Cypher("CREATE TEXT INDEX $0 IF NOT EXISTS\r\n\tFOR $1\r\n\tON ($2$3)")]
    public static Fluent TryCreateTextIndex(
        string name,
        IPattern p,
        ParamsFirst<object?> var, params object?[] vars) => throw new NotImplementedException();

    #endregion // TryCreate..Index

    #region nodes

    /// <summary>
    /// Returns a list containing all the nodes in a path.
    /// https://neo4j.com/docs/cypher-manual/5/functions/list/#functions-nodes
    /// </summary>
    /// <param name="prop">The property.</param>
    /// <returns></returns>
    /// <example>
    /// MATCH p = (a)-->(b)-->(c)
    /// WHERE a.name = 'Alice' AND c.name = 'Eskil'
    /// RETURN nodes(p)
    /// </example>
    [Cypher("nodes($0)")]
    public static VariableDeclaration Nodes(object prop) => throw new NotImplementedException();

    #endregion nodes

    #region relationships

    /// <summary>
    /// returns a list containing all the relationships in a path.
    /// https://neo4j.com/docs/cypher-manual/5/functions/list/#functions-relationships
    /// </summary>
    /// <param name="prop">The property.</param>
    /// <returns></returns>
    /// <example>
    /// MATCH p = (a)-->(b)-->(c)
    /// WHERE a.name = 'Alice' AND c.name = 'Eskil'
    /// RETURN relationships(p)
    /// </example>
    [Cypher("relationships($0)")]
    public static VariableDeclaration Relationships(object prop) => throw new NotImplementedException();

    #endregion relationships

}
