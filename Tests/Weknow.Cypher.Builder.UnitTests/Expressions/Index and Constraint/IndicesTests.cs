using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.GraphDbCommands.Cypher;
using static Weknow.GraphDbCommands.Schema;

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

        #region DROP INDEX test-index

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
                $"DROP INDEX test-index"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);
        }

        #endregion // DROP INDEX test-index

        #region DROP INDEX test-index IF EXISTS

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
                "DROP INDEX test-index IF EXISTS"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);
        }

        #endregion // DROP INDEX test-index IF EXISTS

        #region CREATE INDEX test-index FOR (n:PERSON) ON (n.Id, n.Name)

        [Fact]
        public void Index_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => CreateIndex("test-index", N(n, Person), n._.Id, n._.Name), cfg =>
            {
                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                cfg.AmbientLabels.Add("AmbientLabel"); // index should ignore it
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE INDEX test-index{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name)"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);
        }

        #endregion // CREATE INDEX test-index FOR (n:PERSON) ON (n.Id, n.Name)

        #region Index_FullText1_Test

        [Fact]
        public void Index_FullText1_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() =>
               CreateFullTextIndex("test-index", N(n, Person), FullTextAnalyzer.english, n._.Id, n._.Name)
                ,
                cfg =>
                    {
                        cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                    });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE FULLTEXT INDEX test-index{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON EACH (n.Id, n.Name){NewLine}" +
                $"\tOPTIONS {{{NewLine}\t\tindexConfig: {{{NewLine}\t\t\t`fulltext.analyzer`: 'english'{NewLine}\t\t  }}{NewLine}\t}}", cypher.Query);
            Assert.Empty(cypher.Parameters);
        }

        #endregion // Index_FullText1_Test

        #region Index_FullText2_Test

        [Fact]
        public void Index_FullText2_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() =>
               TryCreateFullTextIndex("test-index", N(n, Person), FullTextAnalyzer.english, n._.Id, n._.Name),
                cfg =>
                    {
                        cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                    });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE FULLTEXT INDEX test-index IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON EACH (n.Id, n.Name){NewLine}" +
                $"\tOPTIONS {{{NewLine}\t\tindexConfig: {{{NewLine}\t\t\t`fulltext.analyzer`: 'english'{NewLine}\t\t  }}{NewLine}\t}}"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);
        }

        #endregion // Index_FullText2_Test

        #region Index_Text1_Test

        [Fact]
        public void Index_Text1_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() =>
               CreateTextIndex("test-index", N(n, Person), n._.Id, n._.Name)
                ,
                cfg =>
                    {
                        cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                    });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE TEXT INDEX test-index{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name)"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);
        }

        #endregion // Index_Text1_Test

        #region Index_Text2_Test

        [Fact]
        public void Index_Text2_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() =>
               TryCreateTextIndex("test-index", N(n, Person), n._.Id, n._.Name),
                cfg =>
                    {
                        cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                    });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE TEXT INDEX test-index IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name)"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);
        }

        #endregion // Index_Text2_Test

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
                $"CREATE INDEX test-index IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name)"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);
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
                $"CREATE INDEX test-index IF NOT EXISTS{NewLine}" +
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
            Assert.Empty(cypher.Parameters);
        }

        #endregion // TryIndex_Options_Test
    }
}

