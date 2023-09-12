// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NETFRAMEWORK

System.Console.Error.WriteLine("Not supported on .NET Framework");
return 1;

#else

using System;
using System.Formats.Tar;
using System.IO;
using System.IO.Compression;

if (args is not [var srcPath, var dstPath])
{
    Console.Error.WriteLine("Usage: <src path> <dst path>");
    return 1;
}

try
{
    if (File.Exists(srcPath))
    {
        Directory.CreateDirectory(dstPath);

        using var srcStream = File.Open(srcPath, FileMode.Open);
        using var gzip = new GZipStream(srcStream, CompressionMode.Decompress);
        TarFile.ExtractToDirectory(gzip, dstPath, overwriteFiles: false);

    }
    else if (Directory.Exists(srcPath))
    {
        using var dstStream = File.Open(dstPath, FileMode.Create);
        using var gzip = new GZipStream(dstStream, CompressionMode.Compress);
        TarFile.CreateFromDirectory(srcPath, gzip, includeBaseDirectory: false);
    }
    else
    {
        Console.Error.WriteLine($"File or directory must exist: '{srcPath}'");
        return 1;
    }
}
catch (Exception e)
{
    Console.Error.Write(e.Message);
    return 1;
}

return 0;

#endif
