// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers.Interfaces;

/// <summary>
/// Helper class for getting info about dotnet host.
/// </summary>
public interface IDotnetHostHelper
{
    /// <summary>
    /// Gets the full path for of .net core host.
    /// </summary>
    /// <returns>Full path to <c>dotnet</c> executable</returns>
    /// <remarks>Debuggers require the full path of executable to launch it.</remarks>
    string GetDotnetPath();

    /// <summary>
    /// Gets the full path of mono host.
    /// </summary>
    /// <returns>Full path to <c>mono</c> executable</returns>
    string GetMonoPath();

    /// <summary>
    /// Try to locate muxer of specific architecture
    /// </summary>
    /// <param name="targetArchitecture">Specific architecture</param>
    /// <param name="dotnetMuxerResolutionStrategy">The dotnet muxer resolution strategy.</param>
    /// <param name="muxerPath">Path to the muxer</param>
    /// <returns>True if native muxer is found</returns>
    bool TryGetDotnetPathByArchitecture(
        PlatformArchitecture targetArchitecture,
        DotnetMuxerResolutionStrategy dotnetMuxerResolutionStrategy,
        out string? muxerPath);
}
