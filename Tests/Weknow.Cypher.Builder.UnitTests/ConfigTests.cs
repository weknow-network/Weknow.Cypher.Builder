using Weknow.Mapping;

using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Segment", "Config")]
    public class ConfigTests
    {
        protected readonly ITestOutputHelper _outputHelper;
        private IType Like => IType.Fake;
        private ILabel Person => ILabel.Fake;
        private ILabel BestSeller => ILabel.Fake;
        private ILabel BOOK => ILabel.Fake;

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
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                        });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:GIT_HUB)
RETURN f"
                           , cypher.Query);
        }

        #endregion // Blank_Label_Convention_Test

        #region Pascal_Label_Camel_Type_Convention_Test

        [Fact]
        public void Pascal_Label_Camel_Type_Convention_Test()
        {
            var f = Variables.Create();

            CypherCommand cypher =
                        _(() =>
                         Create(N(f, BestSeller) - R[BestSeller.R] > N(BOOK))
                         .Return(f)
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("git-hub");
                            cfg.Naming.LabelConvention = CypherNamingConvention.PacalCase;
                            cfg.Naming.TypeConvention = CypherNamingConvention.camelCase;
                        });

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:GitHub:BestSeller)-[:bestSeller]->(:Book:GitHub)
RETURN f"
                           , cypher.Query);
        }

        #endregion // Pascal_Label_Camel_Type_Convention_Test

        #region Blank_ILabel_Convention_Test

        [Fact]
        public void Blank_ILabel_Convention_Test()
        {
            var f = Variables.Create();

            CypherCommand cypher =
                        _(() =>
                         Create(N(f))
                         .Return(f)
                        , cfg =>
                        {
                            cfg.AmbientLabels
                                .Add(Prod)
                                .Add(Maintainer);
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                        });

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:PROD:MAINTAINER)
RETURN f"
                           , cypher.Query);
        }

        #endregion // Blank_ILabel_Convention_Test

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
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
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
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                        };
            CypherCommand cypher =

                        _(() =>
                         Create(N(f, Person, f.AsParameter))
                         .Return(f)
                        );

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:GIT_HUB:PERSON $f)
RETURN f"
                           , cypher.Query);
        }

        #endregion // Label_Convention_Context_Test

        #region NoAmbient_Prop_Label_Convention_Context_Test

        [Fact]
        public void NoAmbient_Prop_Label_Convention_Context_Test()
        {
            CypherConfig.Scope.Value = cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                        };
            CypherCommand cypher =

                        _((n, m) =>
                         Create(N(n, Person) - R[KNOWS] > N(m.NoAmbient))
                         .Return(n, m)
                        );

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (n:GIT_HUB:PERSON)-[:KNOWS]->(m)
RETURN n, m"
                           , cypher.Query);
        }

        #endregion // NoAmbient_Prop_Label_Convention_Context_Test

        #region NoAmbient_Label_Convention_Context_Test

        [Fact]
        public void NoAmbient_Label_Convention_Context_Test()
        {
            var f = Variables.Create();
            CypherConfig.Scope.Value = cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                        };
            CypherCommand cypher =

                        _(() =>
                         NoAmbient(Create(N(f, Person, f.AsParameter)))
                         .SetAmbientLabels(f)
                         .Return(f)
                        );

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:PERSON $f)
SET f:GIT_HUB
RETURN f"
                           , cypher.Query);
        }

        #endregion // NoAmbient_Label_Convention_Context_Test

        #region Enforce_NoAmbient_Label_Convention_Context_Test

        [Fact]
        public void Enforce_NoAmbient_Label_Convention_Context_Test()
        {
            var f = Variables.Create();
            CypherConfig.Scope.Value = cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                        };
            CypherCommand cypher =
                        _(() =>
                         NoAmbient(
                            Create(N(f, Person, f.AsParameter))
                            .SetAmbientLabels(f)
                          )
                         .Return(f)
                        );

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:PERSON $f)
SET f:GIT_HUB
RETURN f"
                           , cypher.Query);
        }

        #endregion // Enforce_NoAmbient_Label_Convention_Context_Test

        #region NoAmbient_Empty_Label_Convention_Context_Test

        [Fact]
        public void NoAmbient_Empty_Label_Convention_Context_Test()
        {
            var f = Variables.Create();
            CypherConfig.Scope.Value = cfg =>
                        {
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                        };
            CypherCommand cypher =

                        _(() =>
                         NoAmbient(Create(N(f, Person, f.AsParameter)))
                         .SetAmbientLabels(f)
                         .Return(f)
                        );

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:PERSON $f)
RETURN f"
                           , cypher.Query);
        }

        #endregion // NoAmbient_Empty_Label_Convention_Context_Test


        #region MATCH (n:Person { Id: $Id }) SET n:Person:Manager

        [Fact]
        public void Match_Set_AddLabels1_Test()
        {
            CypherConfig.Scope.Value = cfg =>
            {
                cfg.AmbientLabels.Add("GitHub");
                cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                cfg.Flavor = Flavor.Neo4j;
            };

            CypherCommand cypher = _((m, n, r) =>
            OptionalMatch(N(m) - R[r] > N(n, Person)));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("OPTIONAL MATCH (m:GIT_HUB)-[r]->(n:GIT_HUB&PERSON)", cypher.Query);
        }

        #endregion // MATCH (n:Person { Id: $Id }) SET n:Person:Manager


        #region Disable_Ambient_Empty_Label_Convention_Context_Test

        [Fact]
        public void Disable_Ambient_Empty_Label_Convention_Context_Test()
        {
            var f = Variables.Create();
            CypherConfig.Scope.Value = cfg =>
                        {
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                            cfg.AmbientLabels.Enable = false;
                        };
            CypherCommand cypher =

                        _(() =>
                         NoAmbient(Create(N(f, Person, f.AsParameter)))
                         .SetAmbientLabels(f)
                         .Return(f)
                        );

            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:PERSON $f)
RETURN f"
                           , cypher.Query);
        }

        #endregion // Disable_Ambient_Empty_Label_Convention_Context_Test

        #region Label_Convention_Context_Overlap_Test

        [Fact]
        public void Label_Convention_Context_Overlap_Test()
        {
            var f = Variables.Create();
            CypherConfig.Scope.Value = cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.AmbientLabels.Add("Microsoft");
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                        };
            CypherCommand cypher =

                        _(() =>
                         Create(N(f, Person, f.Prm))
                         .Return(f)
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.AmbientLabels.Add("Google");
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
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
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                        };
            CypherCommand cypher =

                        _(() =>
                         Create(N(f, Person, f.AsParameter))
                         .Return(f)
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.AmbientLabels.Add("Google");
                            cfg.Naming.LabelConvention = CypherNamingConvention.camelCase;
                        });

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"CREATE (f:gitHub:microsoft:google:person $f)
RETURN f"
                           , cypher.Query);
        }

        #endregion // Label_Convention_Context_Overlap_Case_Test

        #region Label_Convention_Match_Test

        [Fact] // (Skip = "Should be fixed")]
        public void Label_Convention_Match_Neo4j_Test()
        {
            var f = Variables.Create();

            CypherCommand cypher =
                        _((c, mtc) =>
                         Match(N(mtc, Person, mtc.AsParameter))
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.AmbientLabels.Add(Prod);
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                            cfg.Flavor = Flavor.Neo4j;
                        });

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                "MATCH (mtc:GIT_HUB&PROD&PERSON $mtc)"
                           , cypher.Query);
        }

        #endregion // Label_Convention_Match_Test

        #region Label_Convention_Test

        [Fact] // (Skip = "Should be fixed")]
        public void Label_Convention_Test()
        {
            var f = Variables.Create();
            var mrg = Variables.Create<Surface>();

            CypherCommand cypher =
                        _((c, mtc) =>
                         Match(N(mtc, Person, mtc.AsParameter))
                         .Merge(N(mrg, Person, mrg.Prm))
                         .Create(N(c, Person, c.AsParameter))
                         .Return(c, mrg, mtc)
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.AmbientLabels.Add(Prod);
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                            cfg.Flavor = Flavor.Neo4j;
                        });

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal(
                $"MATCH (mtc:GIT_HUB&PROD&PERSON $mtc){NewLine}" +
                $"MERGE (mrg:GIT_HUB:PROD:PERSON {{ Name: $mrg.Name, Color: $mrg.Color }}){NewLine}" +
                $"CREATE (c:GIT_HUB:PROD:PERSON $c){NewLine}" +
                "RETURN c, mrg, mtc"
                           , cypher.Query);
        }

        #endregion // Label_Convention_Test

        #region Label_Convention_Delete_Test

        [Fact]
        public void Label_Convention_Match_Test()
        {
            CypherCommand cypher =
                        _(n =>
                                Match(N(n))
                        //.DetachDelete(n)
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                        });

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"MATCH (n:GIT_HUB)"
                           , cypher.Query);
        }

        #endregion // Label_Convention_Delete_Test

        #region Type_Avoid_Convention_Test

        [Fact]
        public void Type_Avoid_Convention_Test()
        {
            CypherCommand cypher =
                        _(n =>
                                Match(N(n, Person) - R[Like] > N(Person))
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                        });

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"MATCH (n:GIT_HUB:PERSON)-[:Like]->(:PERSON:GIT_HUB)"
                           , cypher.Query);
        }

        #endregion // Type_Avoid_Convention_Test

        #region Avoid_Multi_Ambient_Assignment_Test

        [Fact]
        public void Avoid_Multi_Ambient_Assignment_Test()
        {
            var (n, m) = Variables.CreateMulti<Foo>();

            CypherCommand cypher =
                        _(() =>
                                Match(N(n, Person))
                                .Where(n._.FirstName == "Bob")
                                .Match(N(m, Person))
                                .Where(m._.FirstName == "Dian")
                                .Create(N(n) - R[KNOWS] > N(m))
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                        });

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal($"MATCH (n:GIT_HUB:PERSON){NewLine}" +
                         $"WHERE n.FirstName = $p_0{NewLine}" +
                         $"MATCH (m:GIT_HUB:PERSON){NewLine}" +
                         $"WHERE m.FirstName = $p_1{NewLine}" +
                         $"CREATE (n)-[:KNOWS]->(m)"
                           , cypher.Query);
        }

        #endregion // Avoid_Multi_Ambient_Assignment_Test

        #region Type_Variable_Convention_Test

        [Fact]
        public void Type_Variable_Convention_Test()
        {
            var f = Variables.Create();

            CypherCommand cypher =
                        _((n, r) =>
                                Match(N(n, Person) - R[r, Like] > N(Person))
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                            cfg.Naming.TypeConvention = CypherNamingConvention.SCREAMING_CASE;
                        });

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"MATCH (n:GIT_HUB:PERSON)-[r:LIKE]->(:PERSON:GIT_HUB)"
                           , cypher.Query);
        }

        #endregion // Type_Variable_Convention_Test

        #region Type_Variable_Avoid_Convention_Test

        [Fact]
        public void Type_Variable_Avoid_Convention_Test()
        {
            var f = Variables.Create();

            CypherCommand cypher =
                        _((n, r) =>
                                Match(N(n, Person) - R[r, Like] > N(Person))
                        , cfg =>
                        {
                            cfg.AmbientLabels.Add("GitHub");
                            cfg.Naming.LabelConvention = CypherNamingConvention.SCREAMING_CASE;
                        });

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher);
            Assert.Equal(@"MATCH (n:GIT_HUB:PERSON)-[r:Like]->(:PERSON:GIT_HUB)"
                           , cypher.Query);
        }

        #endregion // Type_Variable_Avoid_Convention_Test
    }
}

