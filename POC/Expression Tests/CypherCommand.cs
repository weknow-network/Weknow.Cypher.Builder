using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static Weknow.Cypher.Builder.Cypher;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    public class CypherCommand
    {
        public string Query { get; }
        public Dictionary<string, object?> Parameters { get; }

        public CypherCommand(
            string query, 
            Dictionary<string, object?> parameters) // TODO: not necessary dictionary
           {
            Query = query;
            Parameters = parameters;
        }

        public void Print() => Console.WriteLine(this);

        public override string ToString()
        {
            return $@"{Query}
---Parameters---
{string.Join(Environment.NewLine, Parameters)}";
        }
    }

}
