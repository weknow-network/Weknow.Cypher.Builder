using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Phrases")]
    [Trait("Segment", "Expression")]
    public class UnwindTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public UnwindTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, .. }) 

        [Fact]
        public void Unwind_Test()
        {
            var items = Parameters.Create();
            var item = Variables.Create<Foo>();
            var n = Variables.Create();

            CypherCommand cypher = _(() =>
                                    Unwind(items, item,
                                    Match(N(n, Person, new { (~item)._.PropA, (~item)._.PropB }))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, .. }) 

        #region UNWIND $items AS item MATCH (n:Person { PropA: item })

        [Fact]
        public void Unwind_PropConst_AsMap_Test()
        {
            CypherCommand cypher = _(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, new { PropA = item }))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { PropA: item })", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { PropA: item })

        #region UNWIND $items AS map CREATE (n:Person) SET n = map

        [Fact]
        public void Unwind_Create_Map_Test()
        {
            var items = Parameters.Create();
            var (map, n) = Variables.CreateMulti();
            CypherCommand cypher = _(() => Unwind(items, map,
                                            Create(N(n, Person))
                                            .Set(n, map)));

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
            Assert.Equal($"UNWIND $items AS map{NewLine}" +
                 $"CREATE (n:Person){NewLine}" +
                 "SET n = map", cypher.Query);
        }

        #endregion // UNWIND $items AS map CREATE (n:Person) SET n = map

        #region UNWIND $items AS map CREATE (n:Person map

        [Fact]
        public void Unwind_Create_AsMap_Test()
        {
            var items = Parameters.Create();
            var (map, n) = Variables.CreateMulti();

            CypherCommand cypher = _(() => Unwind(items, map,
                                            Create(N(n, Person, map))));

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
            Assert.Equal($"UNWIND $items AS map{NewLine}" +
                 "CREATE (n:Person map)", cypher.Query);
        }

        #endregion // UNWIND $items AS map CREATE (n:Person map

        #region UNWIND $items AS map CREATE (n:Person) SET n = map

        [Fact]
        public void Unwind_Create_SET_AsMap_Test()
        {
            var items = Parameters.Create();
            var (map, n) = Variables.CreateMulti();

            CypherCommand cypher = _(() => Unwind(items, map,
                                            Create(N(n, Person))
                                            .Set(n, map)));

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
            Assert.Equal($"UNWIND $items AS map{NewLine}" +
                 $"CREATE (n:Person){NewLine}" +
                 "SET n = map", cypher.Query);
        }

        #endregion // UNWIND $items AS map CREATE (n:Person) SET n = map

        #region UNWIND $items AS map CREATE (n:Person) SET n = map RETURN n

        [Fact]
        public void Unwind_Create_Set_Map_Test()
        {
            var items = Parameters.Create();
            var (n, map) = Variables.CreateMulti();
            CypherCommand cypher = _(() => Unwind(items, map,
                                        Create(N(n, Person))
                                        .Set(n, map)
                                        .Return(n)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"UNWIND $items AS map
CREATE (n:Person)
SET n = map
RETURN n", cypher.Query);
        }

        #endregion // UNWIND $items AS map CREATE (n:Person) SET n = map RETURN n

        #region UNWIND $items AS item MATCH (n:Person { Id: item.Id }) SET n += item

        [Fact]
        public void Unwind_Entities_Update_Test()
        {
            var items = Parameters.Create();
            var n = Variables.Create();
            var item = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Unwind(items, item,
                                    Match(N(n, Person, new { (~item)._.Id }))
                                    .SetPlus(n, item)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { Id: item.Id })
SET n += item", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { Id: item.Id }) SET n += item 

        #region UNWIND $items AS map MERGE (n:PERSON { Id: map.Id }) ON CREATE SET n = map ..

        [Fact]
        public void Unwind_Create_OnCreateSet_Map_Test()
        {
            var items = Parameters.Create();
            var n = Variables.Create();
            var map = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Unwind(items, map,
                                    Merge(N(n, Person, new { (~map)._.Id }))
                                    .OnCreateSet(n, map)
                                    .Return(n)),
                                    cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
            Assert.Equal($"UNWIND $items AS map{NewLine}" +
                $"MERGE (n:PERSON {{ Id: map.Id }}){NewLine}\t" +
                $"ON CREATE SET n = map{NewLine}" +
                "RETURN n", cypher.Query);
        }

        #endregion // UNWIND $items AS map MERGE (n:PERSON { Id: map.Id }) ON CREATE SET n = map .. 

        #region UNWIND $items AS map MERGE (n:PERSON { Id: map.Id }) ON CREATE SET n = map ..

        [Fact]
        public void Unwind_Create_OnCreateSet_Mix_Map_Test()
        {
            var items = Parameters.Create();
            var n = Variables.Create();
            var map = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                   Unwind(items, map,
                                   Merge(N(n, Person, new { (~map)._.Id }))
                                   .OnCreateSet(n, map)
                                   .Return(n)),
                                    cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
            Assert.Equal($"UNWIND $items AS map{NewLine}" +
                $"MERGE (n:PERSON {{ Id: map.Id }}){NewLine}\t" +
                $"ON CREATE SET n = map{NewLine}" +
                "RETURN n", cypher.Query);
        }

        #endregion // UNWIND $items AS map MERGE (n:PERSON { Id: map.Id }) ON CREATE SET n = map ..

        #region UNWIND $items AS map MERGE (n:PERSON {..}) ON CREATE SET n = map ..t

        [Fact]
        public void Unwind_Create_OnCreateSet_Gen_Map_MultiParam_Test()
        {
            var items = Parameters.Create();
            var n = Variables.Create();
            var map = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                   Unwind(items, map,
                                   Merge(N(n, Person, new { (~map)._.Id, (~map)._.Name }))
                                   .OnCreateSet(n, map)
                                   .Return(n)),
                                    cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
            Assert.Equal($"UNWIND $items AS map{NewLine}" +
                $"MERGE (n:PERSON {{ Id: map.Id, Name: map.Name }}){NewLine}\t" +
                $"ON CREATE SET n = map{NewLine}" +
                "RETURN n", cypher.Query);
        }

        #endregion // UNWIND $items AS map MERGE (n:PERSON {.. }) ON CREATE SET n = map ..

        #region UNWIND [1,2,3] as num RETURN num

        [Fact(Skip = "Not implemented")]
        public void Unwind_Array_Test()
        {
            var items = Parameters.Create();
            var n = Variables.Create();
            var (num, txt) = Variables.CreateMulti();

            CypherCommand cypher = _(() =>
                                    Unwind(new[] { 1, 2, 3 }, num,
                                    Unwind(new[] { "a", "b" }, txt,
                                    Return(num, txt))),
                                    cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);

            _outputHelper.WriteLine(cypher);

            Assert.Equal($"UNWIND $p_0 AS num{NewLine}" +
                $"RETURN num", cypher.Query);
            Assert.True(new[] { 1, 2, 3 }.SequenceEqual(cypher.Parameters.Get<IEnumerable<int>>("p_0")));
        }

        #endregion // UNWIND [1,2,3] as num RETURN num

        #region UNWIND [1,2,3] as num UNWIND['a', 'b'] as txt RETURN num, txt

        [Fact(Skip = "Not implemented")]
        public void Unwind_Unwind_Test()
        {
            var items = Parameters.Create();
            var n = Variables.Create();
            var (num, txt) = Variables.CreateMulti();

            CypherCommand cypher = _(() =>
                                    Unwind(new[] { 1, 2, 3 }, num,
                                    Unwind(new[] { "a", "b" }, txt,
                                    Return(num, txt))),
                                    cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);

            _outputHelper.WriteLine(cypher);

            Assert.Equal($"UNWIND $p_0 AS num{NewLine}" +
                $"UNWIND $p_1 AS txt{NewLine}" +
                $"RETURN num, txt", cypher.Query);
            Assert.True(new[] { 1, 2, 3 }.SequenceEqual(cypher.Parameters.Get<IEnumerable<int>>("p_0")));
            Assert.True(new[] { "a", "b" }.SequenceEqual(cypher.Parameters.Get<IEnumerable<string>>("p_1")));
        }

        #endregion // UNWIND [1,2,3] as num UNWIND['a', 'b'] as txt RETURN num, txt

        #region UNWIND $items AS map MERGE (n:PERSON {..})-[:By]->(m:USER {..})

        [Fact]
        public void Unwind_Param_WithoutMap_Test()
        {
            var items = Parameters.Create();
            var n = Variables.Create();
            var map = Variables.Create<Foo>();

            var maintainer_ = Variables.Create();
            var (maintainer_Id, Date) = Parameters.CreateMulti();
            var maintainer = Reuse(() => R[By] > N(maintainer_, Maintainer, new { Id = maintainer_Id, Date = Date }));

            CypherCommand cypher = _(() =>
                                    Unwind(items, map,
                                    Merge(N(n, Person, new { (~map)._.Id }) - maintainer)),
                                    cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
            Assert.Equal($"UNWIND $items AS map{NewLine}" +
                $"MERGE (n:PERSON {{ Id: map.Id }})-[:By]->(maintainer_:MAINTAINER {{ Id: $maintainer_Id, Date: $Date }})", cypher.Query);
        }

        #endregion // UNWIND $items AS map MERGE (n:PERSON {..})-[:By]->(m:USER {..})

        // TODO: [bnaya 2022-11-02] The following (With_Test)
        // CypherCommand cypherOfFriends = _(() =>
        //                  Match(N(user, Person))
        //                  .Where(user._.key == u.key)
        //                  .With(user)
        //                  .Unwind(friends.AsParameter, map,
        //                       .Create(N(friend, Friend, new { key = (~map)._.key }))
        //                       .Merge(N(user) - R[Knows] > N(friend))
        //                         .Set(friend, map)));

    }
}

