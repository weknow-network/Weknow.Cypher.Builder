using System;
using System.Xml.Linq;

using Weknow.GraphDbCommands.Declarations;

using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;
using static Weknow.GraphDbCommands.Schema;
using static System.Environment;

// https://neo4j.com/docs/cypher-cheat-sheet/current/
// https://neo4j.com/docs/cypher-manual/5/indexs/

namespace Weknow.GraphDbCommands
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Index")]
    public class IndicesTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public IndicesTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region DropIndex_Test

        [Fact]
        public void DropIndex_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => DropIndex("test-index"), cfg =>
            {
                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"DROP INDEX $p_0"
                , cypher.Query);
            Assert.Equal("test-index", cypher.Parameters["p_0"]);
            Assert.Equal("test-index", cypher.Parameters["$p_0"]);
        }

        #endregion // DropIndex_Test

        #region TryDropIndex_Test

        [Fact]
        public void TryDropIndex_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => TryDropIndex("test-index"), cfg =>
            {
                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "DROP INDEX $p_0 IF NOT EXISTS"
                , cypher.Query);
            Assert.Equal("test-index", cypher.Parameters["p_0"]);
        }

        #endregion // TryDropIndex_Test

        #region Index_Test

        [Fact]
        public void Index_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => CreateIndex("test-index", N(n, Person), n._.Id, n._.Name), cfg =>
            {
                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE INDEX $p_0{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name)"
                , cypher.Query);
            Assert.Equal("test-index", cypher.Parameters["p_0"]);
            Assert.Equal("test-index", cypher.Parameters["$p_0"]);
        }

        #endregion // Index_Test

        #region TryIndex_Test

        [Fact]
        public void TryIndex_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => TryCreateIndex("test-index", N(n, Person), n._.Id, n._.Name), cfg =>
            {
                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE INDEX $p_0 IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name)"
                , cypher.Query);
            Assert.Equal("test-index", cypher.Parameters["p_0"]);
            Assert.Equal("test-index", cypher.Parameters["$p_0"]);
        }

        #endregion // TryIndex_Test

        #region TryIndex_Options_Test

        [Fact]
        public void TryIndex_Options_Test()
        {
            var n = Variables.Create<Foo>();
#pragma warning disable CS0618 // Type or member is obsolete
            CypherCommand cypher = _(() => TryCreateIndex("test-index",
                                                N(n, Person), 
                                                new[] { n._.Id, n._.Name })
                                           .WithRawCypher(@"
OPTIONS {
  indexConfig: {
    `spatial.wgs-84.min`: [-100.0, -100.0],
    `spatial.wgs-84.max`: [100.0, 100.0]
  }
}"), cfg =>
            {
                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
            });
#pragma warning restore CS0618 // Type or member is obsolete

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE INDEX $p_0 IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name){NewLine}" +
                @"
OPTIONS {
  indexConfig: {
    `spatial.wgs-84.min`: [-100.0, -100.0],
    `spatial.wgs-84.max`: [100.0, 100.0]
  }
}"
                , cypher.Query);
            Assert.Equal("test-index", cypher.Parameters["p_0"]);
            Assert.Equal("test-index", cypher.Parameters["$p_0"]);
        }

        #endregion // TryIndex_Options_Test
    }
}

