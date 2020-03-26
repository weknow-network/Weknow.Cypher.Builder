using System.Linq.Expressions;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    class ContextExpression
    {
        public bool IsPluralize { get; set; }
        public bool IsSingularize { get; set; }
        public Expression Expression { get; set; }

        public ContextExpression(bool isPluralize, bool isSingularize, Expression expression)
        {
            IsPluralize = isPluralize;
            IsSingularize = isSingularize;
            Expression = expression;
        }
    }

}
