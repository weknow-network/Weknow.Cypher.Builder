using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.GraphDbCommands.Cypher;

namespace Weknow.GraphDbCommands
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Phrases")]
    [Trait("Segment", "Expression")]
    public class AsCypherTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public AsCypherTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MERGE (n:Person { Id: $Id }) ON CREATE SET n.PropA = $PropA, n.PropB = $PropB
#pragma warning disable CS0618 // Type or member is obsolete

        [Fact]
        public void Merge_On_Create_SetProperties_Test()
        {
            var Id = Parameters.Create();
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    RawCypher(@"MATCH (n:Person { Id: $Id })")
                                    .Where(n._.Name == "Mike")
                                    .WithRawCypher("RETURN n"));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MATCH (n:Person {{ Id: $Id }}){NewLine}" +
                $"WHERE n.Name = $p_0{NewLine}" +
                "RETURN n"
                , cypher.Query);
        }

#pragma warning restore CS0618 // Type or member is obsolete
        #endregion // MERGE (n:Person { Id: $Id }) ON CREATE SET n.PropA = $PropA, n.PropB = $PropB
    }
}

