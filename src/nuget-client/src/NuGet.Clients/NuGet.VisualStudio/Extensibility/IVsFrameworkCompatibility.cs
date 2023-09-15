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
    [Guid("3B742C14-3DCB-463D-9198-F0C004BF65DD")]
    public interface IVsFrameworkCompatibility
    {
        /// <summary>
        /// Gets all .NETStandard frameworks currently supported, in ascending order by version.
        /// </summary>
        /// <remarks>This API is <a href="https://github.com/microsoft/vs-threading/blob/main/doc/cookbook_vs.md#how-do-i-effectively-verify-that-my-code-is-fully-free-threaded">free-threaded.</a></remarks>
        IEnumerable<FrameworkName> GetNetStandardFrameworks();

        /// <summary>
        /// Gets frameworks that support packages of the provided .NETStandard version.
        /// </summary>
        /// <remarks>
        /// The result list is not exhaustive as it is meant to human-readable. For example,
        /// equivalent frameworks are not returned. Additionally, a framework name with version X
        /// in the result implies that framework names with versions greater than or equal to X
        /// but having the same <see cref="FrameworkName.Identifier"/> are also supported.
        ///
        /// <para>This API is <a href="https://github.com/microsoft/vs-threading/blob/main/doc/cookbook_vs.md#how-do-i-effectively-verify-that-my-code-is-fully-free-threaded">free-threaded.</a></para>
        /// </remarks>
        /// <param name="frameworkName">The .NETStandard version to get supporting frameworks for.</param>
        [Obsolete("This API does not support .NET 5 and higher target frameworks with platforms. Use IVsFrameworkCompatibility3 instead.")]
        IEnumerable<FrameworkName> GetFrameworksSupportingNetStandard(FrameworkName frameworkName);

        /// <summary>
        /// Selects the framework from <paramref name="frameworks"/> that is nearest
        /// to the <paramref name="targetFramework"/>, according to NuGet's framework
        /// compatibility rules. <see langword="null" /> is returned of none of the frameworks
        /// are compatible.
        /// </summary>
        /// <remarks>This API is <a href="https://github.com/microsoft/vs-threading/blob/main/doc/cookbook_vs.md#how-do-i-effectively-verify-that-my-code-is-fully-free-threaded">free-threaded.</a></remarks>
        /// <param name="targetFramework">The target framework.</param>
        /// <param name="frameworks">The list of frameworks to choose from.</param>
        /// <exception cref="ArgumentException">If any of the arguments are <see langword="null" />.</exception>
        /// <returns>The nearest framework.</returns>
        [Obsolete("This API does not support .NET 5 and higher target frameworks with platforms. Use IVsFrameworkCompatibility3 instead.")]
        FrameworkName GetNearest(FrameworkName targetFramework, IEnumerable<FrameworkName> frameworks);
    }
}
