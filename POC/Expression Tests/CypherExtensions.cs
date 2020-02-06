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

    public static class CypherExtensions
    {
        [Cypher("$0\r\nMATCH $1")]
        public static PD Match(this PD p, PD pp) => throw new NotImplementedException();
        [Cypher("$0\r\nCREATE $1")]
        public static PD Create(this PD p, PD pp) => throw new NotImplementedException();
        [Cypher("$0\r\nMERGE $1")]
        public static PD Merge(this PD p, PD pp) => throw new NotImplementedException();
        [Cypher("$0\r\nOPTIONAL MATCH $1")]
        public static PD OptionalMatch(this PD p, PD pp) => throw new NotImplementedException();
        [Cypher("$0\r\nWHERE $1")]
        public static PD Where(this PD p, bool condition) => throw new NotImplementedException();
        [Cypher("$0\r\nRETURN $1")]
        public static PD Return(this PD p, params object[] vars) => throw new NotImplementedException();
        [Cypher("$0\r\nWITH $1")]
        public static PD With(this PD p, params object[] vars) => throw new NotImplementedException();
        [Cypher("$0\r\nORDER BY $1")]
        public static PD OrderBy(this PD p, params object[] vars) => throw new NotImplementedException();
        [Cypher("$0\r\nORDER BY $1 DESC")]
        public static PD OrderByDesc(this PD p, params object[] vars) => throw new NotImplementedException();
        [Cypher("$0\r\nSKIP $1")]
        public static PD Skip(this PD p, int count) => throw new NotImplementedException();
        [Cypher("$0\r\nLIMIT $1")]
        public static PD Limit(this PD p, int count) => throw new NotImplementedException();
        [Cypher("$0\r\nSET $1:$2")]
        public static PD Set(this PD p, IVar node, ILabel label) => throw new NotImplementedException();
        [Cypher("$0\r\n&SET $1 = $2")]
        public static PD Set(this PD p, IVar node, IVar map) => throw new NotImplementedException();
        [Cypher("$0\r\nSET $1")]
        public static PD Set(this PD p, IVar node) => throw new NotImplementedException();
        [Cypher("$0\r\n&SET $1 \\+= \\$$1")]
        public static PD Set(this PD p, IMap properties) => throw new NotImplementedException();
        [Cypher("$0\r\n&SET $1")]
        public static PD Set(this PD p, IProperties properties) => throw new NotImplementedException();
    }

}
