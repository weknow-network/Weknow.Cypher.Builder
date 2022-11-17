using System.Text;

using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherBuilder.Cypher;
using static Weknow.CypherBuilder.Schema;

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]
    [Trait("Segment", "Expression")]
    public class DashTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public DashTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region MATCH (n:Foo { PropA: $PropA, Date: $Date, Count: $Count, Length: $Length, Position: $Position }) / TTTTTT_Test

        [Fact]
        public void TTTTTT_Test()
        {
            var (m, l, s, srm, sb) = Parameters.CreateMulti<Bar, List<int>, string, Stream, StringBuilder>();
            var n = Variables.Create<Foo>();

            CypherCommand cypher = _(() =>
                                        Match(N(n, Person, new
                                        {
                                            n.Prm._.PropA,
                                            m._.Date,
                                            l._.Count,
                                            s._.Length,
                                            srm._.Position,
                                            sb._.Capacity
                                        })));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n:Person { " +
                                    "PropA: $PropA, " +
                                    "Date: $Date, " +
                                    "Count: $Count, " +
                                    "Length: $Length, " +
                                    "Position: $Position, " +
                                    "Capacity: $Capacity })", cypher.Query);
        }

        #endregion // MATCH (n:Foo { PropA: $PropA, Date: $Date, Count: $Count, Length: $Length, Position: $Position }) / TTTTTT_Test
    }
}

