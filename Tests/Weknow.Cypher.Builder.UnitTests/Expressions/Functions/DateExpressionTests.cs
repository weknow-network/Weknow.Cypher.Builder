using Xunit;
using Xunit.Abstractions;

using static System.Environment;
using static Weknow.CypherBuilder.CypherExtensions;
using static Weknow.CypherBuilder.ICypher;
using static Weknow.CypherBuilder.Schema;

// https://neo4j.com/docs/cypher-manual/current/functions/temporal/
// https://neo4j.com/docs/cypher-manual/5/functions/temporal/

namespace Weknow.CypherBuilder
{
    [Trait("TestType", "Unit")]

    public class DateExpressionTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public DateExpressionTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region CREATE (p:Person { Name: $n, Birthday: datetime(), IssueDate: date(), At: time() }) RETURN p

        [Fact]
        public void DateTime_Test()
        {
            CypherCommand cypher = _(p =>
                                    Create(N(p, Person,
                                    new
                                    {
                                        Name = "someone",
                                        Birthday = DateTimeOffset.UtcNow,
                                        IssueDate = DateTime.Today,
                                        At = DateTime.Now.TimeOfDay
                                    }))
                                    .Return(p));
            CypherCommand cypher1 = _(p =>
                                    Create(N(p, Person,
                                    new
                                    {
                                        Name = "someone",
                                        Birthday = DateTimeOffset.UtcNow,
                                        IssueDate = DateTime.Today,
                                        At = DateTime.Now.TimeOfDay
                                    }))
                                    .Return(p),
                                    cfg => cfg.Time.TimeConvention = TimeConvention.AsFunction);

            _outputHelper.WriteLine(cypher);
            _outputHelper.WriteLine(cypher1);
            Assert.Equal($$"""CREATE (p:Person { Name: $p_0, Birthday: datetime(), IssueDate: date(), At: time() }){{NewLine}}""" +
                "RETURN p", cypher.Query);
            Assert.Equal(cypher1.Query, cypher.Query);
        }

        #endregion // CREATE (p:Person { Name: $n, Birthday: datetime(), IssueDate: date(), At: time() }) RETURN p

        #region CREATE (p:Person { Name: $n, Birthday: datetime.transaction(), IssueDate: date.transaction(), At: time.transaction() }) RETURN p

        [Fact]
        public void DateTime_Transaction_Test()
        {
            CypherCommand cypher = _(p =>
                                    Create(N(p, Person,
                                    new
                                    {
                                        Name = "someone",
                                        Birthday = DateTimeOffset.UtcNow,
                                        IssueDate = DateTime.Today,
                                        At = DateTime.Now.TimeOfDay
                                    }))
                                    .Return(p),
                                    cfg =>
                                    {
                                        cfg.Time.ClockConvention = TimeClockConvention.Transaction;
                                    });

            _outputHelper.WriteLine(cypher);
            Assert.Equal($$"""CREATE (p:Person { Name: $p_0, Birthday: datetime.transaction(), IssueDate: date.transaction(), At: time.transaction() }){{NewLine}}""" +
                "RETURN p", cypher.Query);
        }

        #endregion // CREATE (p:Person { Name: $n, Birthday: datetime.transaction(), IssueDate: date.transaction(), At: time.transaction() }) RETURN p

        #region CREATE (p:Person { Name: $p_0, Birthday: $p_1, IssueDate: $p_2, At: $p_3 }) RETURN p

        [Fact]
        public void DateTime_Prm_Test()
        {
            CypherCommand cypher = _(p =>
                                    Create(N(p, Person,
                                    new
                                    {
                                        Name = "someone",
                                        Birthday = DateTimeOffset.UtcNow,
                                        IssueDate = DateTime.Today,
                                        At = DateTime.Now.TimeOfDay
                                    }))
                                    .Return(p),
                                    cfg => cfg.Time.TimeConvention = TimeConvention.AsParameter);

            _outputHelper.WriteLine(cypher);
            Assert.Equal($$"""CREATE (p:Person { Name: $p_0, Birthday: $p_1, IssueDate: $p_2, At: $p_3 }){{NewLine}}""" +
                "RETURN p", cypher.Query);
            Assert.Equal(cypher.Parameters["p_2"], new DateTimeOffset(DateTime.Today));
        }

        #endregion // CREATE (p:Person { Name: $p_0, Birthday: $p_1, IssueDate: $p_2, At: $p_3 }) RETURN p
    }
}

