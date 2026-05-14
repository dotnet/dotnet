// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;

namespace Microsoft.DotNet.VersionTools.Cli;

public class DirectoryProxy : IDirectoryProxy
{
    public bool Exists(string path)
    {
        return Directory.Exists(path);
    }

    public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
    {
        return Directory.GetFiles(path, searchPattern, searchOption);
    }
}

public class FileProxy : IFileProxy
{
    public void Move(string sourceFileName, string destFileName)
    {
        File.Move(sourceFileName, destFileName);
    }
}
