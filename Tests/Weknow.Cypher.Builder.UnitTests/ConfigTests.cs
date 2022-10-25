using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;
using static Weknow.GraphDbCommands.Schema;

namespace Weknow.GraphDbCommands
{
    [Trait("TestType", "Unit")]
    [Trait("Segment", "Config")]
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

        [Fact(Skip = "Not implemented")]
        public void Config_Test()
        {
            throw new NotImplementedException();
            // TODO: [bnaya, 2020-08] rewrite the sample
            //            var p = Parameters.Create<Foo>();

            //            CypherCommand cypher = _(a => r1 => b => r2 => c =>
            //             Match(N(a, Person) - R[r1, KNOWS] > N(b, Person) < R[r2, KNOWS] - N(c, Person))
            //             .Where(a.OfType<Foo>().Name == "Avi")
            //             .Return(a.OfType<Foo>().Name, r1, b.All<Bar>(), r2, c)
            //             .OrderBy(a.OfType<Foo>().Name)
            //             .Skip(1)
            //             .Limit(10),
            //             cfg =>
            //             {
            //                 cfg.AmbientLabels.Add("Prod", "MyOrg");
            //                 cfg.AmbientLabels.Formatter = "`@{0}`";
            //                 cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
            //             });

            //            _outputHelper.WriteLine(cypher);
            //            _outputHelper.WriteLine(cypher);
            //			 Assert.Equal(
            //@"MATCH (a:PERSON:`@PROD`:`@MY_ORG`)-[r1:KNOWS]->(b:PERSON:`@PROD`:`@MY_ORG`)<-[r2:KNOWS]-(c:PERSON:`@PROD`:`@MY_ORG`)
            //WHERE a.Name = $p_0
            //RETURN a.Name, r1, b.Id, b.Name, b.Date, r2, c
            //ORDER BY a.Name
            //SKIP $p_1
            //LIMIT $p_2", cypher.Query);

            //            _outputHelper.WriteLine(cypher);
            //			 Assert.Equal("Avi", cypher.Parameters["p_0"]);
            //            _outputHelper.WriteLine(cypher);
            //			 Assert.Equal(1, cypher.Parameters["p_1"]);
            //            _outputHelper.WriteLine(cypher);
            //			 Assert.Equal(10, cypher.Parameters["p_2"]);
        }

        #endregion // Config_Test

        #region Blank_Label_Convention_Test

        [Fact]
        public void Blank_Label_Convention_Test()
        {
            var f = Variables.Create();

            CypherCommand cypher =
                        _(() =>
                         Create(N(f))
                         .Return(f)
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                        });

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:GIT_HUB)
RETURN f"
                           , cypher.Query);
        }

        #endregion // Blank_Label_Convention_Test

        #region Blank_Label_Parameter_Convention_Test

        [Fact]
        public void Blank_Label_Parameter_Convention_Test()
        {
            var f = Variables.Create();

            CypherCommand cypher =
                        _(() =>
                         Create(N(f, f.AsParameter))
                         .Return(f)
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                        });

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:GIT_HUB $f)
RETURN f"
                           , cypher.Query);
        }

        #endregion // Blank_Label_Parameter_Convention_Test

        #region Label_Convention_Context_Test

        [Fact]
        public void Label_Convention_Context_Test()
        {
            var f = Variables.Create();
            CypherConfig.Scope.Value = cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                        };
            CypherCommand cypher =

                        _(() =>
                         Create(N(f, Person, f.AsParameter))
                         .Return(f)
                        );

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:GIT_HUB:PERSON $f)
RETURN f"
                           , cypher.Query);
        }

        #endregion // Label_Convention_Context_Test

        #region Label_Convention_Context_Overlap_Test

        [Fact]
        public void Label_Convention_Context_Overlap_Test()
        {
            var f = Variables.Create();
            CypherConfig.Scope.Value = cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.AmbientLabels.Add("Microsoft");
                            cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                        };
            CypherCommand cypher =

                        _(() =>
                         Create(N(f, Person, f.AsParameter))
                         .Return(f)
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.AmbientLabels.Add("Google");
                            cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                        });

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:GIT_HUB:MICROSOFT:GOOGLE:PERSON $f)
RETURN f"
                           , cypher.Query);
        }

        #endregion // Label_Convention_Context_Overlap_Test

        #region Label_Convention_Context_Overlap_Case_Test

        [Fact]
        public void Label_Convention_Context_Overlap_Case_Test()
        {
            var f = Variables.Create();
            CypherConfig.Scope.Value = cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.AmbientLabels.Add("Microsoft");
                            cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                        };
            CypherCommand cypher =

                        _(() =>
                         Create(N(f, Person, f.AsParameter))
                         .Return(f)
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.AmbientLabels.Add("Google");
                            cfg.Naming.Convention = CypherNamingConvention.camelCase;
                        });

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:gitHub:microsoft:google:person $f)
RETURN f"
                           , cypher.Query);
        }

        #endregion // Label_Convention_Context_Overlap_Case_Test

        #region Label_Convention_Test

        [Fact]
        public void Label_Convention_Test()
        {
            var f = Variables.Create();

            CypherCommand cypher =
                        _(() =>
                         Create(N(f, Person, f.AsParameter))
                         .Return(f)
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                        });

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:GIT_HUB:PERSON $f)
RETURN f"
                           , cypher.Query);
        }

        #endregion // Label_Convention_Test
    }
}

