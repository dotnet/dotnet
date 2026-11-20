// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace NuGet.VisualStudio
{
    public interface INuGetFeatureFlagInitializer
    {
        /// <summary>
        /// Initializes the AppContext switches for feature flags and experiments shared with core libraries.
        /// Each entry point must await this before invoking code that reads those switches.
        /// </summary>
        /// <remarks>
        /// Other packages and MEF APIs can start operations before NuGetPackage initializes.
        /// Lookup failures are logged, leave switches unchanged, and are not retried until Visual Studio restarts.
        /// </remarks>
        Task InitializeAsync();
    }
}
