// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.VersionTools.Cli;

public interface IDirectoryProxy
{
    bool Exists(string path);
    string[] GetFiles(string path, string searchPattern, System.IO.SearchOption searchOption);
}

public interface IFileProxy
{
    void Move(string sourceFileName, string destFileName);
}
