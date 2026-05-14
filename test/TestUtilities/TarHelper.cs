// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Formats.Tar;
using System.IO;

namespace TestUtilities;

public static class TarHelper
{
    /// <summary>
    /// Enumerates tar entry names from a stream. Callers are responsible for
    /// providing the appropriate stream (e.g. GZipStream for .tar.gz, raw stream for .tar).
    /// </summary>
    public static IEnumerable<string> GetEntryNames(Stream stream)
    {
        using TarReader reader = new(stream);
        TarEntry? entry;
        while ((entry = reader.GetNextEntry()) is not null)
        {
            yield return entry.Name;
        }
    }
}
