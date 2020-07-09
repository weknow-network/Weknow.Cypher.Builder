
using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
    [Trait("Segment", "Invalid-Api")]
    public class INVALID_API_TESTS
    {
        private readonly ITestOutputHelper _outputHelper;

        private const string INVALID_MESSAGE = 
                            "Shouldn't compile " +
                            "because it will fall on runtime. " +
                            "those scenarios should be separate at the signature level," +
                            "Either by changing the signature or by analyzer";

        #region Ctor

        public INVALID_API_TESTS(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Match_Set_Invalid_Test

        [Fact]
        public void Match_Set_Invalid_Test()
        {
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, P(Id)))
                                    .Set(P(nameof(Foo.PropA), nameof(Foo.PropB))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MATCH (n:Person { Id: $Id })\r\nSET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
            throw new MethodAccessException(INVALID_MESSAGE);
        }

        #endregion // Match_Set_Invalid_Test

        #region Properties_Invalid_OfT_DefaultAndAdditionLabel_Test

        [Fact]
        public void Properties_Invalid_OfT_DefaultAndAdditionLabel_Test()
        {
            CypherCommand cypher = _(n => Match(N<Foo>(n, Person, P(nameof(Foo.PropA), nameof(Foo.PropB)))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n:Foo:Person { PropA: $PropA, PropB: $PropB })", cypher.Query);
            throw new MethodAccessException(INVALID_MESSAGE);
        }

        #endregion // Properties_Invalid_OfT_DefaultAndAdditionLabel_Test

        #region Merge_On_SetNamedAsMap_Update_Test

        [Fact]
        public void Merge_On_SetNamedAsMap_Update_Test()
        {
            CypherCommand cypher = _(n => map =>
                                    Merge(N(n, Person, P(Id)))
                                    .OnCreateSet(n.Convention<Foo>(m => m != nameof(Foo.Id)))
                                    .OnMatchSet(/*valid*/ /*+*/n, /*invalid*/ +map.AsMap));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Id: $Id })\r\n\t" +
                    "ON CREATE SET n.Name = $Name, n.PropA = $PropA, n.PropB = $PropB\r\n\t" +
                    "ON MATCH SET n += $map", cypher.Query);
            throw new MethodAccessException(INVALID_MESSAGE);
        }

        #endregion // Merge_On_SetNamedAsMap_Update_Test

        #region Properties_Invalid_All_Except_WithDefaultLabel_Test

        [Fact]
        public void Properties_Invalid_All_Except_WithDefaultLabel_Test()
        {
            CypherCommand cypher = _(n => Match(N<Foo>(n, AllExcept(nameof(Foo.Id), nameof(Foo.Name)))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n:Foo { PropA: $PropA, PropB: $PropB })", cypher.Query);
            throw new MethodAccessException(INVALID_MESSAGE);
        }

        #endregion // Properties_Invalid_All_Except_WithDefaultLabel_Test

        #region With_Needed_Test

        [Fact]
        public void With_Needed_Test()
        {
            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, Id))
                                    .Match(N(n, Person, Id)));

            _outputHelper.WriteLine(cypher);
            throw new MethodAccessException(INVALID_MESSAGE);
        }

        #endregion // MWith_Needed_Test

        #region MATCH (n) RETURN n.Id, n.PropA, n.PropB, n.pre_PropC / Return_Props_Lambda_Prefix_Test

        [Fact]
        public void Return_Props_Lambda_Prefix_Test()
        {
            CypherCommand cypher = _(n => pre_ =>
                                    Match(N(n))
                                    .Return(n.P(Id, P<Foo>(x => x.PropA, x => x.PropB), _P(pre_, PropC))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n)\r\n" +
                           "RETURN n.Id, n.PropA, n.PropB, n.pre_PropC", cypher.Query);
        }

        #endregion // MATCH (n) RETURN n.Id, n.PropA, n.PropB, n.pre_PropC / Return_Props_Lambda_Prefix_Test
    }
}