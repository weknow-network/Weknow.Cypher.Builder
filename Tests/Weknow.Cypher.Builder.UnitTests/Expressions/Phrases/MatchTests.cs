#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

using Weknow.CypherBuilder.Declarations;
using Weknow.Mapping;

using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Phrases")]

    public class MatchTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public MatchTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MATCH (n:Person:Manager  { Id: $Id }) RETURN n

        [Fact]
        public void Match_Return_Multi_Label_Test()
        {
            var n = Variables.Create();
            var Id = Parameters.Create();
            CypherCommand cypher = _(() => Match(N(n, Person & Manager, new { Id }))
                                    .Return(n));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
$"MATCH (n:Person:Manager {{ Id: $Id }}){NewLine}" +
"RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person:Manager { Id: $Id }) RETURN n 

        #region MATCH (n:Person { Id: $Id }) RETURN n 

        [Fact]
        public void Match_Return_Test()
        {
            var n = Variables.Create();
            var Id = Parameters.Create();
            CypherCommand cypher = _(() => Match(N(n, Person, new { Id }))
                                    .Return(n));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
$"MATCH (n:Person {{ Id: $Id }}){NewLine}" +
"RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) RETURN n 

        #region MATCH (n:Person { Id: $Id }) RETURN n 

        [Fact]
        public void Match_Or_Label_Test()
        {
            var n = Variables.Create();
            var Id = Parameters.Create();
            CypherCommand cypher = _(() => Match(N(n, Person | Animal, new { Id }))
                                    .Return(n)
                                    , cfg => cfg.Flavor = Flavor.Neo4j);

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
$"MATCH (n:Person|Animal {{ Id: $Id }}){NewLine}" +
"RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) RETURN n 

        #region MATCH (n:Person { Id: $Id }), (a:Bar:Animal { Name: $Name } RETURN n

        [Fact]
        public void Match_2_Return_Test()
        {
            var (n, a) = Variables.CreateMulti();
            var Id = Parameters.Create();
            var p = Parameters.Create<Bar>();

            CypherCommand cypher = _(() =>
                            Match(
                                N(n, Person, new { Id }),
                                N(a, Animal, new { p._.Name }))
                            .Return(n));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                       "MATCH (n:Person { Id: $Id }), " +
                       $"(a:Animal {{ Name: $Name }}){NewLine}" +
                       "RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }), (a:Bar:Animal { Name: $Name } RETURN n

        #region MATCH (n:Person { Id: $Id }), (a:Animal { Name: $Name } RETURN n

        [Fact]
        public void Match_2_Return_NoGenLabel_Test()
        {
            var Id = Parameters.Create();
            var p = Parameters.Create<Bar>();
            var (n, a) = Variables.CreateMulti();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id }),
                                          N(a, Animal, new { p._.Name }))
                                    .Return(n));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                       "MATCH (n:Person { Id: $Id }), " +
                       $"(a:Animal {{ Name: $Name }}){NewLine}" +
                       "RETURN n", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }), (a:Animal { Name: $Name } RETURN n

        #region MATCH (n:Person { Id: $n_Id }) RETURN n

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

        #endregion // MATCH (n:Person { Id: $n_Id }) RETURN n

        #region MATCH (n:Person { Id: $nId }) MATCH(m:Person { Id: $mId }) RETURN n, m

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

        #endregion // MATCH (n:Person { Id: $nId }) MATCH(m:Person { Id: $mId }) RETURN n, m

        #region MATCH (n:Person {{ Id: $Id }}) SET n += $map

        [Fact]
        public void Match_SetAsMap_Update_Test()
        {
            var Id = Parameters.Create();
            var map = Parameters.Create();
            var n = Variables.Create();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id }))
                                    .SetPlus(n, map));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
$"MATCH (n:Person {{ Id: $Id }}){NewLine}" +
"SET n += $map", cypher.Query);
        }

        #endregion // MATCH (n:Person {{ Id: $Id }}) SET n += $map 

        #region MATCH (n:Person {{ Id: $Id }}) SET n = $map

        [Fact]
        public void Match_SetAsMap_Replace_Test()
        {
            var n = Variables.Create();
            var (Id, map) = Parameters.CreateMulti();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id }))
                                    .Set(n, map));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
$"MATCH (n:Person {{ Id: $Id }}){NewLine}" +
"SET n = $map", cypher.Query);
        }

        #endregion // MATCH (n:Person {{ Id: $Id }}) SET n = $map

        #region UNWIND $items AS item MATCH (n:Person { Id: $Id }) SET n = item

        [Fact]
        public void Match_Set1_Test()
        {
            var Id = Parameters.Create();
            var items = Parameters.Create<Foo>();
            var n = Variables.Create();
            CypherCommand cypher = _(() =>
                                    Unwind(items, item =>
                                    Match(N(n, Person, new { Id }))
                                    .Set(n, item)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
$"UNWIND $items AS item{NewLine}" +
$$"""MATCH (n:Person { Id: $Id }){{NewLine}}""" +
"SET n = item", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { Id: $Id }) SET n = item

        #region MATCH (n:Person { Id: $Id }) SET n.PropA = $PropA, n.PropB = $PropB

        [Fact]
        public void Match_Set_Test()
        {
            var Id = Parameters.Create();
            var p = Parameters.Create<Foo>();
            var n = Variables.Create();
            CypherCommand cypher = _(() =>
                                    Match(N(n, Person, new { Id }))
                                    .Set(n, new { p._.PropA, p._.PropB }));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n = { PropA: $PropA, PropB: $PropB }", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) SET n.PropA = $PropA, n.PropB = $PropB

        #region MATCH (n:Person { Id: $Id }) SET n:Person

        [Fact]
        public void Match_Set_AddLabel_Test()
        {
            var Id = Parameters.Create();
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, new { Id }))
                                    .Set(n, Person));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n:Person", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) SET n:Person

        #region MATCH (n:Person { Id: $Id }) SET n:Person:Manager

        [Fact]
        public void Match_Set_AddLabels_Test()
        {
            var Id = Parameters.Create();
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, new { Id }))
                                    .Set(n, Person, Manager));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n:Person:Manager", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) SET n:Person:Manager

        #region MATCH (n)-->(m) RETURN m

        [Fact]
        public void Match_Pattern_Test()
        {
            var (n, m) = Variables.CreateMulti();
            CypherCommand cypher = _(() =>
                                    Match(N(n) > N(m))
                                    .Return(m));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
$"MATCH (n)-->(m){NewLine}" +
"RETURN m", cypher.Query);
        }

        #endregion // MATCH (n)-->(m) RETURN m

        #region MATCH (n)-->(m) RETURN m

        [Fact]
        public void Match_Pattern_Inline_Test()
        {
            CypherCommand cypher = _((n, m) =>
                                    Match(N(n) > N(m))
                                    .Return(m));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
$"MATCH (n)-->(m){NewLine}" +
"RETURN m", cypher.Query);
        }

        #endregion // MATCH (n)-->(m) RETURN m

        #region MATCH (n:Person { Id: $Id }) SET n:Person:Manager

        [Fact]
        public void Match_Set_AddLabels1_Test()
        {
            var Id = Parameters.Create();
            CypherCommand cypher = _(n =>
                                    Match(N(n, Person, new { Id }))
                                    .Set(n, Person & Manager));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
@"MATCH (n:Person { Id: $Id })
SET n:Person:Manager", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) SET n:Person:Manager
    }
}

