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
                $"DROP INDEX test-index"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);
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
                "DROP INDEX test-index IF EXISTS"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);
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
                $"CREATE INDEX test-index{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name)"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);
        }

        #endregion // Index_Test

        #region Index_Multi_Test

        [Fact]
        public void Index_Multi_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => 
                CreateIndex("test-index", N(n, Person), n._.Id, n._.Name)
                .CreateIndex("test-index1", N(n, Person), n._.Id, n._.Name)
                .TryCreateIndex("test-index2", N(n, Person), n._.Id, n._.Name)
                .CreateTextIndex("test-index3", N(n, Person), n._.Id, n._.Name)
                .TryCreateTextIndex("test-index4", N(n, Person), n._.Id, n._.Name)
                .CreateIndex("test-index5", N(n, Person), n._.Id, n._.Name)
                .TryCreateIndex("test-index6", N(n, Person), n._.Id, n._.Name)
                .CreateFullTextIndex("test-index7", N(n, Person), FullTextAnalyzer.english, n._.Id, n._.Name)
                .TryCreateFullTextIndex("test-index8", N(n, Person), FullTextAnalyzer.english, n._.Id, n._.Name)
                , 
                cfg =>
                    {
                        cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                    });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE INDEX test-index{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name){NewLine}" +

                $"CREATE INDEX test-index1{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name){NewLine}" +

                $"CREATE INDEX test-index2 IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name){NewLine}" +

                $"CREATE TEXT INDEX test-index3{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name){NewLine}" +

                $"CREATE TEXT INDEX test-index4 IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name){NewLine}" +

                $"CREATE INDEX test-index5{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name){NewLine}" +

                $"CREATE INDEX test-index6 IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name){NewLine}" +

                $"CREATE FULLTEXT INDEX test-index7{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON EACH (n.Id, n.Name){NewLine}" +
                $"\tOPTIONS {{{NewLine}\t\tindexConfig: {{{NewLine}\t\t\t`fulltext.analyzer`: 'english'{NewLine}\t\t  }}{NewLine}\t}}{NewLine}" +

                $"CREATE FULLTEXT INDEX test-index8 IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON EACH (n.Id, n.Name){NewLine}" +
                $"\tOPTIONS {{{NewLine}\t\tindexConfig: {{{NewLine}\t\t\t`fulltext.analyzer`: 'english'{NewLine}\t\t  }}{NewLine}\t}}" 
                , cypher.Query);
            Assert.Empty(cypher.Parameters);

        }

        #endregion // Index_Multi_Test

        #region Index_FullText1_Test

        [Fact]
        public void Index_FullText1_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => 
               CreateFullTextIndex("test-index", N(n, Person), FullTextAnalyzer.english, n._.Id, n._.Name)
                .TryCreateFullTextIndex("test-index1", N(n, Person), FullTextAnalyzer.english, n._.Id, n._.Name)
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
                $"\tOPTIONS {{{NewLine}\t\tindexConfig: {{{NewLine}\t\t\t`fulltext.analyzer`: 'english'{NewLine}\t\t  }}{NewLine}\t}}{NewLine}" +

                $"CREATE FULLTEXT INDEX test-index1 IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON EACH (n.Id, n.Name){NewLine}" +
                $"\tOPTIONS {{{NewLine}\t\tindexConfig: {{{NewLine}\t\t\t`fulltext.analyzer`: 'english'{NewLine}\t\t  }}{NewLine}\t}}" 
                , cypher.Query);
            Assert.Empty(cypher.Parameters);
        }

        #endregion // Index_FullText1_Test

        #region Index_FullText2_Test

        [Fact]
        public void Index_FullText2_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => 
               TryCreateFullTextIndex("test-index", N(n, Person), FullTextAnalyzer.english, n._.Id, n._.Name)
                .CreateFullTextIndex("test-index1", N(n, Person), FullTextAnalyzer.english, n._.Id, n._.Name)
                , 
                cfg =>
                    {
                        cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                    });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE FULLTEXT INDEX test-index IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON EACH (n.Id, n.Name){NewLine}" +
                $"\tOPTIONS {{{NewLine}\t\tindexConfig: {{{NewLine}\t\t\t`fulltext.analyzer`: 'english'{NewLine}\t\t  }}{NewLine}\t}}{NewLine}" +

                $"CREATE FULLTEXT INDEX test-index1{NewLine}" +
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
               CreateTextIndex("test-index", N(n, Person),  n._.Id, n._.Name)
                .TryCreateTextIndex("test-index1", N(n, Person),  n._.Id, n._.Name)
                , 
                cfg =>
                    {
                        cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                    });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE TEXT INDEX test-index{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name){NewLine}" +

                $"CREATE TEXT INDEX test-index1 IF NOT EXISTS{NewLine}" +
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
               TryCreateTextIndex("test-index", N(n, Person), n._.Id, n._.Name)
                .CreateTextIndex("test-index1", N(n, Person), n._.Id, n._.Name)
                , 
                cfg =>
                    {
                        cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                    });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE TEXT INDEX test-index IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tON (n.Id, n.Name){NewLine}" +

                $"CREATE TEXT INDEX test-index1{NewLine}" +
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

