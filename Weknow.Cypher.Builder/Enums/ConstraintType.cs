﻿using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.GraphDbCommands
{

    /// <summary>
    /// Behaviors of Ignore Context.
    /// </summary>
    public enum ConstraintType
    {
        /// <summary>
        /// IS UNIQUE
        /// </summary>
        IsUnique,
        /// <summary>
        /// IS NOT NULL
        /// </summary>
        IsNotNull,
        /// <summary>
        /// IS NODE KEY
        /// </summary>
        IsNodeKey
    }
}