// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Shell.TableControl;

namespace NuGet.SolutionRestoreManager.ErrorListFixers
{
    /// <summary>
    /// Inspects Error List entries to determine whether a fixer applies to them.
    /// </summary>
    internal interface IErrorListEntryInspector
    {
        /// <summary>
        /// Determines whether the <paramref name="entry"/> carries an error code that is supported by the fixer.
        /// </summary>
        /// <param name="entry">The Error List entry to inspect.</param>
        /// <returns><see langword="true"/> if the entry's code is supported; otherwise <see langword="false"/>.</returns>
        bool IsSupportedCode(ITableEntryHandle entry);
    }
}
