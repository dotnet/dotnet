// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Runtime.InteropServices;
using EnvDTE;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Contains methods to uninstall packages from a project within the current solution.
    /// </summary>
    [ComImport]
    [Guid("AF63941E-6BA8-4FEC-9827-8E4D1113F231")]
    public interface IVsPackageUninstaller
    {
        /// <summary>
        /// Uninstall the specified package from a project and specify whether to uninstall its dependency packages
        /// too.
        /// </summary>
        /// <remarks>Can be called from a background thread, if the UI thread is not blocked waiting for this to finish.
        /// See <a href="https://github.com/nuget/home/issues/11476">https://github.com/nuget/home/issues/11476</a></remarks>
        /// <param name="project">The project from which the package is uninstalled.</param>
        /// <param name="packageId">The package to be uninstalled</param>
        /// <param name="removeDependencies">
        /// A boolean to indicate whether the dependency packages should be
        /// uninstalled too.
        /// </param>
        void UninstallPackage(Project project, string packageId, bool removeDependencies);
    }
}
