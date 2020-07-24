using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
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

        #region UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, PropB: item.PropB }) / Unwind_Test

        [Fact]
        public void Unwind_Test()
        {
            CypherCommand cypher = _(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, item._(PropA, PropB)))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { PropA: item.PropA, PropB: item.PropB })", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, PropB: item.PropB }) / Unwind_Test

        #region UNWIND $items AS item MATCH (n:Person { PropA: item.x }) / Unwind_PropConst_Test

        [Fact]
        public void Unwind_PropConst_Test()
        {
            CypherCommand cypher = _(items => item => n => x =>
                                    Unwind(items, item,
                                    Match(N(n, Person, item._(P_(PropA, x))))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { PropA: item.x })", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { PropA: item.x }) / Unwind_PropConst_Test

        #region UNWIND $items AS item MATCH (n:Person { Id: $id, PropB: $PropB }) / Unwind_NonWindProp_Test

        [Fact]
        public void Unwind_NonWindProp_Test()
        {
            CypherCommand cypher = _(items => item => n => id =>
                                    Unwind(items, item,
                                    Match(N(n, Person, NoLoopFormat(P( P_(Id, id), PropB))))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { Id: $id, PropB: $PropB })", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { Id: $id, PropB: $PropB }) / Unwind_NonWindProp_Test

        #region UNWIND $items AS item MATCH (n:Person { Id: $Id }) / Unwind_NonWindProp_T_Test

        [Fact]
        public void Unwind_NonWindProp_T_Test()
        {
            CypherCommand cypher = _(items => item => n => 
                                    Unwind(items, item,
                                    Match(N(n, Person, NoLoopFormat(P("Id"))))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { Id: $Id })", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { Id: $Id }) / Unwind_NonWindProp_T_Test
       
        #region UNWIND $items AS item MATCH (n:Person { PropA: item }) / Unwind_PropConst_AsMap_Test

        [Fact]
        public void Unwind_PropConst_AsMap_Test()
        {
            CypherCommand cypher = _(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, P_(PropA, item)))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { PropA: item })", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { PropA: item }) / Unwind_PropConst_AsMap_Test

        #region UNWIND $items AS item MATCH (n:Person { Id: item.custom }) / Unwind_NonWindProp_T_Test

        [Fact]
        public void Unwind_CustomProp_Test()
        {
            CypherCommand cypher = _(items => item => n => custom =>
                                    Unwind(items, item,
                                    Match(N(n, Person, item._(P_(Id, custom))))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { Id: item.custom })", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { Id: item.custom }) / Unwind_NonWindProp_T_Test

        #region UNWIND $items AS item MATCH (n:Person { Id: item.Name }) / Unwind_NonWindProp_T_Test

        [Fact]
        public void Unwind_CustomProp_T_Test()
        {
            CypherCommand cypher = _<Foo>(n => items => item => 
                                    Unwind(items, item,
                                    Match(N(n, Person, item._(P_(Id, n._.Name))))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { Id: item.Name })", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { Id: item.Name }) / Unwind_NonWindProp_T_Test

        #region UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, PropB: item.PropB }) / Unwind_WithPropConvention_Test

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

        #endregion // UNWIND $items AS item MATCH (n:Person { PropA: item.PropA, PropB: item.PropB }) / Unwind_WithPropConvention_Test

        #region UNWIND $items AS map CREATE (n:Person) SET n = map / Unwind_Create_Map_Test

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

        #endregion // UNWIND $items AS map CREATE (n:Person) SET n = map / Unwind_Create_Map_Test

        #region UNWIND $items AS map CREATE (n:Person) SET n = map / Unwind_Create_AsMap_Test

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

        #endregion // UNWIND $items AS map CREATE (n:Person) SET n = map / Unwind_Create_AsMap_Test

        #region UNWIND $items AS map CREATE (n:Person) SET n = map RETURN n / Unwind_Create_Set_Map_Test

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

        #endregion // UNWIND $items AS map CREATE (n:Person) SET n = map RETURN n / Unwind_Create_Set_Map_Test

        #region UNWIND $items AS item MATCH (n:Person { Id: item.Id }) SET n += item / Unwind_Entities_Update_Test

        [Fact]
        public void Unwind_Entities_Update_Test()
        {
            CypherCommand cypher = _(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, item._(Id)))
                                    .Set(+n, item)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { Id: item.Id })
SET n += item", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { Id: item.Id }) SET n += item / Unwind_Entities_Update_Test

        #region UNWIND $items AS item MATCH (n:Person { Id: item.Id }) SET n = item / Unwind_Entities_Replace_Test

        [Fact]
        public void Unwind_Entities_Replace_Test()
        {
            CypherCommand cypher = _(items => item => n =>
                                    Unwind(items, item,
                                    Match(N(n, Person, item._(Id)))
                                    .Set(n, item))); // + should be unary operator of IVar

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"UNWIND $items AS item
MATCH (n:Person { Id: item.Id })
SET n = item", cypher.Query);
        }

        #endregion // UNWIND $items AS item MATCH (n:Person { Id: item.Id }) SET n = item / Unwind_Entities_Replace_Test

        #region UNWIND $items AS map MERGE (n:PERSON { Id: map.Id }) ON CREATE SET n = map RETURN n / Unwind_Create_OnCreateSet_Map_Test

        [Fact]
        public void Unwind_Create_OnCreateSet_Map_Test()
        {
            CypherCommand cypher = _(items => map => n =>
                                    Unwind(items, map,
                                    Merge(N(n, Person, map._(Id)))
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

        #endregion // UNWIND $items AS map MERGE (n:PERSON { Id: map.Id }) ON CREATE SET n = map RETURN n / Unwind_Create_OnCreateSet_Map_Test

        #region UNWIND $items AS map MERGE (n:PERSON { Id: map.Id }) ON CREATE SET n = map RETURN n / Unwind_Create_OnCreateSet_Mix_Map_Test

        [Fact]
        public void Unwind_Create_OnCreateSet_Mix_Map_Test()
        {
            CypherCommand cypher = _(n => map => items =>
                                   Unwind(items, map,
                                   Merge(N(n, Person, map._(Id)))
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

        #endregion // UNWIND $items AS map MERGE (n:PERSON { Id: map.Id }) ON CREATE SET n = map RETURN n / Unwind_Create_OnCreateSet_Mix_Map_Test

        #region UNWIND $items AS map MERGE (n:PERSON { Id: map.Id }) ON CREATE SET n = map RETURN n / Unwind_Create_OnCreateSet_Gen_Map_Test

        [Fact]
        public void Unwind_Create_OnCreateSet_Gen_Map_Test()
        {
            CypherCommand cypher = _<Foo>(n => map => items =>
                                   Unwind(items, map,
                                   Merge(N(n, Person, map._(n._.Id)))
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

        #endregion // UNWIND $items AS map MERGE (n:PERSON { Id: map.Id }) ON CREATE SET n = map RETURN n / Unwind_Create_OnCreateSet_Gen_Map_Test

        #region UNWIND $items AS map MERGE (n:PERSON { Id: map.Id, Name: map.Name }) ON CREATE SET n = map RETURN n / Unwind_Create_OnCreateSet_Gen_Map_MultiParam_Test

        [Fact]
        public void Unwind_Create_OnCreateSet_Gen_Map_MultiParam_Test()
        {
            CypherCommand cypher = _<Foo>(n => map => items =>
                                   Unwind(items, map,
                                   Merge(N(n, Person, map._(n._.Id, n._.Name)))
                                   .OnCreateSet(n, map.AsMap)
                                   .Return(n)),
                                    cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
            Assert.Equal("UNWIND $items AS map\r\n" +
                "MERGE (n:PERSON { Id: map.Id, Name: map.Name })\r\n\t" +
                "ON CREATE SET n = map\r\n" +
                "RETURN n", cypher.Query);
        }

        #endregion // UNWIND $items AS map MERGE (n:PERSON { Id: map.Id, Name: map.Name }) ON CREATE SET n = map RETURN n / Unwind_Create_OnCreateSet_Gen_Map_MultiParam_Test

        #region UNWIND $items AS map MERGE (n:PERSON { Id: map.Id }) / Unwind_Create_Param_Test

        [Fact]
        public void Unwind_Create_Param_Test()
        {
            CypherCommand cypher = _(items => map => n =>
                                    Unwind(items, map,
                                    Merge(N(n, Person, map._(Id)))),
                                    cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
            Assert.Equal("UNWIND $items AS map\r\n" +
                "MERGE (n:PERSON { Id: map.Id })", cypher.Query);
        }

        #endregion // UNWIND $items AS map MERGE (n:PERSON { Id: map.Id }) / Unwind_Create_Param_Test

        #region UNWIND $items AS map MERGE (n:PERSON { Id: map.Id })-[:By]->(maintainer_:MAINTAINER { Id: $maintainer_Id, Date: $Date }) / Unwind_Param_WithoutMap_Test

        [Fact]
        public void Unwind_Param_WithoutMap_Test()
        {
            var maintainer = Reuse(maintainer_ => R[By] > N(maintainer_, Maintainer, NoLoopFormat(P(_P(maintainer_, Id), Date))));

            CypherCommand cypher = _(items => map => n =>
                                    Unwind(items, map,
                                    Merge(N(n, Person, map._(Id)) - maintainer)),
                                    cfg => cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE);

            _outputHelper.WriteLine(cypher);

            // Require remodel of the cypher generator,
            // On the remodeling it would be nice to add built-in indentation
            Assert.Equal("UNWIND $items AS map\r\n" +
                "MERGE (n:PERSON { Id: map.Id })-[:By]->(maintainer_:MAINTAINER { Id: $maintainer_Id, Date: $Date })", cypher.Query);
        }

        #endregion // UNWIND $items AS map MERGE (n:PERSON { Id: map.Id })-[:By]->(maintainer_:MAINTAINER { Id: $maintainer_Id, Date: $Date }) / Unwind_Param_WithoutMap_Test
    }
}

