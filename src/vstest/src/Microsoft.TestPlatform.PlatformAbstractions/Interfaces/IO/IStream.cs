// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

/// <summary>
/// Helper class to return platform specific stream.
/// </summary>
public interface IStream
{
    /// <summary>
    /// Returns platform specific Buffered Stream with desired buffer size.
    /// </summary>
    /// <param name="stream">Input Stream</param>
    /// <param name="bufferSize">Buffer Size</param>
    /// <returns>Buffered Stream</returns>
    Stream CreateBufferedStream(Stream stream, int bufferSize);
}
