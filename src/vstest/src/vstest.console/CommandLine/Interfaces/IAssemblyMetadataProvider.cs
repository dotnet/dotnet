// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.Versioning;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// Defines interface for assembly properties.
/// </summary>
internal interface IAssemblyMetadataProvider
{
    /// <summary>
    /// Determines FrameworkName from filePath.
    /// </summary>
    FrameworkName GetFrameworkName(string filePath);

    /// <summary>
    /// Determines Architecture from filePath.
    /// </summary>
    Architecture GetArchitecture(string filePath);

    /// <summary>
    /// Determines whether the assembly at <paramref name="filePath"/> is a Microsoft.Testing.Platform (MTP)
    /// application, i.e. it is marked with
    /// <c>[assembly: AssemblyMetadata("Microsoft.Testing.Platform.Application", "true")]</c>.
    /// </summary>
    bool IsMicrosoftTestingPlatformApp(string filePath);
}
