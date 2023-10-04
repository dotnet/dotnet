// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Contains methods to discover frameworks and compatibility between frameworks.
    /// </summary>
    [ComImport]
    [Guid("61C038E9-0308-482B-8602-279E42A3BB1E")]
    [Obsolete("This API does not support .NET 5 and higher target frameworks with platforms. Use IVsFrameworkCompatibility3 instead.")]
    public interface IVsFrameworkCompatibility2 : IVsFrameworkCompatibility
    {
        /// <summary>
        /// Selects the framework from <paramref name="frameworks"/> that is nearest
        /// to the <paramref name="targetFramework"/>, according to NuGet's framework
        /// compatibility rules. <see langword="null" /> is returned of none of the frameworks
        /// are compatible.
        /// </summary>
        /// <remarks>This API is <a href="https://github.com/microsoft/vs-threading/blob/main/doc/cookbook_vs.md#how-do-i-effectively-verify-that-my-code-is-fully-free-threaded">free-threaded.</a></remarks>
        /// <param name="targetFramework">The target framework.</param>
        /// <param name="fallbackTargetFrameworks">
        /// Target frameworks to use if the provided <paramref name="targetFramework"/> is not compatible.
        /// These fallback frameworks are attempted in sequence after <paramref name="targetFramework"/>.
        /// </param>
        /// <param name="frameworks">The list of frameworks to choose from.</param>
        /// <exception cref="ArgumentException">If any of the arguments are <see langword="null" />.</exception>
        /// <returns>The nearest framework.</returns>
        [Obsolete("This API does not support .NET 5 and higher target frameworks with platforms. Use IVsFrameworkCompatibility3 instead.")]
        FrameworkName GetNearest(
            FrameworkName targetFramework,
            IEnumerable<FrameworkName> fallbackTargetFrameworks,
            IEnumerable<FrameworkName> frameworks);
    }
}
