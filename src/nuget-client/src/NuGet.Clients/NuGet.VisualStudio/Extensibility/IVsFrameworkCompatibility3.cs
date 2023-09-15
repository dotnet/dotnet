// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Contains methods to discover frameworks and compatibility between frameworks.
    /// </summary>
    [ComImport]
    [Guid("EBE9DECE-1E8F-4769-9D59-59CEE7558124")]
    public interface IVsFrameworkCompatibility3
    {
        /// <summary>
        /// Selects the framework from <paramref name="frameworks"/> that is nearest
        /// to the <paramref name="targetFramework"/>, according to NuGet's framework
        /// compatibility rules. <see langword="null" /> is returned of none of the frameworks
        /// are compatible.
        /// </summary>
        /// <param name="targetFramework">The target framework.</param>
        /// <param name="frameworks">The list of frameworks to choose from.</param>
        /// <exception cref="ArgumentNullException">If any of the arguments are null.</exception>
        /// <exception cref="ArgumentException">If any of the frameworks cannot be parsed.</exception>
        /// <returns>The nearest framework.</returns>
        /// <remarks>This API is <a href="https://github.com/microsoft/vs-threading/blob/9f065f155525c4561257e02ad61e66e93e073886/doc/cookbook_vs.md#how-do-i-effectively-verify-that-my-code-is-fully-free-threaded">free-threaded</a>.</remarks>
        IVsNuGetFramework GetNearest(IVsNuGetFramework targetFramework, IEnumerable<IVsNuGetFramework> frameworks);

        /// <summary>
        /// Selects the framework from <paramref name="frameworks"/> that is nearest
        /// to the <paramref name="targetFramework"/>, according to NuGet's framework
        /// compatibility rules. <see langword="null" /> is returned of none of the frameworks
        /// are compatible.
        /// </summary>
        /// <param name="targetFramework">The target framework.</param>
        /// <param name="fallbackTargetFrameworks">
        /// Target frameworks to use if the provided <paramref name="targetFramework"/> is not compatible.
        /// These fallback frameworks are attempted in sequence after <paramref name="targetFramework"/>.
        /// </param>
        /// <param name="frameworks">The list of frameworks to choose from.</param>
        /// <exception cref="ArgumentNullException">If any of the arguments are null.</exception>
        /// <exception cref="ArgumentException">If any of the frameworkscannot be parsed.</exception>
        /// <returns>The nearest framework.</returns>
        /// <remarks>This API is <a href="https://github.com/microsoft/vs-threading/blob/9f065f155525c4561257e02ad61e66e93e073886/doc/cookbook_vs.md#how-do-i-effectively-verify-that-my-code-is-fully-free-threaded">free-threaded</a>.</remarks>
        IVsNuGetFramework GetNearest(
            IVsNuGetFramework targetFramework,
            IEnumerable<IVsNuGetFramework> fallbackTargetFrameworks,
            IEnumerable<IVsNuGetFramework> frameworks);
    }
}
