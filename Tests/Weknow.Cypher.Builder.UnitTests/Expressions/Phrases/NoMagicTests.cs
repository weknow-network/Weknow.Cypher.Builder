using System;
using System.Linq.Expressions;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder
{
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
            ParameterDeclaration<Foo>? map = null;

            CypherCommand cypher =
                _<Foo>(n => Create(N(n, Person, new Foo { Id = map._.Id, Name = map._.FirstName + map._.LastName }))
                           .Set(new { n = map }));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "CREATE (n:Person { Id: $map.Id, Name: $map.FirstName + $map.LastName })\r\n" +
                "SET n = $map", cypher.Query);
        }

        #endregion // Merge_NoMagic1_Test

        #region Merge_NoMagic2_Test

        [Fact]
        public void Merge_NoMagic2_Test()
        {
            var map_Id = CreateParameter();
            VariableDeclaration n = null;

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
            var map = CreateParameter<Foo>();
            VariableDeclaration n = null;

            CypherCommand cypher =
                _(() => Create(N(n, Person, 
                                        new 
                                        { 
                                            Id = map._.Id, 
                                            Name = map._.Name
                                        }))
                           .Set(new { n = +map }));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "CREATE (n:Person { Id: $map.Id, Name: $map.Name })\r\n" +
                "SET n += $map", cypher.Query);
        }

        #endregion // Merge_NoMagic3_Test

        #region Merge_NoMagic4_Test

        [Fact]
        public void Merge_NoMagic4_Test()
        {
            var map = CreateParameter<Someone>();
            VariableDeclaration n = null;

            CypherCommand cypher =
                _(() => Create(N(n, Person,
                                new 
                                { 
                                    Id = map._.Id, 
                                    Name = map._.FirstName 
                                }))
                           .Set(n._(new { Address = map._.Address })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "CREATE (n:Person { Id: $map.Id, Name: $map.FirstName })\r\n" +
                "SET n.Address = $map.Adress", cypher.Query);
        }

        #endregion // Merge_NoMagic4_Test

        #region Unwind_NoMagic5_Test

        [Fact]
        public void Unwind_NoMagic5_Test()
        {
            var items = CreateParameter();
            VariableDeclaration<Foo> item = null;
            VariableDeclaration n = null;

            CypherCommand cypher =
                _(() => Unwind(items, item,
                            Create(N(n, Person, 
                                    new Foo { Id = item._.Id, Name = item._.Name })))
                           .Set(new { n = item }));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "UNWIND $items AS item\r\n" +
                "CREATE (n:Person { Id: item.Id, Name: item.Name })\r\n" +
                "SET n = item", cypher.Query);
        }

        #endregion // Unwind_NoMagic5_Test

        #region Merge_On_Match_NoMagic6_SetProperties_OfT_Test

        [Fact]
        public void Merge_On_Match_NoMagic6_SetProperties_OfT_Test()
        {
            var a = CreateParameter<string>();
            var b = CreateParameter<string>();
            var Id = CreateParameter();
            CypherCommand cypher = _(n =>
                                    Merge(N(n, Person, new { Id }))
                                    .OnMatchSet(n._(new Foo { PropA = a, PropB = b })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MERGE (n:Person { Id: $Id })\r\n\t" +
                "ON MATCH SET n.PropA = $a, n.PropB = $b", cypher.Query);
        }

        #endregion // Merge_On_Match_NoMagic6_SetProperties_OfT_Test
    }
}

