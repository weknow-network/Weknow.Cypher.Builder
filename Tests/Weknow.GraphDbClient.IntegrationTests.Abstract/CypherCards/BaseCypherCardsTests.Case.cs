using Weknow.CypherBuilder;

using Xunit;

using static Weknow.CypherBuilder.ICypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

public partial class BaseCypherCardsTests
{
    #region CREATE(user:PERSON:_TEST_ $map)

    [Theory]
    [InlineData("r", "red")]
    [InlineData("g", "green")]
    [InlineData("b", "whatever")]
    public virtual async Task CreateCase_Test(string input, string expected)
    {
        var delimiter = Parameters.Create<string>();

        CypherCommand cypher = _(() =>
                                Return()
                                .Case(delimiter)
                                .When("r").Then("red")
                                .When("g").Then("green")
                                .Else("whatever")
                                .End().As("color"));


        _outputHelper.WriteLine($"CYPHER: {cypher}");

        CypherParameters prms = cypher.Parameters
                                      .AddOrUpdate(nameof(delimiter), input);
        var response = await _graphDB.RunAsync(cypher, prms);
        var result = await response.GetAsync<string>("color");
        Assert.Equal(expected, result);
    }

    #endregion // CREATE(user:PERSON:_TEST_ $map)
}
