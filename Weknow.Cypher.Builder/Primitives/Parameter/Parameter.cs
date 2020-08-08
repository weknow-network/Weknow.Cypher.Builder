#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{

    /// <summary>
    /// Use for cypher query's parameters
    /// </summary>
    public class Parameter : IParameter
    {
        private Parameter() { }

        /// <summary>
        /// Default (and only) way to get cypher parameter.
        /// It use under expression and don't need a real implementation;
        /// </summary>
        public static readonly IParameter Default = new Parameter();
    }

    /// <summary>
    /// Use for cypher query's parameters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Weknow.Cypher.Builder.IParameter" />
    public class Parameter<T> : IParameter<T>
    {
        private Parameter() { }

        /// <summary>
        /// Default (and only) way to get cypher parameter.
        /// It use under expression and don't need a real implementation;
        /// </summary>
        public static readonly IParameter<T> Default = new Parameter<T>();

        public T _ => throw new System.NotImplementedException();
    }

}
