using Neo4j.Driver;

using Neo4jMapper;

using ServiceStack;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Cypher.Builder.IntegrationTests
{
    //    //[Trait("Group", "Predicates")]
    //[Trait("Segment", "Expression")]
    public class In_IntegTests : BaseIntegrationTests
    {
        #region Ctor

        public In_IntegTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        #endregion // Ctor

        #region InitDataAsync

        /// <summary>
        /// Initializes the data.
        /// </summary>
        public async Task InitDataAsync()
        {
            ParameterDeclaration p1Id = null, p2Id = null, t1Id = null, t2Id = null;
            CypherCommand cypher = _(p1 => p2 => t1 => t2 =>
                                    Create(N(p1, Person, new { Id = p1Id }))
                                    .Create(N(p2, Person, new { Id = p2Id }))
                                    .Create(N(t1, Tag, new { Id = t1Id }))
                                    .Create(N(t2, Tag, new { Id = t2Id }))
                                    .Merge(N(p1) - R[Affinity] > N(t1))
                                    .Merge(N(p1) - R[Affinity] > N(t2)),
                                    CONFIGURATION);

            CypherParameters prms = cypher.Parameters;
            string id = nameof(Id);
            prms[$"p1{id}"] = "Ben";
            prms[$"p2{id}"] = "Roth";
            prms[$"t1{id}"] = "Manager";
            prms[$"t2{id}"] = "Finance";

            IResultCursor result = await _session.RunAsync(cypher, prms);
            await result.ConsumeAsync();
        }

        #endregion // InitDataAsync

        //private async Task TestData = 
        #region MATCH (n:Person { Id: $Id }) WHERE n IN $items RETURN n / In_Test

        [Fact]
        public async Task In_Test()
        {
            await InitDataAsync();

            CypherCommand cypher = _(n => t => items =>
                                    Match(N(n, Person) - R[Affinity] > N(t, Tag))
                                    .Where(t.In(Id, items))
                                    .Return(n),
                                    CONFIGURATION);

            CypherParameters prms = cypher.Parameters;
            prms["items"] = new[] { "Manager", "Tester" };
            IResultCursor result = await _session.RunAsync(cypher, prms);
            List<Foo> foos = await result.MapAsync<Foo>();
            _outputHelper.WriteLine(cypher);


            Assert.Single(foos);
        }

        #endregion // MATCH (n:Person { Id: $Id }) WHERE n IN $items RETURN n / In_Test
    }
}
