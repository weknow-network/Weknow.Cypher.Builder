using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;
using static Weknow.GraphDbCommands.Schema;
using static System.Environment;

namespace Weknow.GraphDbCommands
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Phrases")]
    [Trait("Segment", "Expression")]
    public class WithTests
    {
        private readonly ITestOutputHelper _outputHelper;
        record KeyedEntity(int key);

        #region Ctor

        public WithTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MERGE (n:PERSON { Id: $Id }) WITH * MATCH (i:PERSON { Id: $Id }) RETURN i.Name

        [Fact]
        public void With_Test()
        {
            var Id = Parameters.Create();
            var n = Variables.Create();
            var i = Variables.Create<Foo>(); ;

            CypherCommand cypher = _(() =>
                        Merge(N(n, Person, new { Id }))
                        .With()
                        .Match(N(i, Person, new { Id }))
                        .Return(i._.Name),
                        cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);
            ;

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
                $"MERGE (n:PERSON {{ Id: $Id }}){NewLine}" +
                $"WITH *{NewLine}" +
                $"MATCH (i:PERSON {{ Id: $Id }}){NewLine}" +
                "RETURN i.Name", cypher.Query);
        }

        #endregion // MERGE (n:PERSON { Id: $Id }) WITH * MATCH (i:PERSON { Id: $Id }) RETURN i.Name 

        #region MERGE (n:PERSON { Id: $Id }) WITH n MATCH (i:PERSON { Id: $Id }) RETURN i.Name

        [Fact]
        public void With_Param_Test()
        {
            var n = Variables.Create();
            var i = Variables.Create<Foo>(); 
            var Id = Parameters.Create();

            CypherCommand cypher = _(() =>
                        Merge(N(n, Person, new { Id }))
                        .With(n)
                        .Match(N(i, Person, new { Id }))
                        .Return(i._.Name),
                        cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);
            ;

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
                $"MERGE (n:PERSON {{ Id: $Id }}){NewLine}" +
                $"WITH n{NewLine}" +
                $"MATCH (i:PERSON {{ Id: $Id }}){NewLine}" +
                "RETURN i.Name", cypher.Query);
        }

        #endregion // MERGE (n:PERSON { Id: $Id }) WITH n MATCH (i:PERSON { Id: $Id }) RETURN i.Name

        #region UNWIND ... MERGE (n:PERSON ...) WITH * MATCH (i:PERSON ...}) RETURN n 

        [Fact]
        public void With_Complex_Test()
        {
            var items = Parameters.Create();
            var (n, i) = Variables.CreateMulti();
            var map = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                        Unwind(items, map,
                        Merge(N(n, Person, new { (~map)._.Id }))
                        .OnCreateSet(n, map)
                        .OnMatchSetPlus(n, map)
                        .With()
                        .Match(N(i, Person, new { (~map)._.Id }))
                        .Return(n)),
                        cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);
            ;

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
                $"UNWIND $items AS map{NewLine}" +
                $"MERGE (n:PERSON {{ Id: map.Id }}){NewLine}\t" +
                    $"ON CREATE SET n = map{NewLine}\t" +
                    $"ON MATCH SET n += map{NewLine}" +
                $"WITH *{NewLine}" +
                $"MATCH (i:PERSON {{ Id: map.Id }}){NewLine}" +
                "RETURN n", cypher.Query);
        }

        #endregion // UNWIND ... MERGE (n:PERSON ...) WITH * MATCH (i:PERSON ...}) RETURN n 

        #region UNWIND ... MERGE (n:PERSON ...) WITH n, map MATCH (i:PERSON ...}) RETURN i / With_Params_Complex_Test

        [Fact]
        public void With_Params_Complex_Test()
        {
            var items = Parameters.Create();
            var (n, i) = Variables.CreateMulti();
            var map = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                        Unwind(items, map,
                        Merge(N(n, Person, new { (~map)._.Id }))
                        .OnCreateSet(n, map)
                        .OnMatchSetPlus(n, map)
                        .With(n, map)
                        .Match(N(i, Person, new { (~map)._.Id }))
                        .Return(i)),
                        cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
                $"UNWIND $items AS map{NewLine}" +
                $"MERGE (n:PERSON {{ Id: map.Id }}){NewLine}\t" +
                    $"ON CREATE SET n = map{NewLine}\t" +
                    $"ON MATCH SET n += map{NewLine}" +
                $"WITH n, map{NewLine}" +
                $"MATCH (i:PERSON {{ Id: map.Id }}){NewLine}" +
                "RETURN i", cypher.Query);
        }

        #endregion // UNWIND ... MERGE (n:PERSON ...) WITH n, map MATCH (i:PERSON ...}) RETURN i / With_Complex_Test

        #region MATCH (user:PERSON) WITH user UNWIND $friends AS map MERGE (friend:FRIEND) MERGE (user)-[:KNOWS]->(friend)

        [Fact]
        public void With_Unwind_Test()
        {
            CypherConfig.Scope.Value = cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;

            var users = Parameters.Create();
            var (user, friend, friends) = Variables.CreateMulti();
            var map = Variables.Create<KeyedEntity>();

            CypherCommand cypher = _(() =>
                                    Match(N(user, Person))
                                    .With(user)
                                    .Unwind(friends.AsParameter, map,
                                         Merge(N(friend, Friend, new { key = (~map)._.key }))
                                            .Set(friend, map)
                                         .Merge(N(user) - R[KNOWS] > N(friend))));

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
                $"MATCH (user:PERSON){NewLine}" +
                $"WITH user{NewLine}" +
                $"UNWIND $friends AS map{NewLine}" +
                $"MERGE (friend:FRIEND {{ key: map.key }}){NewLine}" +
                $"SET friend = map{NewLine}" +
                $"MERGE (user)-[:KNOWS]->(friend)", cypher.Query);
        }

        #endregion // MATCH (user:PERSON) WITH user UNWIND $friends AS map MERGE (friend:FRIEND) MERGE (user)-[:KNOWS]->(friend)

        #region MATCH (user:PERSON)-[:KNOWS]->(friend:FRIEND) WITH user, count(friend) AS friends .. RETURN user

        [Fact]
        public void With_Count_Test()
        {
            CypherConfig.Scope.Value = cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;

            var users = Parameters.Create();
            var friends = Variables.Create();
            var userName = Parameters.Create<string>();
            var (user, friend, map) = Variables.CreateMulti();

            CypherCommand cypher = _(() =>
                                    Match(N(user, Person) - R[KNOWS] > N(friend, Friend))
                                    .With(user, friend.Count().As(friends))
                                    .Where(friends > 5)
                                    .Return(user));

            _outputHelper.WriteLine(cypher.Dump());
			 Assert.Equal(
                $"MATCH (user:PERSON)-[:KNOWS]->(friend:FRIEND){NewLine}" +
                $"WITH user, count(friend) AS friends{NewLine}" +
                $"WHERE friends > $p_0{NewLine}" +
                "RETURN user", cypher.Query);
        }

        #endregion // MATCH (user:PERSON)-[:KNOWS]->(friend:FRIEND) WITH user, count(friend) AS friends .. RETURN user
    }
}

