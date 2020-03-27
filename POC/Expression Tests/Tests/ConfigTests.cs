using System;
using System.Linq.Expressions;
using Xunit;
using Xunit.Abstractions;
using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

// TODO: Duplicate class Pattern to FullNamePattern for naming standard

// TODO: parameter factory injection for enabling to work with Neo4jParameters (Neo4jMapper)
//       Mimic Neo4jMappaer WithEntity, WithEntities + integration test
//       validate flat entity (in deep complex type throw exception with recommendation for best practice)

namespace Weknow.Cypher.Builder
{
    public class ConfigTests
    {
        protected readonly ITestOutputHelper _outputHelper;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigTests"/> class.
        /// </summary>
        /// <param name="outputHelper">The output helper.</param>
        public ConfigTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Config_Test

        [Fact]
        public void Config_Test()
        {
            CypherCommand cypher = _(a => r1 => b => r2 => c =>
             Match(N(a, Person) - R[r1, KNOWS] > N(b, Person) < R[r2, KNOWS] - N(c, Person))
             .Where(a.As<Foo>().Name == "Avi")
             .Return(a.As<Foo>().Name, r1, b.All<Bar>(), r2, c)
             .OrderBy(a.As<Foo>().Name)
             .Skip(1)
             .Limit(10),
             cfg =>
             {
                 cfg.AmbientLabels.Add("Prod", "MyOrg");
                 cfg.AmbientLabels.Formatter = "`@{0}`";
                 cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
             });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(
@"MATCH (a:PERSON:`@PROD`:`@MY_ORG`)-[r1:KNOWS]->(b:PERSON:`@PROD`:`@MY_ORG`)<-[r2:KNOWS]-(c:PERSON:`@PROD`:`@MY_ORG`)
WHERE a.Name = $p_0
RETURN a.Name, r1, b.Id, b.Name, b.Date, r2, c
ORDER BY a.Name
SKIP $p_1
LIMIT $p_2", cypher.Query);

            Assert.Equal("Avi", cypher.Parameters["p_0"]);
            Assert.Equal(1, cypher.Parameters["p_1"]);
            Assert.Equal(10, cypher.Parameters["p_2"]);
        }

        #endregion // Config_Test

        #region Generate_Expression_Test

        [Fact]
        public void Label_Convention_Test()
        {
            CypherCommand cypher =
                        _(f =>
                         Create(N<Foo>(f, f.AsMap))
                         .Return(f)
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                        });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:FOO:GIT_HUB $f)
RETURN f"
                            , cypher.Query);
        }

        #endregion // Generate_Expression_Test
    }
}

