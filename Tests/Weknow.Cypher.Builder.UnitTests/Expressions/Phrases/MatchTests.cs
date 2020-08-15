using System;

using Weknow.Cypher.Builder.Declarations;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
        [Trait("Group", "Phrases")]
    [Trait("Segment", "Expression")]
    public class MatchTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public MatchTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MATCH (n:Person { Id: $Id }) RETURN n / Match_Return_Test

        [Fact]
        public void Match_Return_Test()
        {
            var n = Variables.Create();
            var Id = Parameters.Create();
            CypherCommand cypher = _(() => Match(N(n, Person, new { Id }))
                                    .Return(n));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) RETURN n / Match_Return_Test

        #region MATCH (n:Person { Id: $Id }), (a:Bar:Animal { Name: $Name } RETURN n / Match_2_Return_Test

        [Fact]
        public void Match_2_Return_Test()
        {
            var (n, a) = Variables.CreateMulti();
            var Id = Parameters.Create();
            var p = Parameters.Create<Bar>();

            CypherCommand cypher = _(() =>
                            Match(
                                N(n, Person, new { Id }),
                                N(a, Animal, p._.Name))
                            .Return(n));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
                        "MATCH (n:Person { Id: $Id }), " +
                        "(a:Bar:Animal { Name: $Name })\r\n" +
                        "RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }), (a:Bar:Animal { Name: $Name } RETURN n / Match_2_Return_Test

        #region MATCH (n:Person { Id: $Id }), (a:Animal { Name: $Name } RETURN n / Match_2_Return_NoGenLabel_Test

        [Fact]
        public void Match_2_Return_NoGenLabel_Test()
        {
            var Id = Parameters.Create();
            var p = Parameters.Create<Bar>();
            var (n, a) = Variables.CreateMulti();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id }),
                                          N(a, Animal, p._.Name))
                                    .Return(n));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
                        "MATCH (n:Person { Id: $Id }), " +
                        "(a:Animal { Name: $Name })\r\n" +
                        "RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }), (a:Animal { Name: $Name } RETURN n / Match_2_Return_NoGenLabel_Test

        #region Match_Pre_Return_Test

        [Fact]
        public void Match_Pre_Return_Test()
        {
            ParameterDeclaration? n_Id = null;
            var n = Variables.Create();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id = n_Id }))
                                    .Return(n));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
@"MATCH (n:Person { Id: $n_Id })
RETURN n", cypher.Query);
        }

        #endregion // Match_Pre_Return_Test

        #region Match_Multi_Return_Test

        [Fact]
        public void Match_Multi_Return_Test()
        {
            ParameterDeclaration? nId = null, mId = null;
            var (n, m) = Variables.CreateMulti();

            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id = nId }))
                                    .Match(N(m, Person, new { Id = mId }))
                                    .Return(n, m));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
@"MATCH (n:Person { Id: $nId })
MATCH (m:Person { Id: $mId })
RETURN n, m", cypher.Query);
        }

        #endregion // Match_Multi_Return_Test

        #region Match_SetAsMap_Update_Test

        [Fact]
        public void Match_SetAsMap_Update_Test()
        {
            var Id = Parameters.Create();
            var map = Parameters.Create();
            var n = Variables.Create();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id }))
                                    .Set(n.peq(map)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n += $map", cypher.Query);
        }

        #endregion // Match_SetAsMap_Update_Test

        #region Match_SetAsMap_Replace_Test

        [Fact]
        public void Match_SetAsMap_Replace_Test()
        {
            var n = Variables.Create();
            var (Id, map) = Parameters.CreateMulti();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id }))
                                    .Set(n.eq(map.AsMap)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n = $map", cypher.Query);
        }

        #endregion // Match_SetAsMap_Replace_Test

        // TODO: [bnaya, 2020_08] disuss API with Avi
        #region Match_Set_Test

        [Fact]
        public void Match_Set_Test()
        {
            var Id = Parameters.Create();
            var p = Parameters.Create<Foo>();
            var n = Variables.Create();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id }))
                                    .Set(n.eq(new { p._.PropA, p._.PropB }))); 

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // Match_Set_Test

        #region Match_Set_AddLabel_Test

        [Fact]
        public void Match_Set_AddLabel_Test()
        {
            var Id = Parameters.Create();
            CypherCommand cypher = _(n =>
                                    Match(N<Foo>(n, new { Id }))
                                    .Set(n, Person));
            
            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Foo { Id: $Id })
SET n:Person", cypher.Query);
        }

        #endregion // Match_Set_AddLabel_Test

        #region Match_Set_AddLabels_Test

        [Fact]
        public void Match_Set_AddLabels_Test()
        {
            var Id = Parameters.Create();
            CypherCommand cypher = _(n =>
                                    Match(N<Foo>(n, new { Id }))
                                    .Set(n, Person, Manager));
            
            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Foo { Id: $Id })
SET n:Person:Manager", cypher.Query);
        }

        #endregion // Match_Set_AddLabels_Test
    }
}

