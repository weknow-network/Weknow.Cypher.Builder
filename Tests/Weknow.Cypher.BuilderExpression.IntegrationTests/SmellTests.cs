using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Weknow.Cypher.Builder;
using Xunit;
using Xunit.Abstractions;
using static Weknow.Cypher.Builder.Cypher;
using static Weknow.Cypher.Builder.Schema;

namespace Weknow.Tests
{
    public class SmellTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public SmellTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        // TODO: think of smell tests

        #region Smell_A_Test

        [Fact]
        [Trait("Type", "Integration")]
        public async Task Smell_A_Test()
        {
            throw new NotImplementedException();
            //CypherCommand cypher = _(n1 => n2 => r =>
            //              Reuse(P(Id, Name))
            //             .By(p => items => 
            //                Unwind(items, 
            //                      Create(N(n1, Person, p) - R[r, LIKE] > N(n2, Animal, p)))
            //             ));

            //var items = Enumerable.Range(0, 30);



 //           CypherCommand cypher = _(a => r1 => b => r2 => c =>
 //Create(N(a, Person) - R[r1, KNOWS] > N(b, Person) < R[r2, KNOWS] - N(c, Person))
 //.Where(a.As<Foo>().Name == "Avi")
 //.Return(a.As<Foo>().Name, r1, b.All<Bar>(), r2, c)
 //.OrderBy(a.As<Foo>().Name)
 //.Skip(1)
 //.Limit(10));

        }

        #endregion // Smell_A_Test
    }
}

