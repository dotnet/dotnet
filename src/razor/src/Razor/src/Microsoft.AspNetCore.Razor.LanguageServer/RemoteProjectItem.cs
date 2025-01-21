// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class RemoteProjectItem : RazorProjectItem
{
    public RemoteProjectItem(string filePath, string physicalPath, string? fileKind)
    {
        FilePath = filePath;
        PhysicalPath = physicalPath;
        FileKind = fileKind ?? FileKinds.GetFileKindFromFilePath(FilePath);
        RelativePhysicalPath = FilePath.StartsWith('/')
            ? FilePath[1..]
            : FilePath;
    }

    public override string BasePath => "/";

    public override string FilePath { get; }

    public override string PhysicalPath { get; }

    public override string FileKind { get; }

    public override string RelativePhysicalPath { get; }

    public override bool Exists
    {
        get
        {
            var platformPath = PhysicalPath[1..];

            return Path.IsPathRooted(platformPath)
                ? File.Exists(platformPath)
                : File.Exists(PhysicalPath);
        }
    }

    public override Stream Read()
        => throw new NotSupportedException();
}
