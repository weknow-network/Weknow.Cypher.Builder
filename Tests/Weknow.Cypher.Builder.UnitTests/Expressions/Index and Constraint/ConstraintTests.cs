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
    [Trait("Group", "Constraint")]
    public class ConstraintTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public ConstraintTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region DropConstraint_Test

        [Fact]
        public void DropConstraint_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => DropConstraint("test-constraint"), cfg =>
            {
                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"DROP CONSTRAINT $p_0"
                , cypher.Query);
            Assert.Equal("test-constraint", cypher.Parameters["p_0"]);
            Assert.Equal("test-constraint", cypher.Parameters["$p_0"]);
        }

        #endregion // DropConstraint_Test

        #region TryDropConstraint_Test

        [Fact]
        public void TryDropConstraint_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => TryDropConstraint("test-constraint"), cfg =>
            {
                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "DROP CONSTRAINT $p_0 IF NOT EXISTS"
                , cypher.Query);
            Assert.Equal("test-constraint", cypher.Parameters["p_0"]);
        }

        #endregion // TryDropConstraint_Test

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

        #region TryConstraint_IsUnique_Test

        [Fact]
        public void TryConstraint_IsUnique_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => TryCreateConstraint("test-constraint",
                                                N(n, Person), 
                                                new[] { n._.Id, n._.Name },
                                                ConstraintType.IsUnique), cfg =>
            {
                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE CONSTRAINT $p_0 IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tREQUIRE (n.Id, n.Name) IS UNIQUE"
                , cypher.Query);
            Assert.Equal("test-constraint", cypher.Parameters["p_0"]);
            Assert.Equal("test-constraint", cypher.Parameters["$p_0"]);
        }

        #endregion // TryConstraint_IsUnique_Test

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

        #region TryConstraint_IsNotNull_Test

        [Fact]
        public void TryConstraint_IsNotNull_Test()
        {
            var (n, r) = Variables.CreateMulti<Foo, Foo>();
            CypherCommand cypher = _(() => TryCreateConstraint("test-constraint",
                                                N(n, Person) - R[r, KNOWS] > N(), 
                                                new[] { n._.Id, r._.Name },
                                                ConstraintType.IsNotNull), cfg =>
            {
                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE CONSTRAINT $p_0 IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON)-[r:KNOWS]->(){NewLine}" +
                $"\tREQUIRE (n.Id, r.Name) IS NOT NULL"
                , cypher.Query);
            Assert.Equal("test-constraint", cypher.Parameters["p_0"]);
            Assert.Equal("test-constraint", cypher.Parameters["$p_0"]);
        }

        #endregion // TryConstraint_IsNotNull_Test

        #region TryConstraint_IsNodeKey_Options_Test

        [Fact]
        public void TryConstraint_IsNodeKey_Options_Test()
        {
            var n = Variables.Create<Foo>();
#pragma warning disable CS0618 // Type or member is obsolete
            CypherCommand cypher = _(() => TryCreateConstraint("test-constraint",
                                                N(n, Person), 
                                                new[] { n._.Id, n._.Name },
                                                ConstraintType.IsNodeKey)
                                           .WithRawCypher(@"
OPTIONS {
  constraintConfig: {
    `spatial.wgs-84.min`: [-100.0, -100.0],
    `spatial.wgs-84.max`: [100.0, 100.0]
  }
}"), cfg =>
            {
                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
            });
#pragma warning restore CS0618 // Type or member is obsolete

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE CONSTRAINT $p_0 IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tREQUIRE (n.Id, n.Name) IS NODE KEY{NewLine}" +
                @"
OPTIONS {
  constraintConfig: {
    `spatial.wgs-84.min`: [-100.0, -100.0],
    `spatial.wgs-84.max`: [100.0, 100.0]
  }
}"
                , cypher.Query);
            Assert.Equal("test-constraint", cypher.Parameters["p_0"]);
            Assert.Equal("test-constraint", cypher.Parameters["$p_0"]);
        }

        #endregion // TryConstraint_IsNodeKey_Options_Test
    }
}

