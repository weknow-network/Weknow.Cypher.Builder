using System.Diagnostics;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherRelationTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherRelationTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Relation_Test

        [Fact]
        public void Relation_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                        })
                        .R("n", "TypeA");  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("[n:TypeA]", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Relation_Test

        #region Relation_WithPropPrefix_Test

        [Fact]
        public void Relation_WithPropPrefix_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Sign = string.Empty;
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .R("n", "TypeA",  p => p.Add("Name", "Id"));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("[n:TypeA { n.Name: x_Name ,n.Id: x_Id }]", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Relation_WithPropPrefix_Test

        #region Relation_WithPropPrefix_Short_Test

        [Fact]
        public void Relation_WithPropPrefix_Short_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Sign = string.Empty;
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .R("n", "TypeA",  p => p._("Name", "Id"));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("[n:TypeA { n.Name: x_Name ,n.Id: x_Id }]", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Relation_WithPropPrefix_Short_Test

        #region Relation_WithSign_Test

        [Fact]
        public void Relation_WithSign_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                        })
                        .R("n", "TypeA",  p => p.Add("Name", "Id"));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("[n:TypeA { n.Name: $Name ,n.Id: $Id }]", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Relation_WithSign_Test

        #region Relation_WithSignAndPrefix_Test

        [Fact]
        public void Relation_WithSignAndPrefix_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .R("n", "TypeA",  p => p.Add("Name", "Id"));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("[n:TypeA { n.Name: $x_Name ,n.Id: $x_Id }]", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Relation_WithSignAndPrefix_Test

        #region Relation_ofT_All_Test

        [Fact]
        public void Relation_ofT_All_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .R<Foo>("n", "TypeA",  p => p.All(f => f.DateOfBirth));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("[n:TypeA { n.Id: $x_Id ,n.Name: $x_Name }]", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Relation_ofT_All_Test

        #region Relation_ofT_Convention_Test

        [Fact]
        public void Relation_ofT_Convention_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .R<Foo>("n", "TypeA",  
                                p => p.ByConvention(
                                   name => name != nameof(Foo.DateOfBirth)));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("[n:TypeA { n.Id: $x_Id ,n.Name: $x_Name }]", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Relation_ofT_Convention_Test

        #region Relation_ofT_WithSignAndPrefix_Test

        [Fact]
        public void Relation_ofT_WithSignAndPrefix_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .R<Foo>("n", "TypeA",  p => p.Add(f => f.Name)
                                                      .Add(f => f.Id));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("[n:TypeA { n.Name: $x_Name ,n.Id: $x_Id }]", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // Relation_ofT_WithSignAndPrefix_Test

        #region RelationAll_Test

        [Fact]
        public void RelationAll_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .RelationAll<Foo>("n", f => f.DateOfBirth);  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("[n:Foo { n.Id: $x_Id ,n.Name: $x_Name }]", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // RelationAll_Test

        #region RelationByConvention_Test

        [Fact]
        public void RelationByConvention_Test()
        {
            FluentCypher cypherCommand = 
                        C.Create(cfg =>
                        {
                            cfg.AmbientLabels.Add("TestLabel");
                            cfg.Naming.PropertyParameterConvention.Prefix = "x_";
                        })
                        .RelationByConvention<Foo>(  
                                n => n != nameof(Foo.DateOfBirth));  

            _outputHelper.WriteLine(cypherCommand);
            Assert.Equal("[n:Foo { n.Id: $x_Id ,n.Name: $x_Name }]", cypherCommand.ToCypher(CypherFormat.SingleLine));
        }

        #endregion // RelationByConvention_Test
    }
}