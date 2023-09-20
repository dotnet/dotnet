// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;

namespace NuGet.Packaging.Core
{
    /// <summary>
    /// Callback invoked to extract a package file.
    /// </summary>
    /// <param name="sourceFile">The path of the file in the package.</param>
    /// <param name="targetPath">The path to write to.</param>
    /// <param name="fileStream">The file <see cref="Stream"/>.</param>
    /// <returns>The file name if the file was written; otherwise <see langword="null" />.</returns>
    public delegate string ExtractPackageFileDelegate(string sourceFile, string targetPath, Stream fileStream);
}
