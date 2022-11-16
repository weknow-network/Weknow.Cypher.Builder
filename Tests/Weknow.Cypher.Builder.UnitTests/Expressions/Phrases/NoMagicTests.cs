using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.Cypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "NoMagic")]
    [Trait("Segment", "Expression")]
    public class NoMagicTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public NoMagicTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Merge_NoMagic1_Test

        [Fact]
        public void Merge_NoMagic1_Test()
        {
            var map = Parameters.Create<Foo>();
            var n = Variables.Create();

            CypherCommand cypher =
                _(() => Create(N(n, Person, new Foo { Id = (~map)._.Id, Name = (~map)._.FirstName + (~map)._.LastName }))
                           .Set(n, map));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE (n:Person {{ Id: $map.Id, Name: $map.FirstName + $map.LastName }}){NewLine}" +
                "SET n = $map", cypher.Query);
        }

        #endregion // Merge_NoMagic1_Test

        #region Merge_NoMagic2_Test

        [Fact]
        public void Merge_NoMagic2_Test()
        {
            var map_Id = Parameters.Create();
            var n = Variables.Create(); ;

            CypherCommand cypher =
                _(() => Create(N(n, Person, new { Id = map_Id })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "CREATE (n:Person { Id: $map_Id })", cypher.Query);
        }

        #endregion // Merge_NoMagic2_Test

        #region Merge_NoMagic3_Test

        [Fact]
        public void Merge_NoMagic3_Test()
        {
            var map = Parameters.Create<Foo>();
            var n = Variables.Create(); ;

            CypherCommand cypher =
                _(() => Create(N(n, Person,
                                        new
                                        {
                                            (~map)._.Id,
                                            (~map)._.Name
                                        }))
                           .SetPlus(n, map));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE (n:Person {{ Id: $map.Id, Name: $map.Name }}){NewLine}" +
                "SET n += $map", cypher.Query);
        }

        #endregion // Merge_NoMagic3_Test

        #region Merge_NoMagic4_Test

        [Fact]
        public void Merge_NoMagic4_Test()
        {
            var map = Parameters.Create<Someone>();
            var n = Variables.Create(); ;

            CypherCommand cypher =
                _(() => Create(N(n, Person,
                                new
                                {
                                    (~map)._.Id,
                                    Name = (~map)._.FirstName
                                }))
                           .Set(n, new { (~map)._.Address }));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE (n:Person {{ Id: $map.Id, Name: $map.FirstName }}){NewLine}" +
                "SET n.Address = $map.Address", cypher.Query);
        }

        #endregion // Merge_NoMagic4_Test

        #region Unwind_NoMagic5_Test

        [Fact]
        public void Unwind_NoMagic5_Test()
        {
            var items = Parameters.Create();
            var item = Variables.Create<Foo>();
            var n = Variables.Create(); ;

            CypherCommand cypher =
                _(() => Unwind(items, item,
                            Create(N(n, Person,
                                    new Foo { Id = (~item)._.Id, Name = (~item)._.Name })))
                           .Set(n, item));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"UNWIND $items AS item{NewLine}" +
                $"CREATE (n:Person {{ Id: item.Id, Name: item.Name }}){NewLine}" +
                "SET n = item", cypher.Query);
        }

        #endregion // Unwind_NoMagic5_Test

        #region Merge_On_Match_NoMagic6_SetProperties_OfT_Test

        [Fact]
        public void Merge_On_Match_NoMagic6_SetProperties_OfT_Test()
        {
            var (a, b) = Parameters.CreateMulti<string, string>();
            var Id = Parameters.Create();
            var n = Variables.Create(); ;

            CypherCommand cypher = _(() =>
                                    Merge(N(n, Person, new { Id }))
                                    .OnMatchSet(n, new Foo { PropA = a, PropB = b }));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MERGE (n:Person {{ Id: $Id }}){NewLine}\t" +
                "ON MATCH SET n.PropA = $a, n.PropB = $b", cypher.Query);
        }

        #endregion // Merge_On_Match_NoMagic6_SetProperties_OfT_Test
    }
}

