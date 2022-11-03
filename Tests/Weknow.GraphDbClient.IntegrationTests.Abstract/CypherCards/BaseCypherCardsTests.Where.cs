using System.Collections.Generic;
using System;
using System.Data;

using Weknow.GraphDbClient.Abstraction;
using Weknow.GraphDbCommands;
using Weknow.Mapping;

using Xunit;
using Xunit.Abstractions;

using static Weknow.GraphDbCommands.Cypher;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

partial class BaseCypherCardsTests 
{
    #region WHERE n.property <> $value

    [Fact]
    public virtual async Task Where_Prop_NotEq_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        #region Prepare

        var (n_name, m_name, k_name) = Parameters.CreateMulti<PersonEntity, PersonEntity, PersonEntity>();
        CypherCommand cypherPrapare = _(() =>
                                Create(N(Person, new { name = "Dana", age = 10 }))
                                .Create(N(Person, new { name = "Groum", age = 20 }))
                                .Create(N(Person, new { name = "Borka", age = 25 })));
        CypherParameters prmsPrepare = cypherPrapare.Parameters;
        await _graphDB.RunAsync(cypherPrapare, prmsPrepare);

        #endregion // Prepare

        var value = Parameters.Create<int>();
        var n = Variables.Create<PersonEntity>();
        CypherCommand cypher = _(() =>
                                Match(N(n, Person))
                                .Where(n._.age != value)
                                .Return(n._.name));


        CypherParameters prms = cypher.Parameters;
        prms.Add(nameof(value), 25);

        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);

        var people = await response.GetRangeAsync<string>(nameof(n), nameof(n._.name)).ToArrayAsync();

        Assert.Equal(2, people.Length);
        Assert.Contains("Dana", people);
        Assert.Contains("Groum", people);

        _outputHelper.WriteLine($"CYPHER: {cypher}");
    }

    #endregion // WHERE n.property <> $value

    #region WHERE EXISTS { MATCH(n)-->(m) WHERE n.age = m.age }

    [Fact]
    public virtual async Task Where_Exists_Test()
    {
        CypherConfig.Scope.Value = CONFIGURATION;

        string ALICE = "Alice";
        string MIKE = "Mike";
        string BOB = "Bob";

        var (n, m, k) = Variables.CreateMulti<PersonEntity, PersonEntity, PersonEntity>();
        var r = Variables.Create();

        #region Prepare

        var (n_name, m_name, k_name) = Parameters.CreateMulti<PersonEntity, PersonEntity, PersonEntity>();
        CypherCommand cypherPrapare = _(() =>
                                Create(N(n, Person, new { name = n_name, age = 25 }))
                                .Create(N(Person, new { name = "Borka" }))
                                .Create(N(k, Person, new { name = k_name, age = 30 }) - R[Knows] > N(n))
                                .Create(N(m, Person, new { name = m_name, age = 25 }) - R[Knows] > N(n)), CONFIGURATION);
        CypherParameters prmsPrepare = cypherPrapare.Parameters;
        prmsPrepare.Add(nameof(n_name), MIKE);
        prmsPrepare.Add(nameof(m_name), ALICE);
        prmsPrepare.Add(nameof(k_name), BOB);
        await _graphDB.RunAsync(cypherPrapare, prmsPrepare);

        #endregion // Prepare

        CypherCommand cypher = _(() =>
                                Match(N(n, Person))
                                .Where(Exists(() => Match(N(n) > N(m))
                                                    .Where(n._.age == m._.age)))
                                .Return(n));

        CypherParameters prms = cypher.Parameters;
        IGraphDBResponse response = await _graphDB.RunAsync(cypher, prms);

        var people = await response.GetRangeAsync<PersonEntity>(nameof(n)).ToArrayAsync();

        Assert.Single(people);
        Assert.Equal(ALICE, people[0].name);
        _outputHelper.WriteLine($"CYPHER: {cypher}");
    }

    #endregion // WHERE EXISTS { MATCH(n)-->(m) WHERE n.age = m.age }
}
