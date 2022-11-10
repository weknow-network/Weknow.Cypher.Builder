using System;
using System.Xml.Linq;

using Weknow.GraphDbCommands.Declarations;

using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;
using static Weknow.GraphDbCommands.Schema;
using static System.Environment;

// https://neo4j.com/docs/cypher-cheat-sheet/current/
// https://neo4j.com/docs/cypher-manual/5/constraints/

namespace Weknow.GraphDbCommands
{
    [Trait("TestType", "Unit")]
    [Trait("Group", "Index")]
    public class ConstraintTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public ConstraintTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Constraint_Test

        [Fact]
        public void Constraint_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => CreateConstraint("test-constraint", N(n, Person), n._.Id, n._.Name), cfg =>
            {
                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE CONSTRAINT $p_0{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tREQUIRE (n.Id, n.Name)"
                , cypher.Query);
            Assert.Equal("test-constraint", cypher.Parameters["p_0"]);
            Assert.Equal("test-constraint", cypher.Parameters["$p_0"]);
        }

        #endregion // Constraint_Test

        #region TryConstraint_Test

        [Fact]
        public void TryConstraint_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => TryCreateConstraint("test-constraint", N(n, Person), n._.Id, n._.Name), cfg =>
            {
                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE CONSTRAINT $p_0 IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tREQUIRE (n.Id, n.Name)"
                , cypher.Query);
            Assert.Equal("test-constraint", cypher.Parameters["p_0"]);
            Assert.Equal("test-constraint", cypher.Parameters["$p_0"]);
        }

        #endregion // TryConstraint_Test

        #region Constraint_IsNodeKey_Test

        [Fact]
        public void Constraint_IsNodeKey_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => CreateConstraint("test-constraint", 
                                                N(n, Person), 
                                                new[] { n._.Id, n._.Name },
                                                ConstraintType.IsNodeKey), cfg =>
            {
                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE CONSTRAINT $p_0{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tREQUIRE (n.Id, n.Name) IS NODE KEY"
                , cypher.Query);
            Assert.Equal("test-constraint", cypher.Parameters["p_0"]);
            Assert.Equal("test-constraint", cypher.Parameters["$p_0"]);
        }

        #endregion // Constraint_IsNodeKey_Test

        #region TryConstraint_IsNodeKey_Test

        [Fact]
        public void TryConstraint_IsNodeKey_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => TryCreateConstraint("test-constraint",
                                                N(n, Person), 
                                                new[] { n._.Id, n._.Name },
                                                ConstraintType.IsNodeKey), cfg =>
            {
                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE CONSTRAINT $p_0 IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tREQUIRE (n.Id, n.Name) IS NODE KEY"
                , cypher.Query);
            Assert.Equal("test-constraint", cypher.Parameters["p_0"]);
            Assert.Equal("test-constraint", cypher.Parameters["$p_0"]);
        }

        #endregion // TryConstraint_IsNodeKey_Test
    }
}

