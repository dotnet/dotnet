// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI.Utility
{
    /// <summary>
    /// A <see cref="INuGetSearchService"/>, but the implementation is responsible for reconnecting when the service
    /// broker availability changes. Therefore, callers of this interface do not need to handle more than transitive failures.
    /// </summary>
    public interface IReconnectingNuGetSearchService : INuGetSearchService
    {
    }
}
