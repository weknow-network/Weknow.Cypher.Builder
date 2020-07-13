using System;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;
using static System.Environment;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Weknow.Cypher.Builder
{
    [Trait("Category", "Property")]
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

        #region MATCH (n:Foo { PropA: $PropA, PropB: $PropB }) / T_Test

        [Fact]
        public void T_Test()
        {
            CypherCommand cypher = _<Foo>(n => Match(N(n, P(n._.PropA, n._.PropB))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n:Foo { PropA: $PropA, PropB: $PropB })", cypher.Query);
        }

        #endregion // MATCH (n:Foo { PropA: $PropA, PropB: $PropB }) / T_Test

        #region MATCH (n:Foo { PropA: $PropA, Date: $Date }) / TT_Test

        [Fact]
        public void TT_Test()
        {
            CypherCommand cypher = _<Foo, Bar>(n => m => Match(N(n, x =>  P(n._.PropA, m._.Date))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n:Foo { PropA: $PropA, Date: $Date })", cypher.Query);
        }

        #endregion // MATCH (n:Foo { PropA: $PropA, Date: $Date }) / TT_Test

        #region MATCH (n:Foo { PropA: $PropA, Date: $Date, Count: $Count }) / TTT_Test

        [Fact]
        public void TTT_Test()
        {
            CypherCommand cypher = _<Foo, Bar, List<int>>(n => m => l => Match(N(n, P(n._.PropA, m._.Date, l._.Count))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n:Foo { PropA: $PropA, Date: $Date, Count: $Count })", cypher.Query);
        }

        #endregion // MATCH (n:Foo { PropA: $PropA, Date: $Date, Count: $Count }) / TTT_Test

        #region MATCH (n:Foo { PropA: $PropA, Date: $Date, Count: $Count, Length: $Length }) / TTTT_Test

        [Fact]
        public void TTTT_Test()
        {
            CypherCommand cypher = _<Foo, Bar, List<int>, string>(n => m => l => s =>
                                        Match(N(n, P(n._.PropA, m._.Date, l._.Count, s._.Length))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n:Foo { PropA: $PropA, Date: $Date, Count: $Count, Length: $Length })", cypher.Query);
        }

        #endregion // MATCH (n:Foo { PropA: $PropA, Date: $Date, Count: $Count, Length: $Length }) / TTTT_Test

        #region MATCH (n:Foo { PropA: $PropA, Date: $Date, Count: $Count, Length: $Length, Position: $Position }) / TTTTT_Test

        [Fact]
        public void TTTTT_Test()
        {
            CypherCommand cypher = _<Foo, Bar, List<int>, string, Stream>(n => m => l => s => srm =>
                                        Match(N(n, P(
                                                    n._.PropA,
                                                    m._.Date,
                                                    l._.Count, 
                                                    s._.Length,
                                                    srm._.Position))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n:Foo { " +
                                    "PropA: $PropA, " +
                                    "Date: $Date, " +
                                    "Count: $Count, " +
                                    "Length: $Length, " +
                                    "Position: $Position })", cypher.Query);
        }

        #endregion // MATCH (n:Foo { PropA: $PropA, Date: $Date, Count: $Count, Length: $Length, Position: $Position }) / TTTTT_Test

        #region MATCH (n:Foo { PropA: $PropA, Date: $Date, Count: $Count, Length: $Length, Position: $Position }) / TTTTTT_Test

        [Fact]
        public void TTTTTT_Test()
        {
            CypherCommand cypher = _<Foo, Bar, List<int>, string, Stream, StringBuilder>(n => m => l => s => srm => sb =>
                                        Match(N(n, P(
                                                    n._.PropA,
                                                    m._.Date,
                                                    l._.Count, 
                                                    s._.Length,
                                                    srm._.Position,
                                                    sb._.Capacity))));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n:Foo { " +
                                    "PropA: $PropA, " +
                                    "Date: $Date, " +
                                    "Count: $Count, " +
                                    "Length: $Length, " +
                                    "Position: $Position, " +
                                    "Capacity: $Capacity })", cypher.Query);
        }

        #endregion // MATCH (n:Foo { PropA: $PropA, Date: $Date, Count: $Count, Length: $Length, Position: $Position }) / TTTTTT_Test

        #region MATCH (n:Foo { PropA: $PropAvar var pattern, m: $m, l: $l, s: $s, srm: $srm }) / TTTTTT_Test

        [Fact]
        public void Tttttt_Test()
        {
            var pattern = Reuse(n => m => l => s => srm => 
                            N<Foo>(n, x => P(x.PropA, m, l, s, srm)));
            CypherCommand cypher = _<Foo>(n => 
                                        Match(pattern));

            _outputHelper.WriteLine(cypher);
            Assert.Equal("MATCH (n:Foo { " +
                                    "PropA: $PropA, " +
                                    "m: $m, " +
                                    "l: $l, " +
                                    "s: $s, " +
                                    "srm: $srm })", cypher.Query);
        }

        #endregion // MATCH (n:Foo { PropA: $PropA, m: $m, l: $l, s: $s, srm: $srm }) / TTTTTT_Test
    }
}

