// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// This attribute is applied to <see cref="ITestDiscoverer"/>s. It indicates the test discoverer discovers tests
/// present inside a directory (as opposed to the <see cref="FileExtensionAttribute"/> which indicates that the
/// discoverer discovers tests present in files with a specified extension).
/// </summary>
/// <remarks>
/// If neither this attribute nor the <see cref="FileExtensionAttribute"/> is provided on the test discoverer,
/// it will be called for all relevant test files and directories.
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
public sealed class DirectoryBasedTestDiscovererAttribute : Attribute
{
}
