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
                            .Merge($"(f:Foo)")
                            .Merge($"(b:Bar)")
                                .Set("f.SomeProperty = $sp")
                                .Set("f", "SomeOtherProp", "More")
                                .Set<Foo>(f => f.Id)
                                .Set<Bar>(b => b.Value)
                                .SetMore(b => b.Name);

            string expected = 
                "MERGE (f:Foo) " +
                "MERGE (b:Bar) " +
                "SET f.SomeProperty = $sp , " +
                "f.SomeOtherProp = $f_SomeOtherProp , " +
                "f.More = $f_More , " +
                "f.Id = $f_Id , " +
                "b.Value = $b_Value , " +
                "b.Name = $b_Name";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // SetCombination_Test

        #region SetByConvention_Test

        [Fact]
        public void SetByConvention_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = P.Create<Foo>(f => f.Id);
            var cypherCommand = CypherBuilder.Default
                            .Merge($"(f:Foo {props})")
                               .SetByConvention<Foo>("f", n => n != "Id");

            string expected = "MERGE (f:Foo { Id: $Id }) " +
                "SET f.Name = $f_Name , f.DateOfBirth = $f_DateOfBirth";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // SetByConvention_Test

        #region SetAll_Test

        [Fact]
        public void SetAll_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = P.Create<Foo>(f => f.Id);
            var cypherCommand = CypherBuilder.Default
                            .Merge($"(f:Foo {props})")
                               .SetAll<Foo>("f", f => f.Id);

            string expected = "MERGE (f:Foo { Id: $Id }) " +
                "SET f.Name = $f_Name , f.DateOfBirth = $f_DateOfBirth";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // SetAll_Test

        #region SetLabel_Test

        [Fact]
        public void SetLabel_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            string props = P.Create<Foo>(f => f.Id);
            var cypherCommand = CypherBuilder.Default
                            .Merge($"(f:Foo {props})")
                               .SetLabel("f", "NewLabel");

            string expected = "MERGE (f:Foo { Id: $Id }) " +
                "SET f:NewLabel";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // SetLabel_Test

        #region SetInstance_Test

        [Fact]
        public void SetInstance_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            var cypherCommand = CypherBuilder.Default
                            .Merge($"(f:Foo)")
                               .SetInstance<Foo>("f");

            string expected = "MERGE (f:Foo) " +
                "SET f += $Foo";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // SetInstance_Test

        #region SetInstanceReplace_Test

        [Fact]
        public void SetInstanceReplace_Test()
        {
            CypherBuilder.SetDefaultConventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE);

            var cypherCommand = CypherBuilder.Default
                            .Merge($"(f:Foo)")
                               .SetInstance<Foo>("f", SetInstanceBehavior.Replace);

            string expected = "MERGE (f:Foo) " +
                "SET f = $Foo";
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal(expected, cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // SetInstanceReplace_Test
    }
}