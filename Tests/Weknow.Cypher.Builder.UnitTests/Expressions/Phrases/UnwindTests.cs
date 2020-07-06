using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
    [Trait("Category", "Unwind")]
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

        #region Unwind_WithPropConvention_Test

        [Fact]
        public void Unwind_WithPropConvention_Test()
        {
            CypherCommand cypher = _(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, Convention<Foo>(name => name.StartsWith("Prop"))))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
        }

        #endregion // Unwind_WithPropConvention_Test

        #region Unwind_Test

        [Fact]
        public void Unwind_Test()
        {
            CypherCommand cypher = _(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, P(PropA, PropB)))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
        }

        #endregion // Unwind_Test

        #region Unwind_Create_Map_Test

        [Fact]
        public void Unwind_Create_Map_Test()
        {
            CypherCommand cypher = _(items => map => n =>
                                        Unwind(items, map,
                                            Create(N(n, Person))
                                            .Set(n, map)));

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
			Assert.Equal("UNWIND $items AS map\r\n" +
                 "CREATE (n:Person)\r\n" +
                 "SET n = map", cypher.Query);
        }

        #endregion // Unwind_Create_Map_Test

        #region Unwind_Create_AsMap_Test

        [Fact]
        public void Unwind_Create_AsMap_Test()
        {
            CypherCommand cypher = _(items => map => n =>
                                        Unwind(items, map,
                                            Create(N(n, Person))
                                            .Set(n, map.AsMap)));

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
			Assert.Equal("UNWIND $items AS map\r\n" +
                 "CREATE (n:Person)\r\n" +
                 "SET n = map", cypher.Query);
        }

        #endregion // Unwind_Create_AsMap_Test

        #region Unwind_Create_Set_Map_Test

        [Fact]
        public void Unwind_Create_Set_Map_Test()
        {
            CypherCommand cypher = _(items => map => n =>
                                    Unwind(items, map,
                                        Create(N(n, Person))
                                        .Set(n, map)
                                        .Return(n)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(@"UNWIND $items AS map
CREATE (n:Person)
SET n = map
RETURN n", cypher.Query);
        }

        #endregion // Unwind_Create_Set_Map_Test

        #region Unwind_ShouldUseDefaultPlurality_Test

        [Fact]
        public void Unwind_ShouldUseDefaultPlurality_Test()
        {
            CypherCommand cypher = _(items => n =>
                                    Unwind(items, // TODO: should use plurality
                                    Match(N(n, Person, P(PropA, PropB)))));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
        }

        #endregion // Unwind_ShouldUseDefaultPlurality_Test

        #region Unwind_ShouldUseCustomPlurality_Test

        [Fact]
        public void Unwind_ShouldUseCustomPlurality_Test()
        {
            CypherCommand cypher = _(items => n =>
                                    Unwind(items, // TODO: should use plurality
                                    Match(N(n, Person, P(PropA, PropB)))),
                                    cfg => cfg.Naming.SetPluralization(
                                                n => n switch
                                                {
                                                    "unit" => "items",
                                                    _ => $"{n}s"
                                                },
                                                n => n switch
                                                {
                                                    "items" => "unit",
                                                    _ => $"unitOf{n}"
                                                }));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(@"UNWIND $items AS unit
MATCH (n:Person { PropA: unit.PropA, PropB: unit.PropB })", cypher.Query);
        }

        #endregion // Unwind_ShouldUseCustomPlurality_Test

        #region Unwind_Entities_Update_Test

        [Fact]
        public void Unwind_Entities_Update_Test()
        {
            CypherCommand cypher = _(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, P(Id)))
                                    .Set(+n, item)));

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { Id: item.Id })
SET n += item", cypher.Query);
        }

        #endregion // Unwind_Entities_Update_Test

        #region Unwind_Entities_Replace_Test

        [Fact]
        public void Unwind_Entities_Replace_Test()
        {
            CypherCommand cypher = _(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, P(Id)))
                                    .Set(n, item))); // + should be unary operator of IVar

            _outputHelper.WriteLine(cypher);
			 Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { Id: item.Id })
SET n = item", cypher.Query);
        }

        #endregion // Unwind_Entities_Replace_Test

        #region Unwind_Create_OnCreateSet_Map_Test

        [Fact]
        public void Unwind_Create_OnCreateSet_Map_Test()
        {
            CypherCommand cypher = _(items => map => n =>
                                    Unwind(items, map,
                                    Merge(N(n, Person, n.P(Id)))
                                    .OnCreateSet(n, map.AsMap)
                                    .Return(n)),
                                    cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
			Assert.Equal("UNWIND $items AS map\r\n" +
                "MERGE (n:PERSON { Id: map.Id })\r\n\t" +
                "ON CREATE SET n = map\r\n" +
                "RETURN n", cypher.Query);
        }

        #endregion // Unwind_Create_OnCreateSet_Map_Test

        #region Unwind_Create_Param_Test

        [Fact]
        public void Unwind_Create_Param_Test()
        {
            CypherCommand cypher = _(items => map => n =>
                                    Unwind(items, map,
                                    Merge(N(n, Person, n.P(Id)))),
                                    cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
			Assert.Equal("UNWIND $items AS map\r\n" +
                "MERGE (n:PERSON { Id: map.Id })", cypher.Query);
        }

        #endregion // Unwind_Create_Param_Test
    }
}

