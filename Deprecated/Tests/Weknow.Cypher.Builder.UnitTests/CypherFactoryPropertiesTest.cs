using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    [Trait("Segment", "Deprecate")]
    public class CypherFactoryPropertiesTest
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherFactoryPropertiesTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor


        [Fact]
        public void Property_Create_Test()
        {
            FluentCypher cypher = P.Create("Name");
            string result = cypher;
            _outputHelper.WriteLine(result);
            Assert.Equal("{ Name: $Name }", result);
        }

        [Fact]
        public void Properties_Create_Test()
        {
            FluentCypher cypher = P.Create("Name", "Date");
            string result = cypher;
            _outputHelper.WriteLine(result);
            Assert.Equal("{ Name: $Name, Date: $Date }", result);
        }

        [Fact]
        public void Property_Create_Lambda_Test()
        {
            FluentCypher cypher = P.Create<DateTime>( n => n.Ticks);
            string result = cypher;
            _outputHelper.WriteLine(result);
            Assert.Equal("{ Ticks: $n_Ticks }", result);
        }

        [Fact]
        public void Property_Create_Lambdas_Test()
        {
            FluentCypher cypher = P.Create<DateTime>( n => n.Ticks, f => f.Day);
            string result = cypher;
            _outputHelper.WriteLine(result);
            Assert.Equal("{ Ticks: $n_Ticks, Day: $f_Day }", result);
        }

        [Fact]
        public void Properties_CreateAll_Test()
        {
            FluentCypher cypher = P.CreateAll<DateTime>( n => n.Ticks, f => f.DayOfWeek);
            string result = cypher;
            _outputHelper.WriteLine(result);
            Assert.NotEqual(-1, result.IndexOf(nameof(DateTime.Month)));
            Assert.NotEqual(-1, result.IndexOf(nameof(DateTime.Day)));
            Assert.NotEqual(-1, result.IndexOf(nameof(DateTime.Year)));
            Assert.Equal(-1, result.IndexOf(nameof(DateTime.Ticks)));
            Assert.Equal(-1, result.IndexOf(nameof(DateTime.DayOfWeek)));
        }

        [Fact]
        public void Properties_CreateByConvention_Test()
        {
            FluentCypher cypher = P.CreateByConvention<DateTime>( n => n.StartsWith("Mi"));
            string result = cypher;
            _outputHelper.WriteLine(result);
            Assert.Equal("{ Millisecond: $Millisecond, Minute: $Minute }", result);
        }

    }
}