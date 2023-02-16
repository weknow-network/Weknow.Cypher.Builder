using Weknow.CypherBuilder.Declarations;

namespace Weknow.CypherBuilder
{
    internal class Stub : ILabel, IRelation, IType, INode, IEnumerable<IType>
    {
        public static readonly Stub Empty = new Stub();

        #region IRelation Members

        IRelation IRelation.this[IType type] => ((IRelation)Empty)[type];

        IRelation IRelation.this[VariableDeclaration var] => ((IRelation)Empty)[var];

        //IRelation IRelation.this[Range r] => ((IRelation)Empty)[r];

        IRelation IRelation.this[Range r] => ((IRelation)Empty)[r];

        IRelation IRelation.this[VariableDeclaration var, IType type] => ((IRelation)Empty)[var, type];

        IRelation IRelation.this[IType type, object properties] => ((IRelation)Empty)[type, properties];

        //IRelation IRelation.this[VariableDeclaration var, Range r] => ((IRelation)Empty)[var, r];

        IRelation IRelation.this[VariableDeclaration var, Range r] => ((IRelation)Empty)[var, r];

        IRelation IRelation.this[VariableDeclaration var, IType type, object properties] => ((IRelation)Empty)[var, type, properties];

        IRelation IRelation.this[IType type, Range r] => ((IRelation)Empty)[type, r];

        IRelation IRelation.this[VariableDeclaration var, IType type, object properties, Range r] => ((IRelation)Empty)[var, type, properties, r];

        public IEnumerator<IType> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion // IRelation Members
    }
}
