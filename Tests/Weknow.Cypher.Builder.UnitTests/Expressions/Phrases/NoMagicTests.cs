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
            IParameter<Foo>? map = null;

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
            //IParameter<Foo> map_Id = null;

            //CypherCommand cypher =
            //    _<Foo>(() => Create(N(n, Person, new { Id = map_Id })));

            //_outputHelper.WriteLine(cypher);
            //Assert.Equal(
            //    "CREATE(n:Person { Id: $map_Id })", cypher.Query);
        }

        #endregion // Merge_NoMagic2_Test

        #region Merge_NoMagic3_Test

        [Fact]
        public void Merge_NoMagic3_Test()
        {
            throw new NotImplementedException();

            //IParameter<Foo> map = null;

            //CypherCommand cypher =
            //    _<Foo>(n => Create(N(n, Person, new { Id = map._.Id, Name = map._.FirstName + map._.LastName })))
            //               .Set(new { n = +map }));

            //_outputHelper.WriteLine(cypher);
            //Assert.Equal(
            //    "CREATE(n:Person { Id: $map.Id, Name: $map.FirstName + $map.LastName })\r\n" +
            //    "SET n += $map", cypher.Query);
        }

        #endregion // Merge_NoMagic3_Test

        #region Merge_NoMagic4_Test

        [Fact]
        public void Merge_NoMagic4_Test()
        {
            throw new NotImplementedException();
            //IParameter<Foo> map = null;

            //CypherCommand cypher =
            //    _<Foo>(() => Create(N(n, Person, n.With(new { Id = map._.Id, Name = map._.FirstName + map._.LastName }))))
            //               .Set(n.With( new { Address = map._.Address })));

            //_outputHelper.WriteLine(cypher);
            //Assert.Equal(
            //    "CREATE(n:Person { Id: $map.Id, Name: $map.FirstName + $map.LastName })\r\n" +
            //    "Set n.Address = $map.Adress", cypher.Query);
        }

        #endregion // Merge_NoMagic4_Test

        #region Unwind_NoMagic5_Test

        [Fact]
        public void Unwind_NoMagic5_Test()
        {
            throw new NotImplementedException();

            //IParameter items = null;
            //Ivar<Foo> item = null;

            //CypherCommand cypher =
            //    _<Foo>(() => Unwind(items, item, 
            //                Create(N(n, Person, new Foo { Id = item._.Id, Name = item._.FirstName + item._.LastName })))
            //               .Set(new { n = item }));

            //_outputHelper.WriteLine(cypher);
            //Assert.Equal(
            //    "UNWIND items AS item\r\n"
            //    "CREATE(n:Person { Id: item.Id, Name: item.FirstName + item.LastName })\r\n" +
            //    "Set n = item", cypher.Query);
        }

        #endregion // Unwind_NoMagic5_Test
    }
}

