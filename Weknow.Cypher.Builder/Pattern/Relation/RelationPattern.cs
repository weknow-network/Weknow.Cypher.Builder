using System;
using System.Linq.Expressions;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Represent predefine pattern
    /// </summary>
    /// <seealso cref="Weknow.Cypher.Builder.IPattern" />
    public class RelationPattern : ExpressionPattern, IRelation
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        public RelationPattern(Expression expression, CypherConfig configuration)
            :base(expression, configuration)
        {
        }

        #endregion // Ctor

        #region Indexer

        /// <summary>
        /// Gets the <see cref="IRelation"/> with the specified variable.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="var">The variable.</param>
        /// <param name="type">The type.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        
        IRelation IRelation.this[IVar var, IType type, IProperties properties, Rng r] => throw new NotImplementedException();
        /// <summary>
        /// Gets the <see cref="IRelation"/> with the specified variable.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        
        IRelation IRelation.this[IVar var] => throw new NotImplementedException();

        /// <summary>
        /// Gets the <see cref="IRelation"/> with the specified variable.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="var">The variable.</param>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        
        IRelation IRelation.this[IVar var, Rng r] => throw new NotImplementedException();

        /// <summary>
        /// Gets the <see cref="IRelation"/> with the specified r.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        
        IRelation IRelation.this[Rng r] => throw new NotImplementedException();

        /// <summary>
        /// Gets the <see cref="IRelation"/> with the specified variable.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="var">The variable.</param>
        /// <param name="type">The type.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        
        IRelation IRelation.this[IVar var, IType type, IProperties properties, Range r] => throw new NotImplementedException();

        /// <summary>
        /// Gets the <see cref="IRelation"/> with the specified variable.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="var">The variable.</param>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        
        IRelation IRelation.this[IVar var, Range r] => throw new NotImplementedException();

        /// <summary>
        /// Gets the <see cref="IRelation"/> with the specified r.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        
        IRelation IRelation.this[Range r] => throw new NotImplementedException();

        /// <summary>
        /// Gets the <see cref="IRelation"/> with the specified variable.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="var">The variable.</param>
        /// <param name="type">The type.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        
        IRelation IRelation.this[IVar var, IType type, IProperties properties] => throw new NotImplementedException();

        /// <summary>
        /// Gets the <see cref="IRelation"/> with the specified variable.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="var">The variable.</param>
        /// <param name="type">The type.</param>
        /// <param name="property"></param>
        /// <returns></returns>

        IRelation IRelation.this[IVar var, IType type, params IProperty[] property] => throw new NotImplementedException();

        /// <summary>
        /// Gets the <see cref="IRelation"/> with the specified variable.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="var">The variable.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        
        IRelation IRelation.this[IVar var, IType type] => throw new NotImplementedException();

        /// <summary>
        /// Gets the <see cref="IRelation"/> with the specified type.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        
        IRelation IRelation.this[IType type] => throw new NotImplementedException();

        #endregion // Indexer
    }
}
