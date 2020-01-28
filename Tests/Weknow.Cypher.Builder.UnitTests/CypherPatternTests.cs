using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Weknow.CypherFactory;
using static Weknow.PatternFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherPatternTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherPatternTests(ITestOutputHelper outputHelper)
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

        #region Complex_Pattern_Test

        [Fact]
        public void Complex_Pattern_Test()
        {
            FluentCypher cypherCommand =
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                        })
                        .N("n1", "LabelA") - R["r", "RelB"] > N("n2", "LabelA");
                        // .N("n1", "LabelA") - R["r", "RelB"] * 1..3 > N("n2", "LabelA");

            //string cypher = cypherCommand;
            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n1:LabelA:TestLabel)-[r:RelB]->(n2:LabelA:TestLabel)", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Complex_Pattern_Test

        #region Node_WithPropPrefix_Test

        //[Fact]
        public void Node_WithPropPrefix_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Sign = string.Empty;
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        // TODO: .Prop("key" p => p.Add("Name", "Id"))
                        //       .N("n", "LabelA",  p => p["key"]);  
                        // TODO: .Prop(p => p.Add("Name", "Id"),
                        //              p.N("n", "LabelA",  p));  
                        .N("n", "LabelA",  P.Create("Name", "Id"));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:LabelA:TestLabel { n.Name: x_Name ,n.Id: x_Id })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_WithPropPrefix_Test

        #region Node_WithPropPrefix_Short_Test

        //[Fact]
        public void Node_WithPropPrefix_Short_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Sign = string.Empty;
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .N("n", "LabelA",  P.Create("Name".ToYield("Id"), parameterPrefix:"x_"));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:LabelA:TestLabel { n.Name: x_Name ,n.Id: x_Id })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_WithPropPrefix_Short_Test

        #region Node_WithSign_Test

        //[Fact]
        public void Node_WithSign_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                        })
                        .N("n", "LabelA",  P.Create("Name", "Id"));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:LabelA:TestLabel { n.Name: $Name ,n.Id: $Id })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_WithSign_Test

        #region Node_WithSignAndPrefix_Test

        //[Fact]
        public void Node_WithSignAndPrefix_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .N("n", "LabelA", P.Create("Name", "Id"));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:LabelA:TestLabel { n.Name: $x_Name ,n.Id: $x_Id })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_WithSignAndPrefix_Test

        #region Node_ofT_All_Test

        //[Fact]
        public void Node_ofT_All_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .N("n", "LabelA", P.CreateAll<Foo>(f => f.DateOfBirth));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:LabelA:TestLabel { n.Id: $x_Id ,n.Name: $x_Name })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_ofT_All_Test

        #region Node_ofT_Convention_Test

        //[Fact]
        public void Node_ofT_Convention_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .N("n", "LabelA",  
                                P.CreateByConvention<Foo>(
                                   name => name != nameof(Foo.DateOfBirth)));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:LabelA:TestLabel { n.Id: $x_Id ,n.Name: $x_Name })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_ofT_Convention_Test

        #region Node_ofT_WithSignAndPrefix_Test

        //[Fact]
        public void Node_ofT_WithSignAndPrefix_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .N("n", "LabelA",  P.Create<Foo>(f => f.Name),
                                           P.Create<Foo>(f => f.Id));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("(n:LabelA:TestLabel { n.Name: $x_Name ,n.Id: $x_Id })", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Node_ofT_WithSignAndPrefix_Test
    }
}