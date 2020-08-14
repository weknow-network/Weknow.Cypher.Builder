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
                                    .Set(+n, map.AsMap));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n += $n", cypher.Query);
        }

        #endregion // Match_SetAsMap_Update_Test

        #region Match_SetAsMap_Replace_Test

        [Fact]
        public void Match_SetAsMap_Replace_Test()
        {
            var n = Variables.Create();
            var Id = Parameters.Create();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id }))
                                    .Set(n.AsMap));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n = $n", cypher.Query);
        }

        #endregion // Match_SetAsMap_Replace_Test

        // TODO: [bnaya, 2020_08] disuss API with Avi
        #region Match_Set_Test

        [Fact]
        public void Match_Set_Test()
        {
            var Id = Parameters.Create();
            var n = Variables.Create();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id }))
                                    .Set(n, PropA)
                                    .Set(n, PropB)); // ???

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // Match_Set_Test

        // TODO: [bnaya, 2020_08] disuss API with Avi
        #region Match_Set_OfT_Test

        [Fact]
        public void Match_Set_OfT_Test()
        {
            var Id = Parameters.Create();
            var n = Variables.Create();
            var p = Variables.Create<Foo>();

            CypherCommand cypher = _<Foo>(n =>
                                    Match(N(n, Person, new { Id }))
                                    .Set(n._(new { p._.PropA, p._.PropB })));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // Match_Set_OfT_Test

        #region Match_Set_OfT_Convention_Test

        [Fact]
        public void Match_Set_OfT_Convention_Test()
        {
            var Id = Parameters.Create();
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, new { Id }))
                                    .Set(n.Convention<Foo>(name => name.StartsWith("Prop"))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n.PropA = $PropA, n.PropB = $PropB", cypher.Query);
        }

        #endregion // Match_Set_OfT_Convention_Test

        #region Match_Set_OfT_All_Test

        [Fact]
        public void Match_Set_OfT_All_Test()
        {
            var Id = Parameters.Create();
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, new { Id }))
                                    .Set(n.All<Foo>()));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n.Id = $Id, n.Name = $Name, n.PropA = $PropA, n.PropB = $PropB, n.FirstName = $FirstName, n.LastName = $LastName", cypher.Query);
        }

        #endregion // Match_Set_OfT_All_Test

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
    }
}

