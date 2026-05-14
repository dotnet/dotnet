// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Common.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.Common.Interfaces;

/// <summary>
/// Metadata that is available for input test source, e.g. Whether it is native or managed dll, etc..
/// </summary>
public interface IAssemblyProperties
{
    /// <summary>
    /// Determines assembly type from file.
    /// </summary>
    AssemblyType GetAssemblyType(string filePath);
}
