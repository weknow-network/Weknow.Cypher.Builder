using Weknow.CypherBuilder.Declarations;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.CypherBuilder;



#region class Variables<T>

/// <summary>
/// Deconstruct for typed variable of same type
/// </summary>
/// <typeparam name="T"></typeparam>
public class Variables<T>
{
    // TODO: [bnaya 2023-01-24] Use Source Generator in order to generate the Deconstruction (both in form of virtual method with parameters of from an attribute with permutation count)

    public void Deconstruct(
        out VariableDeclaration<T> v1,
        out VariableDeclaration<T> v2)
    {
        v1 = VariableDeclaration<T>.Default;
        v2 = VariableDeclaration<T>.Default;
    }

    public void Deconstruct(
        out VariableDeclaration<T> v1,
        out VariableDeclaration<T> v2,
        out VariableDeclaration<T> v3)
    {
        v1 = VariableDeclaration<T>.Default;
        v2 = VariableDeclaration<T>.Default;
        v3 = VariableDeclaration<T>.Default;
    }
    public void Deconstruct(
        out VariableDeclaration<T> v1,
        out VariableDeclaration<T> v2,
        out VariableDeclaration<T> v3,
        out VariableDeclaration<T> v4)
    {
        v1 = VariableDeclaration<T>.Default;
        v2 = VariableDeclaration<T>.Default;
        v3 = VariableDeclaration<T>.Default;
        v4 = VariableDeclaration<T>.Default;
    }
    public void Deconstruct(
        out VariableDeclaration<T> v1,
        out VariableDeclaration<T> v2,
        out VariableDeclaration<T> v3,
        out VariableDeclaration<T> v4,
        out VariableDeclaration<T> v5)
    {
        v1 = VariableDeclaration<T>.Default;
        v2 = VariableDeclaration<T>.Default;
        v3 = VariableDeclaration<T>.Default;
        v4 = VariableDeclaration<T>.Default;
        v5 = VariableDeclaration<T>.Default;
    }
    public void Deconstruct(
        out VariableDeclaration<T> v1,
        out VariableDeclaration<T> v2,
        out VariableDeclaration<T> v3,
        out VariableDeclaration<T> v4,
        out VariableDeclaration<T> v5,
        out VariableDeclaration<T> v6)
    {
        v1 = VariableDeclaration<T>.Default;
        v2 = VariableDeclaration<T>.Default;
        v3 = VariableDeclaration<T>.Default;
        v4 = VariableDeclaration<T>.Default;
        v5 = VariableDeclaration<T>.Default;
        v6 = VariableDeclaration<T>.Default;
    }
    public void Deconstruct(
        out VariableDeclaration<T> v1,
        out VariableDeclaration<T> v2,
        out VariableDeclaration<T> v3,
        out VariableDeclaration<T> v4,
        out VariableDeclaration<T> v5,
        out VariableDeclaration<T> v6,
        out VariableDeclaration<T> v7)
    {
        v1 = VariableDeclaration<T>.Default;
        v2 = VariableDeclaration<T>.Default;
        v3 = VariableDeclaration<T>.Default;
        v4 = VariableDeclaration<T>.Default;
        v5 = VariableDeclaration<T>.Default;
        v6 = VariableDeclaration<T>.Default;
        v7 = VariableDeclaration<T>.Default;
    }
    public void Deconstruct(
        out VariableDeclaration<T> v1,
        out VariableDeclaration<T> v2,
        out VariableDeclaration<T> v3,
        out VariableDeclaration<T> v4,
        out VariableDeclaration<T> v5,
        out VariableDeclaration<T> v6,
        out VariableDeclaration<T> v7,
        out VariableDeclaration<T> v8)
    {
        v1 = VariableDeclaration<T>.Default;
        v2 = VariableDeclaration<T>.Default;
        v3 = VariableDeclaration<T>.Default;
        v4 = VariableDeclaration<T>.Default;
        v5 = VariableDeclaration<T>.Default;
        v6 = VariableDeclaration<T>.Default;
        v7 = VariableDeclaration<T>.Default;
        v8 = VariableDeclaration<T>.Default;
    }
    public void Deconstruct(
        out VariableDeclaration<T> v1,
        out VariableDeclaration<T> v2,
        out VariableDeclaration<T> v3,
        out VariableDeclaration<T> v4,
        out VariableDeclaration<T> v5,
        out VariableDeclaration<T> v6,
        out VariableDeclaration<T> v7,
        out VariableDeclaration<T> v8,
        out VariableDeclaration<T> v9)
    {
        v1 = VariableDeclaration<T>.Default;
        v2 = VariableDeclaration<T>.Default;
        v3 = VariableDeclaration<T>.Default;
        v4 = VariableDeclaration<T>.Default;
        v5 = VariableDeclaration<T>.Default;
        v6 = VariableDeclaration<T>.Default;
        v7 = VariableDeclaration<T>.Default;
        v8 = VariableDeclaration<T>.Default;
        v9 = VariableDeclaration<T>.Default;
    }
    public void Deconstruct(
        out VariableDeclaration<T> v1,
        out VariableDeclaration<T> v2,
        out VariableDeclaration<T> v3,
        out VariableDeclaration<T> v4,
        out VariableDeclaration<T> v5,
        out VariableDeclaration<T> v6,
        out VariableDeclaration<T> v7,
        out VariableDeclaration<T> v8,
        out VariableDeclaration<T> v9,
        out VariableDeclaration<T> v10)
    {
        v1 = VariableDeclaration<T>.Default;
        v2 = VariableDeclaration<T>.Default;
        v3 = VariableDeclaration<T>.Default;
        v4 = VariableDeclaration<T>.Default;
        v5 = VariableDeclaration<T>.Default;
        v6 = VariableDeclaration<T>.Default;
        v7 = VariableDeclaration<T>.Default;
        v8 = VariableDeclaration<T>.Default;
        v9 = VariableDeclaration<T>.Default;
        v10 = VariableDeclaration<T>.Default;
    }
    public void Deconstruct(
        out VariableDeclaration<T> v1,
        out VariableDeclaration<T> v2,
        out VariableDeclaration<T> v3,
        out VariableDeclaration<T> v4,
        out VariableDeclaration<T> v5,
        out VariableDeclaration<T> v6,
        out VariableDeclaration<T> v7,
        out VariableDeclaration<T> v8,
        out VariableDeclaration<T> v9,
        out VariableDeclaration<T> v10,
        out VariableDeclaration<T> v11)
    {
        v1 = VariableDeclaration<T>.Default;
        v2 = VariableDeclaration<T>.Default;
        v3 = VariableDeclaration<T>.Default;
        v4 = VariableDeclaration<T>.Default;
        v5 = VariableDeclaration<T>.Default;
        v6 = VariableDeclaration<T>.Default;
        v7 = VariableDeclaration<T>.Default;
        v8 = VariableDeclaration<T>.Default;
        v9 = VariableDeclaration<T>.Default;
        v10 = VariableDeclaration<T>.Default;
        v11 = VariableDeclaration<T>.Default;
    }
    public void Deconstruct(
        out VariableDeclaration<T> v1,
        out VariableDeclaration<T> v2,
        out VariableDeclaration<T> v3,
        out VariableDeclaration<T> v4,
        out VariableDeclaration<T> v5,
        out VariableDeclaration<T> v6,
        out VariableDeclaration<T> v7,
        out VariableDeclaration<T> v8,
        out VariableDeclaration<T> v9,
        out VariableDeclaration<T> v10,
        out VariableDeclaration<T> v11,
        out VariableDeclaration<T> v12)
    {
        v1 = VariableDeclaration<T>.Default;
        v2 = VariableDeclaration<T>.Default;
        v3 = VariableDeclaration<T>.Default;
        v4 = VariableDeclaration<T>.Default;
        v5 = VariableDeclaration<T>.Default;
        v6 = VariableDeclaration<T>.Default;
        v7 = VariableDeclaration<T>.Default;
        v8 = VariableDeclaration<T>.Default;
        v9 = VariableDeclaration<T>.Default;
        v10 = VariableDeclaration<T>.Default;
        v11 = VariableDeclaration<T>.Default;
        v12 = VariableDeclaration<T>.Default;
    }
    public void Deconstruct(
        out VariableDeclaration<T> v1,
        out VariableDeclaration<T> v2,
        out VariableDeclaration<T> v3,
        out VariableDeclaration<T> v4,
        out VariableDeclaration<T> v5,
        out VariableDeclaration<T> v6,
        out VariableDeclaration<T> v7,
        out VariableDeclaration<T> v8,
        out VariableDeclaration<T> v9,
        out VariableDeclaration<T> v10,
        out VariableDeclaration<T> v11,
        out VariableDeclaration<T> v12,
        out VariableDeclaration<T> v13)
    {
        v1 = VariableDeclaration<T>.Default;
        v2 = VariableDeclaration<T>.Default;
        v3 = VariableDeclaration<T>.Default;
        v4 = VariableDeclaration<T>.Default;
        v5 = VariableDeclaration<T>.Default;
        v6 = VariableDeclaration<T>.Default;
        v7 = VariableDeclaration<T>.Default;
        v8 = VariableDeclaration<T>.Default;
        v9 = VariableDeclaration<T>.Default;
        v10 = VariableDeclaration<T>.Default;
        v11 = VariableDeclaration<T>.Default;
        v12 = VariableDeclaration<T>.Default;
        v13 = VariableDeclaration<T>.Default;
    }
    public void Deconstruct(
        out VariableDeclaration<T> v1,
        out VariableDeclaration<T> v2,
        out VariableDeclaration<T> v3,
        out VariableDeclaration<T> v4,
        out VariableDeclaration<T> v5,
        out VariableDeclaration<T> v6,
        out VariableDeclaration<T> v7,
        out VariableDeclaration<T> v8,
        out VariableDeclaration<T> v9,
        out VariableDeclaration<T> v10,
        out VariableDeclaration<T> v11,
        out VariableDeclaration<T> v12,
        out VariableDeclaration<T> v13,
        out VariableDeclaration<T> v14)
    {
        v1 = VariableDeclaration<T>.Default;
        v2 = VariableDeclaration<T>.Default;
        v3 = VariableDeclaration<T>.Default;
        v4 = VariableDeclaration<T>.Default;
        v5 = VariableDeclaration<T>.Default;
        v6 = VariableDeclaration<T>.Default;
        v7 = VariableDeclaration<T>.Default;
        v8 = VariableDeclaration<T>.Default;
        v9 = VariableDeclaration<T>.Default;
        v10 = VariableDeclaration<T>.Default;
        v11 = VariableDeclaration<T>.Default;
        v12 = VariableDeclaration<T>.Default;
        v13 = VariableDeclaration<T>.Default;
        v14 = VariableDeclaration<T>.Default;
    }
}

#endregion // class Variables<T>

/// <summary>
/// Variables factories
/// </summary>
public static class Variables
{
    /// <summary>
    /// Get variables declaration.
    /// </summary>
    public static PathVariableDeclaration CreatePath() => PathVariableDeclaration.Default;
    /// <summary>
    /// Get variables declaration.
    /// </summary>
    public static PathVariableDeclaration<T> CreatePath<T>() => PathVariableDeclaration<T>.Default;
    /// <summary>
    /// Get variables declaration.
    /// </summary>
    public static PathDeconstructor CreateMultiPath() => PathDeconstructor.Default;
    /// <summary>
    /// Get variables declaration.
    /// </summary>
    public static PathDeconstructor<T> CreateMultiPath<T>() => PathDeconstructor<T>.Default;

    /// <summary>
    /// Creates the multi.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <example>
    /// <![CDATA[
    /// var (a, b, c, d) = Variables.CreateMulti<Foo>();
    /// ]]>
    /// </example>
    public static Variables<T> CreateMulti<T>() => new Variables<T>();

    /// <summary>
    /// Get variables declaration.
    /// </summary>
    public static VariableDeclaration Create() => VariableDeclaration.Default;
    /// <summary>
    /// Get variables declaration.
    /// </summary>
    public static Deconstructor CreateMulti() => Deconstructor.Default;

    /// <summary>
    /// Get variables declaration.
    /// </summary>
    public static VariableDeclaration<T> Create<T>() => VariableDeclaration<T>.Default;

    /// <summary>
    /// Get variables declaration.
    /// </summary>
    public static (VariableDeclaration<T1>, VariableDeclaration<T2>) CreateMulti<T1, T2>() => (VariableDeclaration<T1>.Default, VariableDeclaration<T2>.Default);
    /// <summary>
    /// Get variables declaration.
    /// </summary>
    public static (VariableDeclaration<T1>, VariableDeclaration<T2>, VariableDeclaration<T3>) CreateMulti<T1, T2, T3>() => (VariableDeclaration<T1>.Default, VariableDeclaration<T2>.Default, VariableDeclaration<T3>.Default);
    /// <summary>
    /// Get variables declaration.
    /// </summary>
    public static (VariableDeclaration<T1>, VariableDeclaration<T2>, VariableDeclaration<T3>, VariableDeclaration<T4>) CreateMulti<T1, T2, T3, T4>() => (VariableDeclaration<T1>.Default, VariableDeclaration<T2>.Default, VariableDeclaration<T3>.Default, VariableDeclaration<T4>.Default);
    /// <summary>
    /// Get variables declaration.
    /// </summary>
    public static (VariableDeclaration<T1>, VariableDeclaration<T2>, VariableDeclaration<T3>, VariableDeclaration<T4>, VariableDeclaration<T5>) CreateMulti<T1, T2, T3, T4, T5>() => (VariableDeclaration<T1>.Default, VariableDeclaration<T2>.Default, VariableDeclaration<T3>.Default, VariableDeclaration<T4>.Default, VariableDeclaration<T5>.Default);
    /// <summary>
    /// Get variables declaration.
    /// </summary>
    public static (VariableDeclaration<T1>, VariableDeclaration<T2>, VariableDeclaration<T3>, VariableDeclaration<T4>, VariableDeclaration<T5>, VariableDeclaration<T6>) CreateMulti<T1, T2, T3, T4, T5, T6>() => (VariableDeclaration<T1>.Default, VariableDeclaration<T2>.Default, VariableDeclaration<T3>.Default, VariableDeclaration<T4>.Default, VariableDeclaration<T5>.Default, VariableDeclaration<T6>.Default);

    #region Deconstructor

    /// <summary>
    /// Variables Deconstruction.
    /// </summary>
    public class Deconstructor
    {
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        public static readonly Deconstructor Default = new Deconstructor();

        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        public void Deconstruct(
            out VariableDeclaration v1,
            out VariableDeclaration v2)
        {
            v1 = VariableDeclaration.Default;
            v2 = VariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        public void Deconstruct(
            out VariableDeclaration v1,
            out VariableDeclaration v2,
            out VariableDeclaration v3)
        {
            v1 = VariableDeclaration.Default;
            v2 = VariableDeclaration.Default;
            v3 = VariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        public void Deconstruct(
            out VariableDeclaration v1,
            out VariableDeclaration v2,
            out VariableDeclaration v3,
            out VariableDeclaration v4)
        {
            v1 = VariableDeclaration.Default;
            v2 = VariableDeclaration.Default;
            v3 = VariableDeclaration.Default;
            v4 = VariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        public void Deconstruct(
            out VariableDeclaration v1,
            out VariableDeclaration v2,
            out VariableDeclaration v3,
            out VariableDeclaration v4,
            out VariableDeclaration v5)
        {
            v1 = VariableDeclaration.Default;
            v2 = VariableDeclaration.Default;
            v3 = VariableDeclaration.Default;
            v4 = VariableDeclaration.Default;
            v5 = VariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        /// <param name="v6">The v6.</param>
        public void Deconstruct(
            out VariableDeclaration v1,
            out VariableDeclaration v2,
            out VariableDeclaration v3,
            out VariableDeclaration v4,
            out VariableDeclaration v5,
            out VariableDeclaration v6)
        {
            v1 = VariableDeclaration.Default;
            v2 = VariableDeclaration.Default;
            v3 = VariableDeclaration.Default;
            v4 = VariableDeclaration.Default;
            v5 = VariableDeclaration.Default;
            v6 = VariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        /// <param name="v6">The v6.</param>
        /// <param name="v7">The v7.</param>
        public void Deconstruct(
            out VariableDeclaration v1,
            out VariableDeclaration v2,
            out VariableDeclaration v3,
            out VariableDeclaration v4,
            out VariableDeclaration v5,
            out VariableDeclaration v6,
            out VariableDeclaration v7)
        {
            v1 = VariableDeclaration.Default;
            v2 = VariableDeclaration.Default;
            v3 = VariableDeclaration.Default;
            v4 = VariableDeclaration.Default;
            v5 = VariableDeclaration.Default;
            v6 = VariableDeclaration.Default;
            v7 = VariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        /// <param name="v6">The v6.</param>
        /// <param name="v7">The v7.</param>
        /// <param name="v8">The v8.</param>
        public void Deconstruct(
            out VariableDeclaration v1,
            out VariableDeclaration v2,
            out VariableDeclaration v3,
            out VariableDeclaration v4,
            out VariableDeclaration v5,
            out VariableDeclaration v6,
            out VariableDeclaration v7,
            out VariableDeclaration v8)
        {
            v1 = VariableDeclaration.Default;
            v2 = VariableDeclaration.Default;
            v3 = VariableDeclaration.Default;
            v4 = VariableDeclaration.Default;
            v5 = VariableDeclaration.Default;
            v6 = VariableDeclaration.Default;
            v7 = VariableDeclaration.Default;
            v8 = VariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        /// <param name="v6">The v6.</param>
        /// <param name="v7">The v7.</param>
        /// <param name="v8">The v8.</param>
        /// <param name="v9">The v9.</param>
        public void Deconstruct(
            out VariableDeclaration v1,
            out VariableDeclaration v2,
            out VariableDeclaration v3,
            out VariableDeclaration v4,
            out VariableDeclaration v5,
            out VariableDeclaration v6,
            out VariableDeclaration v7,
            out VariableDeclaration v8,
            out VariableDeclaration v9)
        {
            v1 = VariableDeclaration.Default;
            v2 = VariableDeclaration.Default;
            v3 = VariableDeclaration.Default;
            v4 = VariableDeclaration.Default;
            v5 = VariableDeclaration.Default;
            v6 = VariableDeclaration.Default;
            v7 = VariableDeclaration.Default;
            v8 = VariableDeclaration.Default;
            v9 = VariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        /// <param name="v6">The v6.</param>
        /// <param name="v7">The v7.</param>
        /// <param name="v8">The v8.</param>
        /// <param name="v9">The v9.</param>
        /// <param name="v10">The P10.</param>
        public void Deconstruct(
            out VariableDeclaration v1,
            out VariableDeclaration v2,
            out VariableDeclaration v3,
            out VariableDeclaration v4,
            out VariableDeclaration v5,
            out VariableDeclaration v6,
            out VariableDeclaration v7,
            out VariableDeclaration v8,
            out VariableDeclaration v9,
            out VariableDeclaration v10)
        {
            v1 = VariableDeclaration.Default;
            v2 = VariableDeclaration.Default;
            v3 = VariableDeclaration.Default;
            v4 = VariableDeclaration.Default;
            v5 = VariableDeclaration.Default;
            v6 = VariableDeclaration.Default;
            v7 = VariableDeclaration.Default;
            v8 = VariableDeclaration.Default;
            v9 = VariableDeclaration.Default;
            v10 = VariableDeclaration.Default;
        }
    }

    #endregion // Deconstructor

    #region PathDeconstructor

    /// <summary>
    /// Variables Deconstruction.
    /// </summary>
    public class PathDeconstructor
    {
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        public static readonly PathDeconstructor Default = new PathDeconstructor();

        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        public void Deconstruct(
            out PathVariableDeclaration v1,
            out PathVariableDeclaration v2)
        {
            v1 = PathVariableDeclaration.Default;
            v2 = PathVariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        public void Deconstruct(
            out PathVariableDeclaration v1,
            out PathVariableDeclaration v2,
            out PathVariableDeclaration v3)
        {
            v1 = PathVariableDeclaration.Default;
            v2 = PathVariableDeclaration.Default;
            v3 = PathVariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        public void Deconstruct(
            out PathVariableDeclaration v1,
            out PathVariableDeclaration v2,
            out PathVariableDeclaration v3,
            out PathVariableDeclaration v4)
        {
            v1 = PathVariableDeclaration.Default;
            v2 = PathVariableDeclaration.Default;
            v3 = PathVariableDeclaration.Default;
            v4 = PathVariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        public void Deconstruct(
            out PathVariableDeclaration v1,
            out PathVariableDeclaration v2,
            out PathVariableDeclaration v3,
            out PathVariableDeclaration v4,
            out PathVariableDeclaration v5)
        {
            v1 = PathVariableDeclaration.Default;
            v2 = PathVariableDeclaration.Default;
            v3 = PathVariableDeclaration.Default;
            v4 = PathVariableDeclaration.Default;
            v5 = PathVariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        /// <param name="v6">The v6.</param>
        public void Deconstruct(
            out PathVariableDeclaration v1,
            out PathVariableDeclaration v2,
            out PathVariableDeclaration v3,
            out PathVariableDeclaration v4,
            out PathVariableDeclaration v5,
            out PathVariableDeclaration v6)
        {
            v1 = PathVariableDeclaration.Default;
            v2 = PathVariableDeclaration.Default;
            v3 = PathVariableDeclaration.Default;
            v4 = PathVariableDeclaration.Default;
            v5 = PathVariableDeclaration.Default;
            v6 = PathVariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        /// <param name="v6">The v6.</param>
        /// <param name="v7">The v7.</param>
        public void Deconstruct(
            out PathVariableDeclaration v1,
            out PathVariableDeclaration v2,
            out PathVariableDeclaration v3,
            out PathVariableDeclaration v4,
            out PathVariableDeclaration v5,
            out PathVariableDeclaration v6,
            out PathVariableDeclaration v7)
        {
            v1 = PathVariableDeclaration.Default;
            v2 = PathVariableDeclaration.Default;
            v3 = PathVariableDeclaration.Default;
            v4 = PathVariableDeclaration.Default;
            v5 = PathVariableDeclaration.Default;
            v6 = PathVariableDeclaration.Default;
            v7 = PathVariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        /// <param name="v6">The v6.</param>
        /// <param name="v7">The v7.</param>
        /// <param name="v8">The v8.</param>
        public void Deconstruct(
            out PathVariableDeclaration v1,
            out PathVariableDeclaration v2,
            out PathVariableDeclaration v3,
            out PathVariableDeclaration v4,
            out PathVariableDeclaration v5,
            out PathVariableDeclaration v6,
            out PathVariableDeclaration v7,
            out PathVariableDeclaration v8)
        {
            v1 = PathVariableDeclaration.Default;
            v2 = PathVariableDeclaration.Default;
            v3 = PathVariableDeclaration.Default;
            v4 = PathVariableDeclaration.Default;
            v5 = PathVariableDeclaration.Default;
            v6 = PathVariableDeclaration.Default;
            v7 = PathVariableDeclaration.Default;
            v8 = PathVariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        /// <param name="v6">The v6.</param>
        /// <param name="v7">The v7.</param>
        /// <param name="v8">The v8.</param>
        /// <param name="v9">The v9.</param>
        public void Deconstruct(
            out PathVariableDeclaration v1,
            out PathVariableDeclaration v2,
            out PathVariableDeclaration v3,
            out PathVariableDeclaration v4,
            out PathVariableDeclaration v5,
            out PathVariableDeclaration v6,
            out PathVariableDeclaration v7,
            out PathVariableDeclaration v8,
            out PathVariableDeclaration v9)
        {
            v1 = PathVariableDeclaration.Default;
            v2 = PathVariableDeclaration.Default;
            v3 = PathVariableDeclaration.Default;
            v4 = PathVariableDeclaration.Default;
            v5 = PathVariableDeclaration.Default;
            v6 = PathVariableDeclaration.Default;
            v7 = PathVariableDeclaration.Default;
            v8 = PathVariableDeclaration.Default;
            v9 = PathVariableDeclaration.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        /// <param name="v6">The v6.</param>
        /// <param name="v7">The v7.</param>
        /// <param name="v8">The v8.</param>
        /// <param name="v9">The v9.</param>
        /// <param name="v10">The P10.</param>
        public void Deconstruct(
            out PathVariableDeclaration v1,
            out PathVariableDeclaration v2,
            out PathVariableDeclaration v3,
            out PathVariableDeclaration v4,
            out PathVariableDeclaration v5,
            out PathVariableDeclaration v6,
            out PathVariableDeclaration v7,
            out PathVariableDeclaration v8,
            out PathVariableDeclaration v9,
            out PathVariableDeclaration v10)
        {
            v1 = PathVariableDeclaration.Default;
            v2 = PathVariableDeclaration.Default;
            v3 = PathVariableDeclaration.Default;
            v4 = PathVariableDeclaration.Default;
            v5 = PathVariableDeclaration.Default;
            v6 = PathVariableDeclaration.Default;
            v7 = PathVariableDeclaration.Default;
            v8 = PathVariableDeclaration.Default;
            v9 = PathVariableDeclaration.Default;
            v10 = PathVariableDeclaration.Default;
        }
    }

    /// <summary>
    /// Variables Deconstruction.
    /// </summary>
    public class PathDeconstructor<T>
    {
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        public static readonly PathDeconstructor<T> Default = new PathDeconstructor<T>();

        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        public void Deconstruct(
            out PathVariableDeclaration<T> v1,
            out PathVariableDeclaration<T> v2)
        {
            v1 = PathVariableDeclaration<T>.Default;
            v2 = PathVariableDeclaration<T>.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        public void Deconstruct(
            out PathVariableDeclaration<T> v1,
            out PathVariableDeclaration<T> v2,
            out PathVariableDeclaration<T> v3)
        {
            v1 = PathVariableDeclaration<T>.Default;
            v2 = PathVariableDeclaration<T>.Default;
            v3 = PathVariableDeclaration<T>.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        public void Deconstruct(
            out PathVariableDeclaration<T> v1,
            out PathVariableDeclaration<T> v2,
            out PathVariableDeclaration<T> v3,
            out PathVariableDeclaration<T> v4)
        {
            v1 = PathVariableDeclaration<T>.Default;
            v2 = PathVariableDeclaration<T>.Default;
            v3 = PathVariableDeclaration<T>.Default;
            v4 = PathVariableDeclaration<T>.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        public void Deconstruct(
            out PathVariableDeclaration<T> v1,
            out PathVariableDeclaration<T> v2,
            out PathVariableDeclaration<T> v3,
            out PathVariableDeclaration<T> v4,
            out PathVariableDeclaration<T> v5)
        {
            v1 = PathVariableDeclaration<T>.Default;
            v2 = PathVariableDeclaration<T>.Default;
            v3 = PathVariableDeclaration<T>.Default;
            v4 = PathVariableDeclaration<T>.Default;
            v5 = PathVariableDeclaration<T>.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        /// <param name="v6">The v6.</param>
        public void Deconstruct(
            out PathVariableDeclaration<T> v1,
            out PathVariableDeclaration<T> v2,
            out PathVariableDeclaration<T> v3,
            out PathVariableDeclaration<T> v4,
            out PathVariableDeclaration<T> v5,
            out PathVariableDeclaration<T> v6)
        {
            v1 = PathVariableDeclaration<T>.Default;
            v2 = PathVariableDeclaration<T>.Default;
            v3 = PathVariableDeclaration<T>.Default;
            v4 = PathVariableDeclaration<T>.Default;
            v5 = PathVariableDeclaration<T>.Default;
            v6 = PathVariableDeclaration<T>.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        /// <param name="v6">The v6.</param>
        /// <param name="v7">The v7.</param>
        public void Deconstruct(
            out PathVariableDeclaration<T> v1,
            out PathVariableDeclaration<T> v2,
            out PathVariableDeclaration<T> v3,
            out PathVariableDeclaration<T> v4,
            out PathVariableDeclaration<T> v5,
            out PathVariableDeclaration<T> v6,
            out PathVariableDeclaration<T> v7)
        {
            v1 = PathVariableDeclaration<T>.Default;
            v2 = PathVariableDeclaration<T>.Default;
            v3 = PathVariableDeclaration<T>.Default;
            v4 = PathVariableDeclaration<T>.Default;
            v5 = PathVariableDeclaration<T>.Default;
            v6 = PathVariableDeclaration<T>.Default;
            v7 = PathVariableDeclaration<T>.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        /// <param name="v6">The v6.</param>
        /// <param name="v7">The v7.</param>
        /// <param name="v8">The v8.</param>
        public void Deconstruct(
            out PathVariableDeclaration<T> v1,
            out PathVariableDeclaration<T> v2,
            out PathVariableDeclaration<T> v3,
            out PathVariableDeclaration<T> v4,
            out PathVariableDeclaration<T> v5,
            out PathVariableDeclaration<T> v6,
            out PathVariableDeclaration<T> v7,
            out PathVariableDeclaration<T> v8)
        {
            v1 = PathVariableDeclaration<T>.Default;
            v2 = PathVariableDeclaration<T>.Default;
            v3 = PathVariableDeclaration<T>.Default;
            v4 = PathVariableDeclaration<T>.Default;
            v5 = PathVariableDeclaration<T>.Default;
            v6 = PathVariableDeclaration<T>.Default;
            v7 = PathVariableDeclaration<T>.Default;
            v8 = PathVariableDeclaration<T>.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        /// <param name="v6">The v6.</param>
        /// <param name="v7">The v7.</param>
        /// <param name="v8">The v8.</param>
        /// <param name="v9">The v9.</param>
        public void Deconstruct(
            out PathVariableDeclaration<T> v1,
            out PathVariableDeclaration<T> v2,
            out PathVariableDeclaration<T> v3,
            out PathVariableDeclaration<T> v4,
            out PathVariableDeclaration<T> v5,
            out PathVariableDeclaration<T> v6,
            out PathVariableDeclaration<T> v7,
            out PathVariableDeclaration<T> v8,
            out PathVariableDeclaration<T> v9)
        {
            v1 = PathVariableDeclaration<T>.Default;
            v2 = PathVariableDeclaration<T>.Default;
            v3 = PathVariableDeclaration<T>.Default;
            v4 = PathVariableDeclaration<T>.Default;
            v5 = PathVariableDeclaration<T>.Default;
            v6 = PathVariableDeclaration<T>.Default;
            v7 = PathVariableDeclaration<T>.Default;
            v8 = PathVariableDeclaration<T>.Default;
            v9 = PathVariableDeclaration<T>.Default;
        }
        /// <summary>
        /// Variables Deconstruction.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="v3">The v3.</param>
        /// <param name="v4">The v4.</param>
        /// <param name="v5">The v5.</param>
        /// <param name="v6">The v6.</param>
        /// <param name="v7">The v7.</param>
        /// <param name="v8">The v8.</param>
        /// <param name="v9">The v9.</param>
        /// <param name="v10">The P10.</param>
        public void Deconstruct(
            out PathVariableDeclaration<T> v1,
            out PathVariableDeclaration<T> v2,
            out PathVariableDeclaration<T> v3,
            out PathVariableDeclaration<T> v4,
            out PathVariableDeclaration<T> v5,
            out PathVariableDeclaration<T> v6,
            out PathVariableDeclaration<T> v7,
            out PathVariableDeclaration<T> v8,
            out PathVariableDeclaration<T> v9,
            out PathVariableDeclaration<T> v10)
        {
            v1 = PathVariableDeclaration<T>.Default;
            v2 = PathVariableDeclaration<T>.Default;
            v3 = PathVariableDeclaration<T>.Default;
            v4 = PathVariableDeclaration<T>.Default;
            v5 = PathVariableDeclaration<T>.Default;
            v6 = PathVariableDeclaration<T>.Default;
            v7 = PathVariableDeclaration<T>.Default;
            v8 = PathVariableDeclaration<T>.Default;
            v9 = PathVariableDeclaration<T>.Default;
            v10 = PathVariableDeclaration<T>.Default;
        }
    }

    #endregion // PathDeconstructor
}
//}
