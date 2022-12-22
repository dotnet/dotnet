// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Runtime.InteropServices;
using EnvDTE;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Contains methods to restore packages installed in a project within the current solution.
    /// </summary>
    [ComImport]
    [Guid("CAAEA598-1393-48E7-9617-1C5A62E38ABD")]
    public interface IVsPackageRestorer
    {
        /// <summary>
        /// Returns a value indicating whether the user consent to download NuGet packages
        /// has been granted.
        /// </summary>
        /// <remarks>Can be called from a background thread.</remarks>
        /// <returns>true if the user consent has been granted; otherwise, false.</returns>
        bool IsUserConsentGranted();

        /// <summary>
        /// Restores NuGet packages installed in the given project within the current solution.
        /// </summary>
        /// <remarks>Can be called from a background thread.</remarks>
        /// <param name="project">The project whose NuGet packages to restore.</param>
        void RestorePackages(Project project);
    }
}
