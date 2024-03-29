﻿using Weknow.CypherBuilder.Declarations;

namespace Weknow.CypherBuilder
{

    /// <summary>
    /// Relation primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// MATCH (n:Person)-[:KNOWS]->(m:Person)
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public interface IRelation : IPattern
    {
        /// <summary>
        /// Creates a mock object's node.
        /// </summary>
        /// <returns></returns>
        internal readonly static IRelation Fake = Stub.Empty;

        #region Indexers this[...]

        /// <summary>
        /// Represent relation with variable and type.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <example><![CDATA[(m)<-[r:KNOWS]-(n)]]></example>
        IRelation this[IType type] { [Cypher("[:$0]")] get; }
        /// <summary>
        /// Represent relation with variable and type.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation" />.
        /// </value>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        /// <example><![CDATA[(m)<-[r:KNOWS]-(n)]]></example>
        IRelation this[VariableDeclaration var] { [Cypher("[$0]")] get; }

        /// <summary>
        /// Represent relation with variable and type.
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <example>
        /// <![CDATA[(m)<-[r:KNOWS]-(n)]]>
        /// </example>
        IRelation this[VariableDeclaration var, IType type] { [Cypher("[$0$1]")] get; }

        /// <summary>
        /// Represent relation with variable, type and properties.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="var">The variable.</param>
        /// <param name="type">The type.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// <![CDATA[(m)<-[r:KNOWS {name: $name}]-(n)]]>
        /// </example>
        IRelation this[VariableDeclaration var, IType type, object properties] { [Cypher("[$0$1 $2]")] get; }
        /// <summary>
        /// Represent relation with variable, type and properties.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation" />.
        /// </value>
        /// <param name="type">The type.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[(m)<-[r:KNOWS {name: $name}]-(n)]]></example>
        IRelation this[IType type, object properties] { [Cypher("[:$0 $1]")] get; }
        ///// <summary>
        ///// Represent relation with variable and range.
        ///// </summary>
        ///// <value>
        ///// The <see cref="IRelation"/>.
        ///// </value>
        ///// <param name="var">Variable</param>
        ///// <param name="r">The range: https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#indices-and-ranges </param>
        ///// <returns></returns>
        ///// <example>
        ///// (n)-[r:*1..5]->(m)
        ///// </example>
        //IRelation this[VariableDeclaration var, Range r] { [Cypher("[$0$1]")] get; }
        /// <summary>
        /// Represent relation with range.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="r">The range: https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#indices-and-ranges </param>
        /// <returns></returns>
        /// <example>
        /// (n)-[*1..5]->(m)
        /// </example>
        IRelation this[System.Range r] { [Cypher("[*$0]")] get; }
        /// <summary>
        /// Represent relation with variable and range.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="var">Variable</param>
        /// <param name="r">The range: https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#indices-and-ranges </param>
        /// <returns></returns>
        /// <example>
        /// (n)-[r:*1..5]->(m)
        /// </example>
        IRelation this[VariableDeclaration var, System.Range r] { [Cypher("[$0*$1]")] get; }
        /// <summary>
        /// Represent relation with variable, type and range.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="type"></param>
        /// <param name="r">The range: https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#indices-and-ranges </param>
        /// <returns></returns>
        /// <example>
        /// (n)-[:KNOW*1..5 {level: 2}]->(m)
        /// </example>
        IRelation this[IType type, System.Range r] { [Cypher("[:$0*$1]")] get; }
        /// <summary>
        /// Represent relation with variable, type and range.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="var">Variable</param>
        /// <param name="type"></param>
        /// <param name="properties"></param>
        /// <param name="r">The range: https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#indices-and-ranges </param>
        /// <returns></returns>
        /// <example>
        /// (n)-[r:KNOW*1..5 {level: 2}]->(m)
        /// </example>
        IRelation this[VariableDeclaration var, IType type, object properties, System.Range r] { [Cypher("[$0$1 $2 *$3]")] get; }

        #endregion // Indexers this [...]

        #region Operators

        /// <summary>
        /// Declaration for operator -.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IRelationNode operator -(IRelationNode r1, IRelation r2) => throw new NotImplementedException();
        /// <summary>
        /// Declaration for operator -.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IRelationNode operator -(IRelation r1, IRelationNode r2) => throw new NotImplementedException();
        /// <summary>
        /// Declaration for operator -.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IRelationNode operator -(IRelation l, INode r) => throw new NotImplementedException();
        /// <summary>
        /// Declaration for operator -.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IRelation operator -(IRelation l, IRelation r) => throw new NotImplementedException();
        /// <summary>
        /// Declaration for operator &gt;.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IRelationNode operator >(IRelation l, INode r) => throw new NotImplementedException();
        /// <summary>
        /// Declaration for operator &gt;.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IRelation operator >(IRelation l, IRelation r) => throw new NotImplementedException();
        /// <summary>
        /// Declaration for operator &lt;.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IRelationNode operator <(IRelation l, INode r) => throw new NotImplementedException();
        /// <summary>
        /// Declaration for operator &lt;.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IRelation operator <(IRelation l, IRelation r) => throw new NotImplementedException();

        #endregion // Operators
    }

}
