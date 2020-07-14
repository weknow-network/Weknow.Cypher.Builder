using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
    [Trait("Category", "With")]
    [Trait("Group", "Phrases")]
    [Trait("Segment", "Expression")]
    public class WithTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public WithTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MERGE (n:PERSON { Id: $Id }) WITH * MATCH (i:PERSON { Id: $Id }) RETURN i.Name / With_Test

        [Fact]
        public void With_Test()
        {
            CypherCommand cypher = _(n => i =>
                        Merge(N(n, Person, n.P(Id)))
                        .With()
                        .Match(N(i, Person, n.P(Id)))
                        .Return(i.OfType<Foo>().Name),
                        cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);
            ;

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
                "MERGE (n:PERSON { Id: $Id })\r\n" +
                "WITH *\r\n" +
                "MATCH (i:PERSON { Id: $Id })\r\n" +
                "RETURN i.Name", cypher.Query);
        }

        #endregion // MERGE (n:PERSON { Id: $Id }) WITH * MATCH (i:PERSON { Id: $Id }) RETURN i.Name / With_Test

        #region MERGE (n:PERSON { Id: $Id }) WITH n MATCH (i:PERSON { Id: $Id }) RETURN i.Name / With_Param_Test

        [Fact]
        public void With_Param_Test()
        {
            CypherCommand cypher = _(n => i =>
                        Merge(N(n, Person, n.P(Id)))
                        .With(n)
                        .Match(N(i, Person, n.P(Id)))
                        .Return(i.OfType<Foo>().Name),
                        cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);
            ;

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
                "MERGE (n:PERSON { Id: $Id })\r\n" +
                "WITH n\r\n" +
                "MATCH (i:PERSON { Id: $Id })\r\n" +
                "RETURN i.Name", cypher.Query);
        }

        #endregion // MERGE (n:PERSON { Id: $Id }) WITH n MATCH (i:PERSON { Id: $Id }) RETURN i.Name / With_Param_Test

        #region UNWIND ... MERGE (n:PERSON ...) WITH * MATCH (i:PERSON ...}) RETURN n / With_Complex_Test

        [Fact]
        public void With_Complex_Test()
        {
            CypherCommand cypher = _(items => map => n => i =>
                        Unwind(items, map,
                        Merge(N(n, Person, n.P(Id)))
                        .OnCreateSet(n, map.AsMap)
                        .OnMatchSet(+n, map.AsMap)
                        .With()
                        .Match(N(i, Person, n.P(Id)))
                        .Return(n)),
                        cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);
            ;

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
                "UNWIND $items AS map\r\n" +
                "MERGE (n:PERSON { Id: map.Id })\r\n\t" +
                    "ON CREATE SET n = map\r\n\t" +
                    "ON MATCH SET n += map\r\n" +
                "WITH *\r\n" +
                "MATCH (i:PERSON { Id: map.Id })\r\n" +
                "RETURN n", cypher.Query);
        }

        #endregion // UNWIND ... MERGE (n:PERSON ...) WITH * MATCH (i:PERSON ...}) RETURN n / With_Complex_Test

        #region UNWIND ... MERGE (n:PERSON ...) WITH n, map MATCH (i:PERSON ...}) RETURN i / With_Params_Complex_Test

        [Fact]
        public void With_Params_Complex_Test()
        {
            CypherCommand cypher = _(items => map => n => i =>
                        Unwind(items, map,
                        Merge(N(n, Person, n.P(Id)))
                        .OnCreateSet(n, map.AsMap)
                        .OnMatchSet(+n, map.AsMap)
                        .With(n, map)
                        .Match(N(i, Person, n.P(Id)))
                        .Return(i)),
                        cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);
            ;

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
                "UNWIND $items AS map\r\n" +
                "MERGE (n:PERSON { Id: map.Id })\r\n\t" +
                    "ON CREATE SET n = map\r\n\t" +
                    "ON MATCH SET n += map\r\n" +
                "WITH n, map\r\n" +
                "MATCH (i:PERSON { Id: map.Id })\r\n" +
                "RETURN i", cypher.Query);
        }

        #endregion // UNWIND ... MERGE (n:PERSON ...) WITH n, map MATCH (i:PERSON ...}) RETURN i / With_Complex_Test
    }
}

