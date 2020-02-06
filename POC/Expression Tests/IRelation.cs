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

    public interface IRelation
    {
        IRelation this[IVar var, IType type] { [Cypher("[$0:$1]")]get; }
        IRelation this[IVar var, IType type, IProperties properties] { [Cypher("[$0:$1 { $2 }]")]get; }
        IRelation this[Range r] { [Cypher("[$0]")]get; }
        IRelation this[IVar var, Range r] { [Cypher("[$0$1]")]get; }
        IRelation this[IVar var, IType type, IProperties properties, Range r] { [Cypher("[$0:$1 { $2 } $3]")]get; }
        public static IRelation operator -(IPattern l, IRelation r) => throw new NotImplementedException();
        public static IRelation operator -(IRelation l, IPattern r) => throw new NotImplementedException();
        public static IRelation operator >(IRelation l, IRelation r) => throw new NotImplementedException();
        public static IRelation operator <(IRelation l, IRelation r) => throw new NotImplementedException();
        public static IPattern operator >(IRelation l, IPattern r) => throw new NotImplementedException();
        public static IPattern operator <(IRelation l, IPattern r) => throw new NotImplementedException();
        public static IPattern operator >(IPattern l, IRelation r) => throw new NotImplementedException();
        public static IPattern operator <(IPattern l, IRelation r) => throw new NotImplementedException();
    }

}
