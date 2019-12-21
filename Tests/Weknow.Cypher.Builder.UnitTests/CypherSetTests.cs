using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherSetTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherSetTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region SetCombination_Test

        [Fact]
        public void SetCombination_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            var cypherCommand = CypherBuilder.Default
                            .Merge($"(n:Foo)")
                                .Set("f.SomeProperty = $sp")
                                .Set("f", "SomeOtherProp", "More")
                                .Set<Foo>(f => f.Id)
                                .Set<Bar>(b => b.Value)
                                .SetMore(b => b.Name);

            string expected = "MERGE (n:Foo) " +
                "SET f.SomeProperty = $sp , " +
                "f.SomeOtherProp = $f_SomeOtherProp , " +
                "f.More = $f_More , " +
                "f.Id = $f_Id , " +
                "b.Value = $b_Value , " +
                "b.Name = $b_Name";
            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal(expected, cypherCommand.CypherLine);
        }

        #endregion // SetCombination_Test

        #region SetCombination_Test

        [Fact]
        public void SetByConvention_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = P.Create<Foo>(f => f.Id);
            var cypherCommand = CypherBuilder.Default
                            .Merge($"(n:Foo {props})")
                               .SetByConvention<Foo>("f", n => n != "Id");

            string expected = "MERGE (n:Foo { Id: $Id }) " +
                "SET f.Name = $f_Name , f.DateOfBirth = $f_DateOfBirth";
            _outputHelper.WriteLine(cypherCommand.Cypher);
            Assert.Equal(expected, cypherCommand.CypherLine);
        }

        #endregion // SetCombination_Test

        // todo: setall, label, instance, formatting
    }
}