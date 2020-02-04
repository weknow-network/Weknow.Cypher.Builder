using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static Weknow.Cypher.Builder.Pattern;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{

    public class Relation
    {
        public Relation this[IVar var, IType type] { [Cypher("[$0:$1]")]get => throw new NotImplementedException(); }
        public Relation this[IVar var, IType type, IProperties properties] { [Cypher("[$0:$1 { $2 }]")]get => throw new NotImplementedException(); }
        public Relation this[Range r] { [Cypher("[$0]")]get => throw new NotImplementedException(); }
        public Relation this[IVar var, Range r] { [Cypher("[$0$1]")]get => throw new NotImplementedException(); }
        public Relation this[IVar var, IType type, IProperties properties, Range r] { [Cypher("[$0:$1 { $2 } $3]")]get => throw new NotImplementedException(); }
        public static Relation operator -(PD l, Relation r) => throw new NotImplementedException();
        public static Relation operator -(Relation l, PD r) => throw new NotImplementedException();
        public static Relation operator >(Relation l, Relation r) => throw new NotImplementedException();
        public static Relation operator <(Relation l, Relation r) => throw new NotImplementedException();
        public static PD operator >(Relation l, PD r) => throw new NotImplementedException();
        public static PD operator <(Relation l, PD r) => throw new NotImplementedException();
        public static PD operator >(PD l, Relation r) => throw new NotImplementedException();
        public static PD operator <(PD l, Relation r) => throw new NotImplementedException();
    }

}
