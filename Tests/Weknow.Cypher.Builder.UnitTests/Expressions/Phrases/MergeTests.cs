using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Phrases")]

    public class MergeTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public MergeTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MERGE (n:Person { Id: $p.Id }) ON CREATE SET n.Name = $p.Name

        [Fact]
        public void Merge_On_Create_Objects_Test()
        {
            var p = Parameters.Create<Foo>();
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { p.__.Id }))
                                    .OnCreateSet(n.__.Name, p.__.Name));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $p.Id }}){NewLine}\t" +
                "ON CREATE SET n.Name = $p.Name", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $p.Id }) ON CREATE SET n.Name = $p.Name

        #region MERGE (n:Person { Id: $Id }) ON CREATE SET n.PropA = $PropA, n.PropB = $PropB

        [Fact]
        public void Merge_On_Create_SetProperties_Test()
        {
            var Id = Parameters.Create();
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { Id }))
                                    .OnCreateSet(n, new { n.Prm._.PropA, n.AsParameter._.PropB })
                                    .SetDateConvention(n));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $Id }}){NewLine}\t" +
                $"ON CREATE SET n = {{ PropA: $PropA, PropB: $PropB }}{NewLine}\t"+
                $"ON CREATE SET n.`creation-date` = datetime(){NewLine}\t"+
                "ON MATCH SET n.`modification-date` = datetime()"
                , cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON CREATE SET n.PropA = $PropA, n.PropB = $PropB

        #region MERGE (n:Person { Id: $Id }) ON CREATE SET n.PropA = $PropA

        [Fact]
        public void Merge_On_Create_SetProperties_Multi_Test()
        {
            var n = Variables.Create<Foo>();
            var pId = Parameters.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { n.Prm._.Id }))
                                    .OnCreateSet(n.__.PropA, n.Prm._.PropA )
                                    .Merge(N(n, Friend, new { n.Prm._.Id }))
                                    .OnCreateSet(n.__.PropB, n.Prm._.PropB )
                                    .Merge(N(n, Person, new { Id = pId }))
                                    .OnCreateSet(n.__.PropA, n.Prm._.PropA));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $Id }}){NewLine}\t" +
                $"ON CREATE SET n.PropA = $PropA{NewLine}" +
                $"MERGE (n:Friend {{ Id: $Id }}){NewLine}\t" +
                $"ON CREATE SET n.PropB = $PropB{NewLine}" +
                $"MERGE (n:Person {{ Id: $pId }}){NewLine}\t" +
                $"ON CREATE SET n.PropA = $PropA"
                ,
                cypher.Query);
            Assert.True(cypher.Parameters.ContainsKey(nameof(n._.Id)));
            Assert.True(cypher.Parameters.ContainsKey(nameof(pId)));
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON CREATE SET n.PropA = $PropA

        #region MERGE (n:Person { Id: $Id }) ON CREATE SET n.PropA = $PropA

        [Fact]
        public void Merge_On_Create_SetProperties_Infer_Test()
        {
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { n.Prm._.Id }))
                                    .OnCreateSet(n.__.PropA,  n.AsParameter._.PropA ));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $Id }}){NewLine}\t" +
                "ON CREATE SET n.PropA = $PropA", cypher.Query);
            Assert.True(cypher.Parameters.ContainsKey(nameof(n._.Id)));
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON CREATE SET n.PropA = $PropA

        #region MERGE (n:Person { Id: $Id }) ON CREATE SET n = $map 

        [Fact]
        public void Merge_On_Create_SetAsMap_Update_Test()
        {
            var (Id, map) = Parameters.CreateMulti();
            var n = Variables.Create();

            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { Id }))
                                    .OnCreateSet(n, map));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $Id }}){NewLine}\t" +
                "ON CREATE SET n = $map", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON CREATE SET n = $map 

        #region MERGE (n:Person { Id: $Id }) ON MATCH SET n.PropA = $PropA, n.PropB = $PropB 

        [Fact]
        public void Merge_On_Match_SetProperties_Test()
        {
            var (Id, PropA, PropB) = Parameters.CreateMulti();
            var n = Variables.Create();

            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { Id }))
                                    .OnMatchSet(n, new { PropA, PropB }));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $Id }}){NewLine}\t" +
                "ON MATCH SET n = { PropA: $PropA, PropB: $PropB }", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON MATCH SET n.PropA = $PropA, n.PropB = $PropB 

        #region MERGE (n:Person { Id: $n.Id }) ON MATCH SET n = $map

        [Fact]
        public void Merge_On_Match_Propagate_From_Var_1_SetProperties_Update_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { (~n.Prm)._.Id }))
                                    .OnMatchSet(n, n.Prm));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $n.Id }}){NewLine}\t" +
                "ON MATCH SET n = $n", cypher.Query);
            Assert.Single(cypher.Parameters);
            Assert.True(cypher.Parameters.ContainsKey(nameof(n)));
        }

        #endregion // MERGE (n:Person { Id: $n.Id }) ON MATCH SET n = $map

        #region MERGE (n:Person { Id: $n.Id }) ON MATCH SET n = $map

        [Fact]
        public void Merge_On_Match_Propagate_From_Var_2_SetProperties_Update_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { n.Prm.__.Id }))
                                    .OnMatchSet(n, n.Prm));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $n.Id }}){NewLine}\t" +
                "ON MATCH SET n = $n", cypher.Query);
            Assert.Single(cypher.Parameters);
            Assert.True(cypher.Parameters.ContainsKey(nameof(n)));
        }

        #endregion // MERGE (n:Person { Id: $n.Id }) ON MATCH SET n = $map

        #region MERGE (n:Person { Id: $n.Id }) ON MATCH SET n = $map

        [Fact]
        public void Merge_On_Match_Propagate_From_Var_3_SetProperties_Update_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { (~n.AsParameter)._.Id }))
                                    .OnMatchSet(n, n.AsParameter));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $n.Id }}){NewLine}\t" +
                "ON MATCH SET n = $n", cypher.Query);
            Assert.Single(cypher.Parameters);
            Assert.True(cypher.Parameters.ContainsKey(nameof(n)));
        }

        #endregion // MERGE (n:Person { Id: $n.Id }) ON MATCH SET n = $map

        #region MERGE (n:Person { Id: $n.Id }) ON MATCH SET n = $map

        [Fact]
        public void Merge_On_Match_Propagate_From_Var_4_SetProperties_Update_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { n.AsParameter.__.Id }))
                                    .OnMatchSet(n, n.AsParameter));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $n.Id }}){NewLine}\t" +
                "ON MATCH SET n = $n", cypher.Query);
            Assert.Single(cypher.Parameters);
            Assert.True(cypher.Parameters.ContainsKey(nameof(n)));
        }

        #endregion // MERGE (n:Person { Id: $n.Id }) ON MATCH SET n = $map

        #region MERGE (n:Person { Id: $map.Id }) ON MATCH SET n = $map

        [Fact]
        public void Merge_On_Match_Propagate_1_SetProperties_Update_Test()
        {
            var map = Parameters.Create<Foo>();
            var n = Variables.Create();
            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { (~map)._.Id }))
                                    .OnMatchSet(n, map));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $map.Id }}){NewLine}\t" +
                "ON MATCH SET n = $map", cypher.Query);
            Assert.Single(cypher.Parameters);
            Assert.True(cypher.Parameters.ContainsKey(nameof(map)));
        }

        #endregion // MERGE (n:Person { Id: $map.Id }) ON MATCH SET n = $map

        #region MERGE (n:Person { Id: $map.Id }) ON MATCH SET n = $map

        [Fact]
        public void Merge_On_Match_Propagate_2_SetProperties_Update_Test()
        {
            var map = Parameters.Create<Foo>();
            var n = Variables.Create();
            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { map.__.Id }))
                                    .OnMatchSet(n, map));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $map.Id }}){NewLine}\t" +
                "ON MATCH SET n = $map", cypher.Query);
            Assert.Single(cypher.Parameters);
            Assert.True(cypher.Parameters.ContainsKey(nameof(map)));
        }

        #endregion // MERGE (n:Person { Id: $map.Id }) ON MATCH SET n = $map

        #region MERGE (n:Person { Id: $Id }) ON MATCH SET n = $map

        [Fact]
        public void Merge_On_Match_NonPropagate_SetProperties_Update_Test()
        {
            var map = Parameters.Create<Foo>();
            var n = Variables.Create();
            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { map._.Id }))
                                    .OnMatchSet(n, map));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $Id }}){NewLine}\t" +
                "ON MATCH SET n = $map", cypher.Query);
            Assert.Equal(2, cypher.Parameters.Count);
            Assert.True(cypher.Parameters.ContainsKey(nameof(map)));
            Assert.True(cypher.Parameters.ContainsKey(nameof(map.__.Id)));
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON MATCH SET n = $map

        #region MERGE (n:Person { Id: $Id }) ON MATCH SET n += $map

        [Fact]
        public void Merge_On_Match_Plus_SetProperties_Update_Test()
        {
            var (Id, map) = Parameters.CreateMulti();
            var n = Variables.Create();
            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { Id }))
                                    .OnMatchSetPlus(n, map));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $Id }}){NewLine}\t" +
                "ON MATCH SET n += $map", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) SET n += $map

        #region MERGE (n:Person { Id: $Id }) ON MATCH SET n += $map

        [Fact]
        public void Merge_SetProperties_Update_Test()
        {
            var (Id, map) = Parameters.CreateMulti();
            var n = Variables.Create();
            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { Id }))
                                    .SetPlus(n, map));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $Id }}){NewLine}" +
                "SET n += $map", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) SET n += $map

        #region MERGE (n:Person { Id: $Id }) ON MATCH SET n = $map

        [Fact]
        public void Merge_On_Match_SetProperties_Update_Test()
        {
            var (Id, map) = Parameters.CreateMulti();
            var n = Variables.Create();
            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { Id }))
                                    .OnMatchSet(n, map));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $Id }}){NewLine}\t" +
                "ON MATCH SET n = $map", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $Id }) ON MATCH SET n = $map

        #region MATCH (n:Person { Id: $Id }) MERGE (n)-[:KNOWS]->(a:Animal { Id: $Id })

        [Fact]
        public void Merge_AfterMatch_Test()
        {
            var Id = Parameters.Create();
            CypherCommand cypher = _((n, a) =>
                                    Match(N(n, Person, new { Id }))
                                    .Merge(N(n) - R[KNOWS] > N(a, Animal, new { Id })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MATCH (n:Person {{ Id: $Id }}){NewLine}" +
                "MERGE (n)-[:KNOWS]->(a:Animal { Id: $Id })", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) MERGE (n)-[:KNOWS]->(a:Animal { Id: $Id }) 

        #region MERGE (..) ON CREATE SET n = $map ON MATCH SET n.Version = coalesce(n.Version, 0) + 1

        [Fact]
#pragma warning disable CS0618 // Type or member is obsolete
        public void Merge_On_AsCypher_Test()
        {
            var Id = Parameters.Create();
            var map = Parameters.Create<Foo>();

            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { Id }))
                                    .OnCreateSet(n, map)
                                    .OnMatchSet(FromRawCypher("n.Version = coalesce(n.Version, 0) + 1"))
                                    );

            _outputHelper.WriteLine(cypher);
            Assert.Equal(cypher.Query,
                $"MERGE (n:Person {{ Id: $Id }}){NewLine}" +
                $"\tON CREATE SET n = $map{NewLine}" +
                $"\tON MATCH SET n.Version = coalesce(n.Version, 0) + 1"
                );
        }

#pragma warning restore CS0618 // Type or member is obsolete
        #endregion // MERGE (..) ON CREATE SET n = $map ON MATCH SET n.Version = coalesce(n.Version, 0) + 1

        #region MERGE (n:Person { Id: $p.Id }) ON CREATE SET n.PropA = $p.PropA

        [Fact]
        public void Merge_SetPlus_Test()
        {
            var n = Variables.Create<Foo>();
            var p = Parameters.Create<Foo>();

            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { p.__.Id }))
                                    .OnCreateSet(n, new { p.__.PropA }));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $p.Id }}){NewLine}\t" +
                "ON CREATE SET n = { PropA: $p.PropA }"
                ,
                cypher.Query);

            // TODO: [bnaya 2023-01-09] should add parameter 'p' 
            //Assert.True(cypher.Parameters.ContainsKey(nameof(p)));
        }

        #endregion // MERGE (n:Person { Id: $p.Id }) ON CREATE SET n.PropA = $p.PropA

        #region MERGE (n:Person { Id: $p.Id }) ON CREATE SET n.Name = $p.Name

        [Fact]
        public void Merge_Object_Param_Deconstruct_Test()
        {
            var p = Parameters.Create<Surface>();

            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, p)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Name: $p.Name, Color: $p.Color })", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $p.Id }) ON CREATE SET n.Name = $p.Name

        #region MERGE (n:Person { Id: $p.Id }) ON CREATE SET n.Name = $p.Name

        [Fact]
        public void Merge_Object_Param_OfVar_Deconstruct_Test()
        {
            var p = Variables.Create<Surface>();

            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, p.Prm)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Name: $p.Name, Color: $p.Color })", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $p.Id }) ON CREATE SET n.Name = $p.Name

        #region MERGE (n:Person { Id: $p.Id }) ON CREATE SET n.Name = $p.Name

        [Fact]
        public void Merge_Object_Var_Deconstruct_Test()
        {
            var p = Variables.Create<Surface>();

            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, p)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Name: p.Name, Color: p.Color })", cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $p.Id }) ON CREATE SET n.Name = $p.Name

        #region MERGE (n:Person { Id: $p.Id }) ON CREATE SET n.Name = $p.Name

        [Fact]
        public void Unwind_Merge_Object_Param_Deconstruct_Test()
        {
            var p = Parameters.Create<Surface>();

            CypherCommand cypher = _(n =>
                                    Unwind(p, item =>
                                    Merge(N(n, Person, item))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                        """
                        UNWIND $p AS item
                        MERGE (n:Person { Name: item.Name, Color: item.Color })
                        """
                        , cypher.Query);
        }

        #endregion // MERGE (n:Person { Id: $p.Id }) ON CREATE SET n.Name = $p.Name

        // TODO: 
        /*
         MERGE (n:Person {name: $value})
              ON CREATE SET n.created = timestamp()
              ON MATCH SET
                n.counter = coalesce(n.counter, 0) + 1,
                n.accessTime = timestamp()
         */
    }
}

