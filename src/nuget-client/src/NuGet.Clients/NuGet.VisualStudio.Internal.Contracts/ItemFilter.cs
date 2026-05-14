// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio.Internal.Contracts
{
    public enum ItemFilter
    {
        /// <summary>
        /// The value All represents the Browse tab in PM UI
        /// </summary>
        All,
        Installed,
        UpdatesAvailable,
        Consolidate
    }
}
