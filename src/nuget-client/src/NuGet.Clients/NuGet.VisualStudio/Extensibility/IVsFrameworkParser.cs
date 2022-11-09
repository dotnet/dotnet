// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// An interface for dealing with the conversion between strings and <see cref="FrameworkName"/>
    /// instances.
    /// </summary>
    [ComImport]
    [Guid("E6335CF1-70FE-416A-BA4B-4E92D90934A1")]
    [Obsolete("This API does not support .NET 5 and higher target frameworks with platforms. Use IVsFrameworkParser2 instead.")]
    public interface IVsFrameworkParser
    {
        /// <summary>
        /// Parses a short framework name (e.g. "net45") or a full framework name
        /// (e.g. ".NETFramework,Version=v4.5") into a <see cref="FrameworkName"/>
        /// instance.
        /// </summary>
        /// <remarks>This API is <a href="https://github.com/microsoft/vs-threading/blob/main/doc/cookbook_vs.md#how-do-i-effectively-verify-that-my-code-is-fully-free-threaded">free-threaded.</a></remarks>
        /// <param name="shortOrFullName">The framework string.</param>
        /// <exception cref="ArgumentNullException">If the provided string is null.</exception>
        /// <exception cref="ArgumentException">If the provided string cannot be parsed.</exception>
        /// <returns>The parsed framework.</returns>
        [Obsolete("This API does not support .NET 5 and higher target frameworks with platforms. Use IVsFrameworkParser2 instead.")]
        FrameworkName ParseFrameworkName(string shortOrFullName);

        /// <summary>
        /// Gets the shortened version of the framework name from a <see cref="FrameworkName"/>
        /// instance.
        /// </summary>
        /// <remarks>
        /// For example, ".NETFramework,Version=v4.5" is converted to "net45". This is the value
        /// used inside of .nupkg folder structures as well as in project.json files.
        /// <para>This API is <a href="https://github.com/microsoft/vs-threading/blob/main/doc/cookbook_vs.md#how-do-i-effectively-verify-that-my-code-is-fully-free-threaded">free-threaded.</a></para>
        /// </remarks>
        /// <param name="frameworkName">The framework name.</param>
        /// <exception cref="ArgumentNullException">If the input is null.</exception>
        /// <exception cref="ArgumentException">
        /// If the provided framework name cannot be converted to a short name.
        /// </exception>
        /// <returns>The short framework name. </returns>
        [Obsolete("This API does not support .NET 5 and higher target frameworks with platforms. Use IVsFrameworkParser2 instead.")]
        string GetShortFrameworkName(FrameworkName frameworkName);
    }
}
