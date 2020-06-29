using Xunit;
using Xunit.Abstractions;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherConfigTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherConfigTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Generate_Expression_Test

        [Fact]
        public void Label_Convention_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("Dev", "GitHub");
                            cfg.AmbientLabels.Formatter = "'@X_{0}'";
                            cfg.Naming.NodeLabelConvention = CypherNamingConvention.SCREAMING_CASE;
                        })
                        .Entities.CreateIfNotExists<Foo>("items", f => f.Id, "map");

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("UNWIND $items AS map " +
                            "MERGE (f:FOO:'@X_DEV':'@X_GIT_HUB' { Id: map.Id })" +
                            " ON CREATE SET f = map RETURN f "
                            , cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Generate_Expression_Test
    }
}