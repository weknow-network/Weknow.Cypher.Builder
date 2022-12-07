using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

// https://neo4j.com/docs/cypher-cheat-sheet/current/
// https://neo4j.com/docs/cypher-manual/5/constraints/

namespace Weknow.CypherBuilder
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

        #region DROP CONSTRAINT test

        [Fact]
        public void DropConstraint_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => DropConstraint("test-constraint"), cfg =>
            {
                cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"DROP CONSTRAINT test-constraint"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);

        }

        #endregion // DROP CONSTRAINT test

        #region DROP CONSTRAINT test IF EXISTS

        [Fact]
        public void TryDropConstraint_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => TryDropConstraint("test-constraint"), cfg =>
            {
                cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "DROP CONSTRAINT test-constraint IF EXISTS"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);
        }

        #endregion // DROP CONSTRAINT test IF EXISTS

        #region CREATE CONSTRAINT test FOR (n:PERSON) REQUIRE (n.Id, n.Name)

        [Fact]
        public void Constraint_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => CreateConstraint("test-constraint", N(n, Person), n._.Id, n._.Name), cfg =>
            {
                cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE CONSTRAINT test-constraint{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tREQUIRE (n.Id, n.Name)"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);

        }

        #endregion // CREATE CONSTRAINT test FOR (n:PERSON) REQUIRE (n.Id, n.Name)

        #region CREATE CONSTRAINT test IF NOT EXISTS FOR (n:PERSON) REQUIRE (n.Id, n.Name)

        [Fact]
        public void TryConstraint_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => TryCreateConstraint("test-constraint", N(n, Person), n._.Id, n._.Name), cfg =>
            {
                cfg.AmbientLabels.Add("AmbientLabel"); // index should ignore it
                cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE CONSTRAINT test-constraint IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tREQUIRE (n.Id, n.Name)"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);
        }

        #endregion // CREATE CONSTRAINT test IF NOT EXISTS FOR (n:PERSON) REQUIRE (n.Id, n.Name)

        #region CREATE CONSTRAINT test FOR (n:PERSON) REQUIRE (n.Id, n.Name) IS NODE KEY

        [Fact]
        public void Constraint_IsNodeKey_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => CreateConstraint("test-constraint",
                                                ConstraintType.IsNodeKey,
                                                N(n, Person),
                                                n._.Id, n._.Name), cfg =>
            {
                cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE CONSTRAINT test-constraint{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tREQUIRE (n.Id, n.Name) IS NODE KEY"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);
        }

        #endregion // CREATE CONSTRAINT test FOR (n:PERSON) REQUIRE (n.Id, n.Name) IS NODE KEY

        #region CREATE CONSTRAINT .. FOR (n:PERSON) REQUIRE (..) IS UNIQUE 

        [Fact]
        public void TryConstraint_IsUnique_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => TryCreateConstraint(
                                                "test-constraint",
                                                ConstraintType.IsUnique,
                                                N(n, Person),
                                                n._.Id, n._.Name), cfg =>
            {
                cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE CONSTRAINT test-constraint IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tREQUIRE (n.Id, n.Name) IS UNIQUE"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);

        }

        #endregion // CREATE CONSTRAINT .. FOR (n:PERSON) REQUIRE (..) IS UNIQUE  

        #region CREATE CONSTRAINT .. FOR (n:PERSON) REQUIRE (..) IS NODE KEY 

        [Fact]
        public void TryConstraint_IsNodeKey_Test()
        {
            var n = Variables.Create<Foo>();
            CypherCommand cypher = _(() => TryCreateConstraint(
                                                "test-constraint",
                                                ConstraintType.IsNodeKey,
                                                N(n, Person),
                                                 n._.Id, n._.Name), cfg =>
            {
                cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE CONSTRAINT test-constraint IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON){NewLine}" +
                $"\tREQUIRE (n.Id, n.Name) IS NODE KEY"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);

        }

        #endregion // CREATE CONSTRAINT .. FOR (n:PERSON) REQUIRE (..) IS NODE KEY 

        #region CREATE CONSTRAINT .. FOR (n:PERSON)-[r:KNOWS]->() REQUIRE .. IS NOT NULL

        [Fact]
        public void TryConstraint_IsNotNull_Test()
        {
            var (n, r) = Variables.CreateMulti<Foo>();
            CypherCommand cypher = _(() => TryCreateConstraint("test-constraint",
                                                ConstraintType.IsNotNull,
                                                N(n, Person) - R[r, KNOWS] > N(),
                                                n._.Id, r._.Name), cfg =>
            {
                cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
            });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE CONSTRAINT test-constraint IF NOT EXISTS{NewLine}" +
                $"\tFOR (n:PERSON)-[r:KNOWS]->(){NewLine}" +
                $"\tREQUIRE (n.Id, r.Name) IS NOT NULL"
                , cypher.Query);
            Assert.Empty(cypher.Parameters);

        }

        #endregion // CREATE CONSTRAINT .. FOR (n:PERSON)-[r:KNOWS]->() REQUIRE .. IS NOT NULL

        #region CREATE CONSTRAINT .. REQUIRE (n.Id, n.Name) IS NODE KEY OPTIONS {..}

        [Fact]
        public void TryConstraint_IsNodeKey_Options_Test()
        {
            var n = Variables.Create<Foo>();
#pragma warning disable CS0618 // Type or member is obsolete
            CypherCommand cypher = _(() => TryCreateConstraint("test-constraint",
                                                ConstraintType.IsNodeKey,
                                                N(n, Person),
                                                n._.Id, n._.Name)
                                           .WithRawCypher(@"
OPTIONS {
  constraintConfig: {
    `spatial.wgs-84.min`: [-100.0, -100.0],
    `spatial.wgs-84.max`: [100.0, 100.0]
  }
}"), cfg =>
            {
                cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
            });
#pragma warning restore CS0618 // Type or member is obsolete

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"CREATE CONSTRAINT test-constraint IF NOT EXISTS{NewLine}" +
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
            Assert.Empty(cypher.Parameters);

        }

        #endregion // CREATE CONSTRAINT .. REQUIRE (n.Id, n.Name) IS NODE KEY OPTIONS {..}
    }
}

