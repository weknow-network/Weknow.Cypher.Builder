using Weknow.CypherBuilder.Declarations;

namespace Weknow.CypherBuilder
{
    internal class Stub : ILabel, IRelation, IType, INode
    {
        public static readonly Stub Empty = new Stub();

        #region IRelation Members

        IRelation IRelation.this[IType type] => ((IRelation)Empty)[type];

        IRelation IRelation.this[VariableDeclaration var] => ((IRelation)Empty)[var];

        IRelation IRelation.this[Range r] => ((IRelation)Empty)[r];

        IRelation IRelation.this[Rng r] => ((IRelation)Empty)[r];

        IRelation IRelation.this[VariableDeclaration var, IType type] => ((IRelation)Empty)[var, type];

        IRelation IRelation.this[IType type, object properties] => ((IRelation)Empty)[type, properties];

        IRelation IRelation.this[VariableDeclaration var, Range r] => ((IRelation)Empty)[var, r];

        IRelation IRelation.this[VariableDeclaration var, Rng r] => ((IRelation)Empty)[var, r];

        IRelation IRelation.this[VariableDeclaration var, IType type, object properties] => ((IRelation)Empty)[var, type, properties];

        IRelation IRelation.this[VariableDeclaration var, IType type, object properties, Rng r] => ((IRelation)Empty)[var, type, properties, r];

        #endregion // IRelation Members
    }
}
