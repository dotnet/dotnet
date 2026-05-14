// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Define multiple sources to trigger restore.
    /// </summary>
    public enum RestoreOperationSource
    {
        /// <summary>
        /// When restore is trigger through OnBuild event.
        /// </summary>
        OnBuild = 0,

        /// <summary>
        /// When restore is trigger through manually from UI.
        /// </summary>
        Explicit = 1,

        /// <summary>
        /// Auto restore with NuGet restore manager.
        /// </summary>
        Implicit = 2,
    }
}
