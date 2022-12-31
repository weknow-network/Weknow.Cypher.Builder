using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder;

[Trait("TestType", "Unit")]
[Trait("Group", "Phrases")]

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
    public void Unwind_FuncT_Test()
    {
        var items = Parameters.Create<Foo>();
        var n = Variables.Create();

        CypherCommand cypher = _(() =>
                                Unwind(items, item =>
                                    Match(N(n, Person, new { item.__.PropA, item.__.PropB })))
                                .Unwind(items, item =>
                                    Match(N(n, Person, new { item.__.PropA, item.__.PropB })))
                                );

        _outputHelper.WriteLine(cypher);
        Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })
UNWIND $items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
    }

    #endregion // UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, .. }) 

    #region UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, .. }) 

    [Fact]
    public void Unwind_Func_Test()
    {
        var items = Parameters.Create();
        var n = Variables.Create();

        CypherCommand cypher = _(() =>
                                Unwind(items, item =>
                                    Match(N(n, Person, new { item.__<Foo>().PropA, item.__<Foo>().PropB })))
                                .Unwind(items, item =>
                                    Match(N(n, Person, new { item.__<Foo>().PropA, item.__<Foo>().PropB })))
                                );

        _outputHelper.WriteLine(cypher);
        Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })
UNWIND $items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
    }

    #endregion // UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, .. }) 

    #region UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, .. }) 

    [Fact]
    public void Unwind_FuncT_Var_Test()
    {
        var items = Variables.Create<Foo>();
        var n = Variables.Create();

        CypherCommand cypher = _(() =>
                                Unwind(items, item =>
                                    Match(N(n, Person, new { item.__.PropA, item.__.PropB })))
                                .Unwind(items, item =>
                                    Match(N(n, Person, new { item.__.PropA, item.__.PropB })))
                                );

        _outputHelper.WriteLine(cypher);
        Assert.Equal(@"UNWIND items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })
UNWIND items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
    }

    #endregion // UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, .. }) 

    #region UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, .. }) 

    [Fact]
    public void Unwind_Func_Var_Test()
    {
        var items = Variables.Create();
        var n = Variables.Create();

        CypherCommand cypher = _(() =>
                                Unwind(items, item =>
                                    Match(N(n, Person, new { item.__<Foo>().PropA, item.__<Foo>().PropB })))
                                .Unwind(items, item =>
                                    Match(N(n, Person, new { item.__<Foo>().PropA, item.__<Foo>().PropB })))
                                );

        _outputHelper.WriteLine(cypher);
        Assert.Equal(@"UNWIND items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })
UNWIND items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
    }

    #endregion // UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, .. }) 

    #region UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, .. }) 

    [Fact]
    public void Unwind_Test()
    {
        var items = Parameters.Create();
        var item = Variables.Create<Foo>();
        var n = Variables.Create();

        CypherCommand cypher = _(() =>
                                Unwind(items, item,
                                Match(N(n, Person, new { item.__.PropA, item.__.PropB }))));

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
        Assert.Equal(@"UNWIND items AS item
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
        CypherCommand cypher = _(n => map => Unwind(items, map,
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
    public void Unwind_Entities_Update_Inline_Test()
    {
        var items = Parameters.Create();
        var item = Variables.Create<Foo>();

        CypherCommand cypher = _(n =>
                                Unwind(items, item,
                                Match(N(n, Person, new { item.__.Id }))
                                .SetPlus(n, item)));

        _outputHelper.WriteLine(cypher);
        Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { Id: item.Id })
SET n += item", cypher.Query);
    }

    #endregion // UNWIND $items AS item MATCH (n:Person { Id: item.Id }) SET n += item 

    #region UNWIND $items AS item MATCH (n:Person { Id: item.Id }) SET n += item

    [Fact]
    public void Unwind_Entities_Update_Test()
    {
        var items = Parameters.Create();
        var n = Variables.Create();
        var item = Variables.Create<Foo>();

        CypherCommand cypher = _(() =>
                                Unwind(items, item,
                                Match(N(n, Person, new { item.__.Id }))
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
                                Merge(N(n, Person, new { map.__.Id }))
                                .OnCreateSet(n, map)
                                .Return(n)),
                                cfg => cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE);

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
                               Merge(N(n, Person, new { map.__.Id }))
                               .OnCreateSet(n, map)
                               .Return(n)),
                                cfg => cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE);

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
                               Merge(N(n, Person, new { map.__.Id, map.__.Name }))
                               .OnCreateSet(n, map)
                               .Return(n)),
                                cfg => cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE);

        _outputHelper.WriteLine(cypher);

        // Require remodel of the cypher generator,
        // On the remodeling it would be nice to add built-in indentation
        Assert.Equal($"UNWIND $items AS map{NewLine}" +
            $"MERGE (n:PERSON {{ Id: map.Id, Name: map.Name }}){NewLine}\t" +
            $"ON CREATE SET n = map{NewLine}" +
            "RETURN n", cypher.Query);
    }

    #endregion // UNWIND $items AS map MERGE (n:PERSON {.. }) ON CREATE SET n = map ..

    #region UNWIND $items AS map MERGE (n:PERSON {..}) ON CREATE SET n = map ..t

    [Fact]
    public void Unwind_Create_OnCreateSet_Gen_Map_MultiParam_Fluent_Test()
    {
        var items = Parameters.Create();
        var map = Variables.Create<Foo>();

        CypherCommand cypher = _(n =>
                               Unwind(items, map,
                               Merge(N(n, Person, new { map.__.Id, map.__.Name }))
                               .OnCreateSet(n, map)
                               .Return(n)),
                                cfg => cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE);

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
    public void Unwind_Array_Func_Test()
    {
        var items = Parameters.Create();
        var n = Variables.Create();
        var (num, txt) = Variables.CreateMulti();

        CypherCommand cypher = _(() =>
                                Unwind(new[] { 1, 2, 3 }, num =>
                                Unwind(new[] { "a", "b" }, txt =>
                                Return(num, txt))),
                                cfg => cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE);

        _outputHelper.WriteLine(cypher);

        Assert.Equal(
            $"UNWIND $p_0 AS num{NewLine}" +
            $"UNWIND $p_1 AS txt{NewLine}" +
            $"RETURN num, txt", cypher.Query);
        Assert.True(new[] { 1, 2, 3 }.SequenceEqual(cypher.Parameters.Get<IEnumerable<int>>("p_0")));
        Assert.True(new[] { "a", "b" }.SequenceEqual(cypher.Parameters.Get<IEnumerable<string>>("p_1")));
    }

    #endregion // UNWIND [1,2,3] as num RETURN num

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
                                cfg => cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE);

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
                                cfg => cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE);

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
    public void Unwind_Param_WithoutMap_Inline_Test()
    {
        var (items, Date) = Parameters.CreateMulti();

        CypherCommand cypher = _(map => n => m =>
                                Unwind(items, map,
                                Merge(N(n, Person, new { map.__<Foo>().Id }) -
                                      R[By] >
                                      N(m, Maintainer,
                                             new { m.Prm._<Foo>().Id, Date }))),
                                cfg => cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE);

        _outputHelper.WriteLine(cypher);

        // Require remodel of the cypher generator,
        // On the remodeling it would be nice to add built-in indentation
        Assert.Equal($"UNWIND $items AS map{NewLine}" +
            $"MERGE (n:PERSON {{ Id: map.Id }})-[:By]->(m:MAINTAINER {{ Id: $Id, Date: $Date }})", cypher.Query);
    }

    #endregion // UNWIND $items AS map MERGE (n:PERSON {..})-[:By]->(m:USER {..})

    #region UNWIND $items AS map MERGE (n:PERSON {..})-[:By]->(m:USER {..})

    [Fact]
    public void Unwind_Param_WithoutMap_Reuse_Test()
    {
        var (items, Id, Date) = Parameters.CreateMulti();
        var map = Variables.Create<Foo>();
        var m = Variables.Create();

        var maintainerPattern = Reuse(() => R[By] > N(m, Maintainer, new { Id, Date }));

        CypherCommand cypher = _(n =>
                                Unwind(items, map,
                                Merge(N(n, Person, new { map.__.Id }) - maintainerPattern)),
                                cfg => cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE);

        _outputHelper.WriteLine(cypher);

        // Require remodel of the cypher generator,
        // On the remodeling it would be nice to add built-in indentation
        Assert.Equal($"UNWIND $items AS map{NewLine}" +
            $"MERGE (n:PERSON {{ Id: map.Id }})-[:By]->(m:MAINTAINER {{ Id: $Id, Date: $Date }})", cypher.Query);
    }

    #endregion // UNWIND $items AS map MERGE (n:PERSON {..})-[:By]->(m:USER {..})

    // TODO: [bnaya 2022-11-02] The following (With_Test)
    // CypherCommand cypherOfFriends = _(() =>
    //                  Match(N(user, Person))
    //                  .Where(user._.key == u.key)
    //                  .With(user)
    //                  .Unwind(friends.AsParameter, map,
    //                       .Create(N(friend, Friend, new { key = map.__.key }))
    //                       .Merge(N(user) - R[Knows] > N(friend))
    //                         .Set(friend, map)));

    #region UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, .. }) 

    [Fact]
    public void Unwind_FuncT_Ambient_Test()
    {
        var items = Parameters.Create<Foo>();

        CypherCommand cypher = _(() =>
                                Unwind(items, item =>
                                    Match(N(item, Person, new { item.__.PropA, item.__.PropB })))
                                .Unwind(items, item1 =>
                                    Match(N(item1, Person, new { item1.__.PropA, item1.__.PropB })))
                                 ,cfg =>
                                 {
                                     cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                                     cfg.AmbientLabels.Add(Prod);
                                     cfg.Flavor = CypherFlavor.OpenCypher;
                                 });

        _outputHelper.WriteLine(cypher);
        Assert.Equal(@"UNWIND $items AS item
MATCH (item:PROD:PERSON { PropA: item.PropA, PropB: item.PropB })
UNWIND $items AS item1
MATCH (item1:PROD:PERSON { PropA: item1.PropA, PropB: item1.PropB })", cypher.Query);
    }

    #endregion // UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, .. }) 

    #region UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, .. }) 

    [Fact (Skip = "Should be fixed")]
    public void Unwind_FuncT_Ambient_String_Test()
    {
        var items = Parameters.Create<Foo>();

        CypherCommand cypher = _(() =>
                                Unwind(items, item =>
                                    Match(N(item, Person, new { item.__.PropA, item.__.PropB })))
                                .Unwind(items, item1 =>
                                    Match(N(item1, Person, new { item1.__.PropA, item1.__.PropB })))
                                .IgnoreAmbient(
                                    Unwind(items, item2 =>
                                        Match(N(item2, Person, new { item2.__.PropA, item2.__.PropB })))
                                )
                                 ,cfg =>
                                 {
                                     cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                                     cfg.AmbientLabels.Add($"Prod");
                                     cfg.Flavor = CypherFlavor.Neo4j5;
                                 });

        _outputHelper.WriteLine(cypher);
        Assert.Equal(@"UNWIND $items AS item
MATCH (item:PROD&PERSON { PropA: item.PropA, PropB: item.PropB })
UNWIND $items AS item1
MATCH (item1:PROD&PERSON { PropA: item1.PropA, PropB: item1.PropB })
UNWIND $items AS item2
MATCH (item2&PERSON { PropA: item2.PropA, PropB: item2.PropB })", cypher.Query);
    }

    #endregion // UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, .. }) 

}

