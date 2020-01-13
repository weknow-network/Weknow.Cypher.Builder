using System;
using System.Linq.Expressions;

namespace Weknow
{
    public interface ICypherAdvance
    {
        /// <summary>
        /// Node mutation by entity.
        /// </summary>
        ICypherEntityMutations Entity { get; }
        /// <summary>
        /// Nodes mutation by entities (use Unwind pattern).
        /// </summary>
        ICypherEntitiesMutations Entities { get; }
    }
}
