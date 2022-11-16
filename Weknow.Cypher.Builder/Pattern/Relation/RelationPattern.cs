using System.Linq.Expressions;

using Weknow.CypherBuilder.Declarations;

namespace Weknow.CypherBuilder
{
    /// <summary>
    /// Represent predefine pattern
    /// </summary>
    /// <seealso cref="Weknow.CypherBuilder.IPattern" />
    public class RelationPattern : ExpressionPattern, IRelation
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        public RelationPattern(Expression expression, CypherConfig configuration)
            : base(expression, configuration)
        {
        }

        #endregion // Ctor

        #region Indexer


        /// <summary>
        /// Gets the <see cref="IRelation" /> with the specified variable.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation" />.
        /// </value>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        IRelation IRelation.this[VariableDeclaration var] => throw new NotImplementedException();

        /// <summary>
        /// Gets the <see cref="IRelation" /> with the specified variable.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation" />.
        /// </value>
        /// <param name="type">The type.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        IRelation IRelation.this[IType type, object properties] => throw new NotImplementedException();

        IRelation IRelation.this[VariableDeclaration var, IType type, object properties] => throw new NotImplementedException();

        /// <summary>
        /// Gets the <see cref="IRelation"/> with the specified variable.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="var">The variable.</param>
        /// <param name="r">The r.</param>
        /// <returns></returns>

        IRelation IRelation.this[VariableDeclaration var, Rng r] => throw new NotImplementedException();

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

        IRelation IRelation.this[VariableDeclaration var, IType type, object properties, Rng r] => throw new NotImplementedException();

        /// <summary>
        /// Gets the <see cref="IRelation"/> with the specified variable.
        /// </summary>
        /// <value>
        /// The <see cref="IRelation"/>.
        /// </value>
        /// <param name="var">The variable.</param>
        /// <param name="r">The r.</param>
        /// <returns></returns>

        IRelation IRelation.this[VariableDeclaration var, Range r] => throw new NotImplementedException();

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
        /// <returns></returns>

        IRelation IRelation.this[VariableDeclaration var, IType type] => throw new NotImplementedException();

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

        #region IRelation this[VariableDeclaration var, IType type, object properties, Rng r]

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
        /// <exception cref="NotImplementedException"></exception>
        public IRelation this[VariableDeclaration var, IType type, object properties, Rng r] => throw new NotImplementedException();

        #endregion // IRelation this[VariableDeclaration var, IType type, object properties, Rng r]
    }
}
