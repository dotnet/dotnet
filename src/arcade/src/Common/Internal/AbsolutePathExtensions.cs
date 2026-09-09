// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;

namespace Microsoft.Build.Framework;

/// <summary>
/// Polyfill for <c>AbsolutePath.GetCanonicalForm()</c>, which is internal in the MSBuild packages
/// this repository currently consumes.
/// </summary>
/// <remarks>
/// <para>
/// <c>TaskEnvironment.GetAbsolutePath</c> absolutizes but deliberately does not canonicalize, so
/// <c>"foo/../bar"</c> stays <c>"&lt;project&gt;\foo/../bar"</c>. Code that previously called
/// <see cref="Path.GetFullPath(string)"/> got both, which matters wherever a path is compared,
/// deduplicated, or written to an output property, item metadata, or a generated file.
/// </para>
/// <para>
/// Only call this on paths that are already fully qualified, which is what
/// <c>TaskEnvironment.GetAbsolutePath</c> returns. <see cref="Path.GetFullPath(string)"/> would
/// otherwise consult per-drive process state for Windows rooted-but-not-fully-qualified inputs
/// such as <c>"C:foo"</c>, which is the process state this migration exists to remove.
/// </para>
/// <para>
/// TODO: delete this file and its &lt;Compile&gt; links once this repository consumes an MSBuild
/// that exposes the API - see https://github.com/dotnet/arcade/issues/17506. The swap is source
/// compatible because the real instance method takes precedence over this extension method.
/// </para>
/// </remarks>
internal static class AbsolutePathExtensions
{
    /// <summary>
    /// Returns the canonical form of an already fully qualified path, resolving <c>.</c> and
    /// <c>..</c> segments and normalizing directory separators.
    /// </summary>
    /// <remarks>
    /// Unlike the MSBuild implementation this does not preserve <c>OriginalValue</c>, because the
    /// constructor overload that carries it is internal.
    /// </remarks>
    internal static AbsolutePath GetCanonicalForm(this AbsolutePath path) =>
        // MSBuildTask0005 flags Path.GetFullPath transitively at every calling task. It is safe
        // here because the input is already fully qualified, so no process state is consulted.
        // The suppression goes away with the file - see https://github.com/dotnet/arcade/issues/17506.
#pragma warning disable MSBuildTask0005
        new(Path.GetFullPath(path.Value));
#pragma warning restore MSBuildTask0005
}
