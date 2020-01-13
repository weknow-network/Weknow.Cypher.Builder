using System.Collections;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherReturnTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherReturnTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        [Fact]
        public void Return_Test()
        {
            var cypher = CypherBuilder.Default
                            .Context.Conventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE)
                            .Match("(f:Foo)")
                            .Return("f.BirthDay");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (f:Foo) RETURN f.BirthDay", cypher.ToCypher(CypherFormat.SingleLine));
        }

        [Fact]
        public void ReturnDistict_Test()
        {
            var cypher = CypherBuilder.Default
                            .Context.Conventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE)
                            .Match("(f:Foo)")
                            .ReturnDistinct("f.BirthDay");

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (f:Foo) RETURN DISTINCT f.BirthDay", cypher.ToCypher(CypherFormat.SingleLine));
        }

        [Fact]
        public void Return_Lambda_Test()
        {
            var cypher = CypherBuilder.Default
                            .Context.Conventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE)
                            .Match("(f:Foo)")
                            .Return<Foo>(f => f.DateOfBirth);

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (f:Foo) RETURN f.DateOfBirth", cypher.ToCypher(CypherFormat.SingleLine));
        }

        [Fact]
        public void ReturnDistict_Lambda_Test()
        {
            var cypher = CypherBuilder.Default
                            .Context.Conventions(CypherNamingConvention.SCREAMING_CASE, CypherNamingConvention.SCREAMING_CASE)
                            .Match("(f:Foo)")
                            .ReturnDistinct<Foo>(f => f.DateOfBirth);

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (f:Foo) RETURN DISTINCT f.DateOfBirth", cypher.ToCypher(CypherFormat.SingleLine));
        }
    }
}