using System;
using System.Security.Cryptography;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;


namespace Weknow.Cypher.Builder
{
        [Trait("Segment", "Expression")]
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
            var reusedPerson = Reuse(person => N(person, Person));
            var reusedAnimal = Reuse(animal => N(animal, Animal));

            CypherCommand cypher = _( r =>
                          Match(reusedPerson - R[r, LIKE] > reusedAnimal));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal("MATCH (person:Person)-[r:LIKE]->(animal:Animal)", cypher.Query);
        }

        #endregion // MATCH (person:Person)-[r:LIKE]->(animal:Animal) / LazyReuse_Node_Test

        #region (n:Person { PropA: $nPropA }) ... / LazyReuse_Overloads_Test

        [Fact]
        public void LazyReuse_Overloads_Test()
        {
            IParameter? nPropA = null, n_PropA = null, n1_PropA = null, n2_PropB = null, n1_Id = null, n2_PropA = null, n3_PropB = null, n4_PropC = null;
            IPattern? pattern1 = Reuse(n => N(n, Person, new { PropA = nPropA }));
            IPattern? pattern2 = Reuse(n => N(n, Person, new { PropA = n_PropA }));
            IPattern? pattern3 = Reuse(n => N(n, Person, new { PropA = n1_PropA, PropB = n2_PropB }));
            IPattern? pattern4 = Reuse(n => N(n, Person, new { Id = n1_Id, PropA = n2_PropA, PropB = n3_PropB }));
            IPattern? pattern5 = Reuse(n => N(n, Person, new { Id = n1_Id, PropA = n2_PropA, PropB = n3_PropB, PropC = n4_PropC }));

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
            var pattern = Reuse(n => R[LIKE]);

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"[:LIKE]", pattern.ToString());
        }

        #endregion // [:LIKE] / Reuse_Relation_Test

        #region (n:Person:Animal)-[:LIKE] / Reuse_Node_And_Relation_Test

        [Fact]
        public void Reuse_Node_And_Relation_Test()
        {
            var pattern = Reuse(n => N(n, Person & Animal) - R[LIKE] );

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(n:Person:Animal)-[:LIKE]", pattern.ToString());
        }

        #endregion // (n:Person:Animal)-[:LIKE] / Reuse_Node_And_Relation_Test

        #region (a)-[r1]->(b) / Reuse_N_R_N_Test

        [Fact]
        public void Reuse_N_R_N_Test()
        {
            var pattern = Reuse(a => r1 => b => r2 =>
                        N(a) - R[r1] > N(b));

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(a)-[r1]->(b)", pattern.ToString());
        }

        #endregion // (a)-[r1]->(b) / Reuse_N_R_N_Test

        #region (a)-[r1]->(b) / Reuse_N_R2_N_Test

        [Fact]
        public void Reuse_N_R2_N_Test()
        {
            var pattern = Reuse(a => b => r2 =>
                        N(a) - R[LIKE, Id] > N(b));

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(a)-[:LIKE { Id: $Id }]->(b)", pattern.ToString());
        }

        #endregion // (a)-[r1]->(b) / Reuse_N_R2_N_Test

        #region (a)-[r1:LIKE { Id: Id }]->(b) / Reuse_N_R3_N_Test

        [Fact]
        public void Reuse_N_R3_N_Test()
        {
            var pattern = Reuse(a => b => r2 =>
                        N(a) - R[LIKE, Id] > N(b));

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal("(a)-[:LIKE { Id: $Id }]->(b)", pattern.ToString());
        }

        #endregion // (a)-[r1:LIKE { Id: Id }]->(b) / Reuse_N_R3_N_Test

        #region (a)-[r1: KIKE { Id: $Id}]->(b) / Reuse_N_R1_N_Test

        [Fact]
        public void Reuse_N_R1_N_Test()
        {
            var pattern = Reuse(a => r1 => b => r2 =>
                        N(a) - R[r1, LIKE, Id] > N(b));

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal("(a)-[r1:LIKE { Id: $Id }]->(b)", pattern.ToString());
        }

        #endregion // (a)-[r1: KIKE { Id: $Id}]->(b) / Reuse_N_R1_N_Test

        #region (a)-[r1]->(b)<-[r2] / Reuse_Complex4_Test

        [Fact]
        public void Reuse_Complex4_Test()
        {
            var pattern = Reuse(a => r1 => b => r2 =>
                        N(a) - R[r1] > N(b) < R[r2]);

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(a)-[r1]->(b)<-[r2]", pattern.ToString());
        }

        #endregion // (a)-[r1]->(b)<-[r2] / Reuse_Complex4_Test

        #region (a)-[r1]->(b)<-[r2]-(c) / Reuse_Complex5_Test

        [Fact]
        public void Reuse_Complex5_Test()
        {
            var pattern = Reuse(a => r1 => b => r2 => c =>
                        N(a) - R[r1] > N(b) < R[r2] - N(c));

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(a)-[r1]->(b)<-[r2]-(c)", pattern.ToString());
        }

        #endregion // (a)-[r1]->(b)<-[r2]-(c) / Reuse_Complex5_Test

        #region (a)-[r1]->(b)<-[r2]-(c) / Reuse_Complex5_Broken_Test

        [Fact]
        public void Reuse_Complex5_Broken_Test()
        {
            var start = Reuse(a => r1  =>
                        N(a) - R[r1]);

            var b = Reuse(b  => N(b));
            var r2 = Reuse(r2  => R[r2]);

            var pattern = Reuse(c =>
                        start > b < r2 - N(c));

            _outputHelper.WriteLine(pattern.ToString());
            Assert.Equal(@"(a)-[r1]->(b)<-[r2]-(c)", pattern.ToString());
        }

        #endregion // (a)-[r1]->(b)<-[r2]-(c) / Reuse_Complex5_Broken_Test

        #region UNWIND ... MATCH(n:Person ...) MATCH(u:Maintainer ...) MERGE (u)-[:By { Date: $Date }]->(n) RETURN n / Reuse_Unwind_Test

        [Fact]
        public void Reuse_Unwind_Test()
        {
            IParameter? maintainer_Id = null;
            INode user = Reuse(u => maintainer_ => N(u, Maintainer, new { Id = maintainer_Id }));
            INode by = Reuse(u => n => N(u) - R[By, Date] > N(n));
            CypherCommand cypher =
                _<Foo>(n => items => item => u => maintainer_ =>
                             Unwind(items, item, 
                                Match(N(n, Person, item._(n._.Id)))
                                .Match(user)
                                .Merge(by)
                                .Return(n)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("UNWIND $items AS item\r\n" +
                "MATCH (n:Person { Id: item.Id })\r\n" +
                "MATCH (u:Maintainer { Id: $maintainer_Id })\r\n" +
                "MERGE (u)-[:By { Date: $Date }]->(n)\r\n" +
                "RETURN n", cypher);
        }

        #endregion // UNWIND ... MATCH(n:Person ...) MATCH(u:Maintainer ...) MERGE (u)-[:By { Date: $Date }]->(n) RETURN n / Reuse_Unwind_Test

        #region UNWIND ... MATCH(n:Person ...) MATCH(u:Maintainer ...) MERGE (u)-[:By { Date: $Date }]->(n) RETURN n / Reuse_Unwind_Arr_Test

        [Fact]
        public void Reuse_Unwind_Arr_Test()
        {
            IParameter? maintainer_Id = null;
            INode user = Reuse(u => maintainer_ => N(u, Maintainer, new { Id = maintainer_Id }));
            INode by = Reuse(u => n => N(u) - R[By, Date] > N(n));
            CypherCommand cypher =
                _<Foo>(n => items => item => u => maintainer_ =>
                             Unwind(items, item,
                                Match(N(n, Person, item._(n._.Id)), user)
                                .Merge(by)
                                .Return(n)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("UNWIND $items AS item\r\n" +
                "MATCH (n:Person { Id: item.Id }), (u:Maintainer { Id: $maintainer_Id })\r\n" +
                "MERGE (u)-[:By { Date: $Date }]->(n)\r\n" +
                "RETURN n", cypher);
        }

        #endregion // UNWIND ... MATCH(n:Person ...) MATCH(u:Maintainer ...) MERGE (u)-[:By { Date: $Date }]->(n) RETURN n / Reuse_Unwind_Arr_Test
    }
}

