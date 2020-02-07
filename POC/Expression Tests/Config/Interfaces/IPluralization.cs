using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;

// Pluralize libraries:
// https://github.com/sarathkcm/Pluralize.NET
// https://github.com/rvegajr/Pluralize.NET.Core


namespace Weknow
{
    /// <summary>
    /// Pluralization services.
    /// </summary>
    public interface IPluralization
    {
        /// <summary>
        /// Pluralize a word using the service.
        /// </summary>
        /// <param name="word">The word to pluralize.</param>
        /// <returns>The pluralized word </returns>
        string Pluralize(string word);

        /// <summary>
        /// Singularize a word using the service.
        /// </summary>
        /// <param name="word">The word to singularize.</param>
        /// <returns>The singularized word.</returns>
        string Singularize(string word);
    }
}
