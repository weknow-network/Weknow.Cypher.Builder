using System.Diagnostics;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherNodeTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherNodeTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Node_Test

        [Fact]
        public void Node_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                        })
                        .N("n", "LabelA");  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:LabelA:TestLabel)", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_Test

        //#region Node_OfT_Test

        //[Fact]
        //public void Node_OfT_Test()
        //{
        //    FluentCypher cypherCommand =
        //                C.Create(cfg =>
        //                {
        //                    cfg.AmbientLabels.Add("TestLabel");
        //                })
        //                .N<Foo>(n =>);

        //    _outputHelper.WriteLine(cypherCommand);
        //    Assert.Equal("(n:LabelA:TestLabel)", cypherCommand.ToCypher(CypherFormat.SingleLine));
        //}

        //#endregion // Node_OfT_Test

        #region Node_WithPropPrefix_Test

        [Fact]
        public void Node_WithPropPrefix_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Sign = string.Empty;
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .N("n", "LabelA",  p => p.Add("Name", "Id"));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:LabelA:TestLabel { n.Name: x_Name ,n.Id: x_Id })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_WithPropPrefix_Test

        #region Node_WithSign_Test

        [Fact]
        public void Node_WithSign_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                        })
                        .N("n", "LabelA",  p => p.Add("Name", "Id"));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:LabelA:TestLabel { n.Name: $Name ,n.Id: $Id })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_WithSign_Test

        #region Node_WithSignAndPrefix_Test

        [Fact]
        public void Node_WithSignAndPrefix_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .N("n", "LabelA",  p => p.Add("Name", "Id"));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:LabelA:TestLabel { n.Name: $x_Name ,n.Id: $x_Id })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_WithSignAndPrefix_Test

        #region Node_ofT_All_Test

        [Fact]
        public void Node_ofT_All_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .N<Foo>("n", "LabelA",  p => p.AddAll(f => f.DateOfBirth));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:LabelA:TestLabel { n.Id: $x_Id ,n.Name: $x_Name })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_ofT_All_Test

        #region Node_ofT_Convention_Test

        [Fact]
        public void Node_ofT_Convention_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .N<Foo>("n", "LabelA",  
                                p => p.AddByConvention(
                                   name => name != nameof(Foo.DateOfBirth)));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:LabelA:TestLabel { n.Id: $x_Id ,n.Name: $x_Name })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_ofT_Convention_Test

        #region Node_ofT_WithSignAndPrefix_Test

        [Fact]
        public void Node_ofT_WithSignAndPrefix_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .N<Foo>("n", "LabelA",  p => p.Add(f => f.Name)
                                                      .Add(f => f.Id));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:LabelA:TestLabel { n.Name: $x_Name ,n.Id: $x_Id })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_ofT_WithSignAndPrefix_Test

        #region Node_All_Test

        [Fact]
        public void Node_All_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .NodeAll<Foo>("n", f => f.DateOfBirth);  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:Foo:TestLabel { n.Id: $x_Id ,n.Name: $x_Name })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_All_Test

        #region Node_Convention_Test

        [Fact]
        public void Node_Convention_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .NodeByConvention<Foo>(  
                                n => n != nameof(Foo.DateOfBirth));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:Foo:TestLabel { n.Id: $x_Id ,n.Name: $x_Name })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_Convention_Test
    }
}