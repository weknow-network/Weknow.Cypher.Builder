using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder;

[Trait("TestType", "Unit")]
[Trait("Group", "Phrases")]

public class CaseTests
{
    private readonly ITestOutputHelper _outputHelper;

    #region Ctor

    public CaseTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    #endregion // Ctor

    #region MATCH (n)-[r]->(m) RETURN CASE WHEN n:Person&Friend THEN ...

    [Fact]
    public void Case_Pattern_Test()
    {
        var n = Variables.Create<Foo>();
        CypherCommand cypher = _(r => m => 
                                Match(N(n) - R[r] > N(m))
                                .Return()
                                .Case()
                                    .When(n, Person & Friend).Then(1)
                                    .When(r, !LIKE & !KNOWS).Then(2)
                                    .Else(-1)
                                .End().As("v"),
                                cfg => cfg.Flavor = CypherFlavor.Neo4j5);
        _outputHelper.WriteLine(cypher);
        Assert.Equal($"MATCH (n)-[r]->(m){NewLine}" +
                     $"RETURN{NewLine}" +
                     $"CASE{NewLine}" +
                     $"\tWHEN n:Person&Friend THEN 1{NewLine}" +
                     $"\tWHEN r:!LIKE&!KNOWS THEN 2{NewLine}" +
                     $"\tELSE -1{NewLine}" +
                     $"END AS v"
                       , cypher.Query);

        CypherParameters parameters = cypher.Parameters;
        Assert.Equal(0, parameters.Count);
    }

    #endregion // MATCH (n)-[r]->(m) RETURN CASE WHEN n:Person&Friend THEN ...

    #region CASE n.Color WHEN 'Blue' THEN '$100' ..

    [Fact]
    public void Case_String_Test()
    {
        var n = Variables.Create<Surface>();
        CypherCommand cypher = _(v =>
                                Match(N(n))
                                .Return()
                                .Case(n.__.Color)
                                .When(nameof(ConsoleColor.Blue)).Then("$100")
                                .When(nameof(ConsoleColor.Yellow)).Then("$50")
                                .Else("$30")
                                .End().As(v));

        _outputHelper.WriteLine(cypher);
        Assert.Equal($"MATCH (n){NewLine}" +
                     $"RETURN{NewLine}" +
                     $"CASE n.Color{NewLine}" +
                     $"\tWHEN 'Blue' THEN '$100'{NewLine}" +
                     $"\tWHEN 'Yellow' THEN '$50'{NewLine}" +
                     $"\tELSE '$30'{NewLine}" +
                     $"END AS v"
                       , cypher.Query);
        CypherParameters parameters = cypher.Parameters;
        Assert.Equal(0, parameters.Count);
    }

    #endregion // CASE n.Color WHEN 'Blue' THEN '$100' ..

    #region CASE $delimiter WHEN 7 THEN 2 ..

    [Fact]
    public void Case_Param_Test()
    {
        var delimiter = Parameters.Create<int>();
        CypherCommand cypher = _(v =>
                                Return()
                                .Case(delimiter)
                                .When(7).Then(2)
                                .Else(4)
                                .End().As(v));

        _outputHelper.WriteLine(cypher);
        Assert.Equal($"RETURN{NewLine}" +
                     $"CASE $delimiter{NewLine}" +
                     $"\tWHEN 7 THEN 2{NewLine}" +
                     $"\tELSE 4{NewLine}" +
                     $"END AS v"
                       , cypher.Query);
        CypherParameters parameters = cypher.Parameters;
        Assert.Equal(1, parameters.Count);
        Assert.True(parameters.ContainsKey(nameof(delimiter)));
    }

    #endregion // CASE $delimiter WHEN 7 THEN 2 ..

    #region CASE n.Color WHEN 'Blue' THEN '$100' ..

    [Fact]
    public void Case_Param_Math_Test()
    {
        var delimiter = Parameters.Create<int>();
        CypherCommand cypher = _(v =>
                                Return()
                                .Case()
                                .When(delimiter < 4).Then(1)
                                .When(delimiter == 40).Then(2)
                                .When(delimiter % 2 == 0).Then(3)
                                .Else(4)
                                .End().As(v));

        _outputHelper.WriteLine(cypher);
        Assert.Equal($"RETURN{NewLine}" +
                     $"CASE{NewLine}" +
                     $"\tWHEN $delimiter < 4 THEN 1{NewLine}" +
                     $"\tWHEN $delimiter = 40 THEN 2{NewLine}" +
                     $"\tWHEN $delimiter % 2 = 0 THEN 3{NewLine}" +
                     $"\tELSE 4{NewLine}" +
                     $"END AS v"
                       , cypher.Query);
        CypherParameters parameters = cypher.Parameters;
        Assert.Equal(1, parameters.Count);
        Assert.True(parameters.ContainsKey(nameof(delimiter)));
    }

    #endregion // CASE n.Color WHEN 'Blue' THEN '$100' ..

    #region CASE $delimiter WHEN 7 THEN 2 ..

    [Fact]
    public void Case_Array_Test()
    {
        var delimiter = Parameters.Create<int>();
        CypherCommand cypher = _(v =>
                                Case()
                                    .When(delimiter % 2 == 0).Then(new[] { 1, 2 })
                                    .When(delimiter % 3 == 0).Then(new List<int> { 2, 3, 4})
                                    .When(delimiter % 5 == 0 ).Then(new List<int> ())
                                    .Else(Array.Empty<int>())
                                .End().As(v));

        _outputHelper.WriteLine(cypher);
        Assert.Equal(
                     $"CASE{NewLine}" +
                     $"\tWHEN $delimiter % 2 = 0 THEN [1, 2]{NewLine}" +
                     $"\tWHEN $delimiter % 3 = 0 THEN [2, 3, 4]{NewLine}" +
                     $"\tWHEN $delimiter % 5 = 0 THEN []{NewLine}" +
                     $"\tELSE []{NewLine}" +
                     $"END AS v"
                       , cypher.Query);
        CypherParameters parameters = cypher.Parameters;
        Assert.Equal(1, parameters.Count);
        Assert.True(parameters.ContainsKey(nameof(delimiter)));
    }

    #endregion // CASE $delimiter WHEN 7 THEN 2 ..
}

