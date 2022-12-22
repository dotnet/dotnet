// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// This attribute is applied to <see cref="ITestDiscoverer"/>s. It indicates that the discoverer discovers tests
/// present in files with the specified extension.
/// </summary>
/// <remarks>
/// If neither this attribute nor the <see cref="DirectoryBasedTestDiscovererAttribute"/> is provided on the test
/// discoverer, it will be called for all relevant test files and directories.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class FileExtensionAttribute : Attribute
{
    /// <summary>
    /// Initializes with a file extension that the test discoverer can process tests from.
    /// For example ".dll" or ".exe".
    /// </summary>
    /// <param name="fileExtension">The file extensions that the test discoverer can process tests from.</param>
    public FileExtensionAttribute(string fileExtension)
    {
        ValidateArg.NotNullOrWhiteSpace(fileExtension, nameof(fileExtension));
        FileExtension = fileExtension;
    }

    /// <summary>
    /// A file extensions that the test discoverer can process tests from.  For example ".dll" or ".exe".
    /// </summary>
    public string FileExtension { get; private set; }

}
