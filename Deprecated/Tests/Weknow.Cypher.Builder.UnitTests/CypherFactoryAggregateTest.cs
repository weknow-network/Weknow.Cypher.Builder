using Xunit;
using Xunit.Abstractions;

using static Weknow.CypherFactory;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.UnitTests
{
    public class CypherFactoryAggregateTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public CypherFactoryAggregateTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        #region Avg

        [Fact]
        public void Avg_Test()
        {
            string result = A.Avg("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("avg(f.Name)", result);
        }

        [Fact]
        public void AvgExp_Test()
        {
            string result = A.Avg<Foo>(f => f.Name);
            _outputHelper.WriteLine(result);
            Assert.Equal("avg(f.Name)", result);
        }

        [Fact]
        public void AvgDistinct_Test()
        {
            string result = A.AvgDistinct("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("avg(DISTINCT f.Name)", result);
        }

        [Fact]
        public void AvgExpDistinct_Test()
        {
            string result = A.AvgDistinct<Foo>(f => f.Name);
            _outputHelper.WriteLine(result);
            Assert.Equal("avg(DISTINCT f.Name)", result);
        } 

        #endregion // Avg

        #region Sum

        [Fact]
        public void Sum_Test()
        {
            string result = A.Sum("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("sum(f.Name)", result);
        }

        [Fact]
        public void SumExp_Test()
        {
            string result = A.Sum<Foo>(f => f.Name);
            _outputHelper.WriteLine(result);
            Assert.Equal("sum(f.Name)", result);
        }

        [Fact]
        public void SumDistinct_Test()
        {
            string result = A.SumDistinct("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("sum(DISTINCT f.Name)", result);
        }

        [Fact]
        public void SumExpDistinct_Test()
        {
            string result = A.SumDistinct<Foo>(f => f.Name);
            _outputHelper.WriteLine(result);
            Assert.Equal("sum(DISTINCT f.Name)", result);
        } 

        #endregion // Sum

        #region Min

        [Fact]
        public void Min_Test()
        {
            string result = A.Min("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("min(f.Name)", result);
        }

        [Fact]
        public void MinExp_Test()
        {
            string result = A.Min<Foo>(f => f.Name);
            _outputHelper.WriteLine(result);
            Assert.Equal("min(f.Name)", result);
        }

        [Fact]
        public void MinDistinct_Test()
        {
            string result = A.MinDistinct("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("min(DISTINCT f.Name)", result);
        }

        [Fact]
        public void MinExpDistinct_Test()
        {
            string result = A.MinDistinct<Foo>(f => f.Name);
            _outputHelper.WriteLine(result);
            Assert.Equal("min(DISTINCT f.Name)", result);
        } 

        #endregion // Min

        #region Max

        [Fact]
        public void Max_Test()
        {
            string result = A.Max("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("max(f.Name)", result);
        }

        [Fact]
        public void MaxExp_Test()
        {
            string result = A.Max<Foo>(f => f.Name);
            _outputHelper.WriteLine(result);
            Assert.Equal("max(f.Name)", result);
        }

        [Fact]
        public void MaxDistinct_Test()
        {
            string result = A.MaxDistinct("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("max(DISTINCT f.Name)", result);
        }

        [Fact]
        public void MaxExpDistinct_Test()
        {
            string result = A.MaxDistinct<Foo>(f => f.Name);
            _outputHelper.WriteLine(result);
            Assert.Equal("max(DISTINCT f.Name)", result);
        } 

        #endregion // Max

        #region Collect

        [Fact]
        public void Collect_Test()
        {
            string result = A.Collect("f.Name").As("names");
            _outputHelper.WriteLine(result);
            Assert.Equal("collect(f.Name) AS names", result);
        }

        [Fact]
        public void Collect_Partial_Test()
        {
            string result = A.Collect("f.Name", "names");
            _outputHelper.WriteLine(result);
            Assert.Equal("collect(f.Name) AS names", result);
        }

        [Fact]
        public void CollectExp_Test()
        {
            string result = A.Collect<Foo>(f => f.Name);
            _outputHelper.WriteLine(result);
            Assert.Equal("collect(f.Name)", result);
        }

        [Fact]
        public void CollectDistinct_Test()
        {
            string result = A.CollectDistinct("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("collect(DISTINCT f.Name)", result);
        }

        [Fact]
        public void CollectExpDistinct_Test()
        {
            string result = A.CollectDistinct<Foo>(f => f.Name);
            _outputHelper.WriteLine(result);
            Assert.Equal("collect(DISTINCT f.Name)", result);
        } 

        #endregion // Collect

        #region Count

        [Fact]
        public void Count_Test()
        {
            string result = A.Count();
            _outputHelper.WriteLine(result);
            Assert.Equal("count(*)", result);
        }

        [Fact]
        public void CountVar_Test()
        {
            string result = A.Count("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("count(f.Name)", result);
        }

        [Fact]
        public void CountDistinct_Test()
        {
            string result = A.CountDistinct();
            _outputHelper.WriteLine(result);
            Assert.Equal("count(DISTINCT *)", result);
        }

        [Fact]
        public void CountDistinctVar_Test()
        {
            string result = A.CountDistinct("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("count(DISTINCT f.Name)", result);
        }

        #endregion // Count

        #region PercentileCount

        [Fact]
        public void PercentileCount_Test()
        {
            string result = A.PercentileCount("f.Name", 0.8);
            _outputHelper.WriteLine(result);
            Assert.Equal("percentileCount(f.Name, 0.8)", result);
        }

        [Fact]
        public void PercentileCountExp_Test()
        {
            string result = A.PercentileCount<Foo>(f => f.Name, 0.8);
            _outputHelper.WriteLine(result);
            Assert.Equal("percentileCount(f.Name, 0.8)", result);
        }

        [Fact]
        public void PercentileCountDistinct_Test()
        {
            string result = A.PercentileCountDistinct("f.Name", 0.8);
            _outputHelper.WriteLine(result);
            Assert.Equal("percentileCount(DISTINCT f.Name, 0.8)", result);
        }

        [Fact]
        public void PercentileCountExpDistinct_Test()
        {
            string result = A.PercentileCountDistinct<Foo>(f => f.Name, 0.8);
            _outputHelper.WriteLine(result);
            Assert.Equal("percentileCount(DISTINCT f.Name, 0.8)", result);
        } 

        #endregion // PercentileCount

        #region PercentileDisc

        [Fact]
        public void PercentileDisc_Test()
        {
            string result = A.PercentileDisc("f.Name", 0.8);
            _outputHelper.WriteLine(result);
            Assert.Equal("percentileDisc(f.Name, 0.8)", result);
        }

        [Fact]
        public void PercentileDiscExp_Test()
        {
            string result = A.PercentileDisc<Foo>(f => f.Name, 0.8);
            _outputHelper.WriteLine(result);
            Assert.Equal("percentileDisc(f.Name, 0.8)", result);
        }

        [Fact]
        public void PercentileDiscDistinct_Test()
        {
            string result = A.PercentileDiscDistinct("f.Name", 0.8);
            _outputHelper.WriteLine(result);
            Assert.Equal("percentileDisc(DISTINCT f.Name, 0.8)", result);
        }

        [Fact]
        public void PercentileDiscExpDistinct_Test()
        {
            string result = A.PercentileDiscDistinct<Foo>(f => f.Name, 0.8);
            _outputHelper.WriteLine(result);
            Assert.Equal("percentileDisc(DISTINCT f.Name, 0.8)", result);
        } 

        #endregion // PercentileDisc

        #region StDev

        [Fact]
        public void StDev_Test()
        {
            string result = A.StDev("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("stDev(f.Name)", result);
        }

        [Fact]
        public void StDevExp_Test()
        {
            string result = A.StDev<Foo>(f => f.Name);
            _outputHelper.WriteLine(result);
            Assert.Equal("stDev(f.Name)", result);
        }

        [Fact]
        public void StDevDistinct_Test()
        {
            string result = A.StDevDistinct("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("stDev(DISTINCT f.Name)", result);
        }

        [Fact]
        public void StDevExpDistinct_Test()
        {
            string result = A.StDevDistinct<Foo>(f => f.Name);
            _outputHelper.WriteLine(result);
            Assert.Equal("stDev(DISTINCT f.Name)", result);
        } 

        #endregion // StDev

        #region StDevP

        [Fact]
        public void StDevP_Test()
        {
            string result = A.StDevP("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("stDevP(f.Name)", result);
        }

        [Fact]
        public void StDevPExp_Test()
        {
            string result = A.StDevP<Foo>(f => f.Name);
            _outputHelper.WriteLine(result);
            Assert.Equal("stDevP(f.Name)", result);
        }

        [Fact]
        public void StDevPDistinct_Test()
        {
            string result = A.StDevPDistinct("f.Name");
            _outputHelper.WriteLine(result);
            Assert.Equal("stDevP(DISTINCT f.Name)", result);
        }

        [Fact]
        public void StDevPExpDistinct_Test()
        {
            string result = A.StDevPDistinct<Foo>(f => f.Name);
            _outputHelper.WriteLine(result);
            Assert.Equal("stDevP(DISTINCT f.Name)", result);
        } 

        #endregion // StDevP
    }
}