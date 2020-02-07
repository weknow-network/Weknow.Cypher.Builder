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
    // TODO: rename to ICypherUnit
    public interface IVar
    {
        public static IVar operator +(IVar l, IVar r) => throw new NotImplementedException();
        public static IVar operator +(IVar item) => throw new NotImplementedException();

        IMap AsMap { get; }
    }

    public interface IPattern
    {
        public static IPattern operator -(IPattern l, IPattern r) => throw new NotImplementedException();
        public static IPattern operator >(IPattern l, IPattern r) => throw new NotImplementedException();
        public static IPattern operator <(IPattern l, IPattern r) => throw new NotImplementedException();
    }

    public interface ILabel { }
    public interface IType { }
    public interface IProperty { }
    public interface IETagProperty { }
    public interface IMap { }
    public interface IProperties { }
    public interface IParameter { }

}
