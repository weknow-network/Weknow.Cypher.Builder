#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]

    public class ReuseTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public ReuseTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MATCH (person:Person)-[r:LIKE]->(animal:Animal) / LazyReuse_Node_Test

        [Fact]
        public void LazyReuse_Node_Test()
        {
            var (person, animal) = Variables.CreateMulti();
            var reusedPerson = Reuse(() => N(person, Person));
            var reusedAnimal = Reuse(() => N(animal, Animal));

            CypherCommand cypher = _(r =>
                          Match(reusedPerson - R[r, LIKE] > reusedAnimal));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (person:Person)-[r:LIKE]->(animal:Animal)", cypher.Query);
        }

        #endregion // MATCH (person:Person)-[r:LIKE]->(animal:Animal) / LazyReuse_Node_Test

        #region (n:Person { PropA: $nPropA }) ... / LazyReuse_Overloads_Test

        [Fact]
        public void LazyReuse_Overloads_Test()
        {
            var (nPropA, n_PropA, n1_PropA, n2_PropB, n1_Id, n2_PropA, n3_PropB, n4_PropC) = Parameters.CreateMulti();
            var n = Variables.Create();

            IPattern? pattern1 = Reuse(() => N(n, Person, new { PropA = nPropA }));
            IPattern? pattern2 = Reuse(() => N(n, Person, new { PropA = n_PropA }));
            IPattern? pattern3 = Reuse(() => N(n, Person, new { PropA = n1_PropA, PropB = n2_PropB }));
            IPattern? pattern4 = Reuse(() => N(n, Person, new { Id = n1_Id, PropA = n2_PropA, PropB = n3_PropB }));
            IPattern? pattern5 = Reuse(() => N(n, Person, new { Id = n1_Id, PropA = n2_PropA, PropB = n3_PropB, PropC = n4_PropC }));

            string? cypher1 = pattern1?.ToString();
            string? cypher2 = pattern2?.ToString();
            string? cypher3 = pattern3?.ToString();
            string? cypher4 = pattern4?.ToString();
            string? cypher5 = pattern5?.ToString();

            _outputHelper.WriteLine(cypher1);
            _outputHelper.WriteLine(cypher2);
            _outputHelper.WriteLine(cypher3);
            _outputHelper.WriteLine(cypher4);
            _outputHelper.WriteLine(cypher5);

            Assert.Equal("(n:Person { PropA: $nPropA })", cypher1);
            Assert.Equal("(n:Person { PropA: $n_PropA })", cypher2);
            Assert.Equal("(n:Person { PropA: $n1_PropA, PropB: $n2_PropB })", cypher3);
            Assert.Equal("(n:Person { Id: $n1_Id, PropA: $n2_PropA, PropB: $n3_PropB })", cypher4);
            Assert.Equal("(n:Person { Id: $n1_Id, PropA: $n2_PropA, PropB: $n3_PropB, PropC: $n4_PropC })", cypher5);
        }

        #endregion // (n:Person { PropA: $nPropA }) ... / LazyReuse_Overloads_Test

        #region [:LIKE] / Reuse_Relation_Test

        [Fact]
        public void Reuse_Relation_Test()
        {
            var pattern = Reuse(() => R[LIKE]);

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"[:LIKE]", pattern.ToString());
        }

        #endregion // [:LIKE] / Reuse_Relation_Test

        #region (n:Person:Animal)-[:LIKE] / Reuse_Node_And_Relation_Test

        [Fact]
        public void Reuse_Node_And_Relation_Test()
        {
            var n = Variables.Create();
            var pattern = Reuse(() => N(n, Person & Animal) - R[LIKE]);

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(n:Person:Animal)-[:LIKE]", pattern.ToString());
        }

        #endregion // (n:Person:Animal)-[:LIKE] / Reuse_Node_And_Relation_Test

        #region (n:Person:Animal)-[:LIKE] / Reuse_Node_And_Relation_Test

        [Fact]
        public void Reuse_Node_As_Type_Test()
        {
            var n = Variables.Create();
            var pattern = Reuse(() => N(n, Locale) - R[Language.R] > N(Language));

            var cypher = pattern.ToString();
            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"(n:Locale)-[:Language]->(:Language)", cypher);
        }

        #endregion // (n:Person:Animal)-[:LIKE] / Reuse_Node_And_Relation_Test

        #region (a)-[r1]->(b) / Reuse_N_R_N_Test

        [Fact]
        public void Reuse_N_R_N_Test()
        {
            var (a, r1, b) = Variables.CreateMulti();
            var pattern = Reuse(() =>
                        N(a) - R[r1] > N(b));

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(a)-[r1]->(b)", pattern.ToString());
        }

        #endregion // (a)-[r1]->(b) / Reuse_N_R_N_Test

        #region (a)-[r1]->(b) / Reuse_N_R2_N_Test

        [Fact]
        public void Reuse_N_R2_N_Test()
        {
            var p = Parameters.Create<Foo>();
            var (a, b) = Variables.CreateMulti();

            var pattern = Reuse(() =>
                        N(a) - R[LIKE, new { p._.Id }] > N(b));

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(a)-[:LIKE { Id: $Id }]->(b)", pattern.ToString());
        }

        #endregion // (a)-[r1]->(b) / Reuse_N_R2_N_Test

        #region (a)-[r1:LIKE { Id: Id }]->(b) / Reuse_N_R3_N_Test

        [Fact]
        public void Reuse_N_R3_N_Test()
        {
            var (a, b) = Variables.CreateMulti();
            var Id = Parameters.Create();
            var pattern = Reuse(() =>
                        N(a) - R[LIKE, new { Id }] > N(b));

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal("(a)-[:LIKE { Id: $Id }]->(b)", pattern.ToString());
        }

        #endregion // (a)-[r1:LIKE { Id: Id }]->(b) / Reuse_N_R3_N_Test

        #region (a)-[r1: KIKE { Id: $Id}]->(b) / Reuse_N_R1_N_Test

        [Fact]
        public void Reuse_N_R1_N_Test()
        {
            var p = Parameters.Create<Foo>();
            var (a, r1, b) = Variables.CreateMulti();

            var pattern = Reuse(() =>
                        N(a) - R[r1, LIKE, new { p._.Id }] > N(b));

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal("(a)-[r1:LIKE { Id: $Id }]->(b)", pattern.ToString());
        }

        #endregion // (a)-[r1: KIKE { Id: $Id}]->(b) / Reuse_N_R1_N_Test

        #region (a)-[r1]->(b)<-[r2] / Reuse_Complex4_Test

        [Fact]
        public void Reuse_Complex4_Test()
        {
            var (a, r1, b, r2) = Variables.CreateMulti();
            var pattern = Reuse(() =>
                        N(a) - R[r1] > N(b) < R[r2]);

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(a)-[r1]->(b)<-[r2]", pattern.ToString());
        }

        #endregion // (a)-[r1]->(b)<-[r2] / Reuse_Complex4_Test

        #region (a)-[r1]->(b)<-[r2]-(c) / Reuse_Complex5_Test

        [Fact]
        public void Reuse_Complex5_Test()
        {
            var (a, r1, b, r2, c) = Variables.CreateMulti();
            var pattern = Reuse(() =>
                        N(a) - R[r1] > N(b) < R[r2] - N(c));

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(a)-[r1]->(b)<-[r2]-(c)", pattern.ToString());
        }

        #endregion // (a)-[r1]->(b)<-[r2]-(c) / Reuse_Complex5_Test

        #region (a)-[r1]->(b)<-[r2]-(c) / Reuse_Complex5_Broken_Test

        [Fact]
        public void Reuse_Complex5_Broken_Test()
        {
            var (a, r1, b, r2, c) = Variables.CreateMulti();
            var start = Reuse(() =>
                        N(a) - R[r1]);

            var bp = Reuse(() => N(b));
            var r2p = Reuse(() => R[r2]);

            var pattern = Reuse(() =>
                        start > bp < r2p - N(c));

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(a)-[r1]->(b)<-[r2]-(c)", pattern.ToString());
        }

        #endregion // (a)-[r1]->(b)<-[r2]-(c) / Reuse_Complex5_Broken_Test

        #region UNWIND ... MATCH(n:Person ...) MATCH(u:Maintainer ...) MERGE (u)-[:By { Date: $Date }]->(n) RETURN n / Reuse_Unwind_Arr_Test

        [Fact]
        public void Reuse_Unwind_Arr_Test()
        {
            var p = Parameters.Create<Bar>();
            var maintainer_Id = Parameters.Create();
            var (u, maintainer_, n) = Variables.CreateMulti();
            var items = Variables.Create<Foo>();

            INode user = Reuse(() => N(u, Maintainer, new { Id = maintainer_Id }));
            INode by = Reuse(() => N(u) - R[By, new { p._.Date }] > N(n));
            CypherCommand cypher =
                _(() =>
                             Unwind(items, item =>
                                //                       Id = item.Id
                                Match(N(n, Person, new { item.__.Id }), user)
                                .Merge(by)
                                .Return(n)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal($"UNWIND items AS item{NewLine}" +
                $"MATCH (n:Person {{ Id: item.Id }}), (u:Maintainer {{ Id: $maintainer_Id }}){NewLine}" +
                $"MERGE (u)-[:By {{ Date: $Date }}]->(n){NewLine}" +
                "RETURN n", cypher);
        }

        #endregion // UNWIND ... MATCH(n:Person ...) MATCH(u:Maintainer ...) MERGE (u)-[:By { Date: $Date }]->(n) RETURN n / Reuse_Unwind_Arr_Test
    }
}

