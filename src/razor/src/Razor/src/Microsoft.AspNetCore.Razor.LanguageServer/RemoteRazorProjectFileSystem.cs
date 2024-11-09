// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class RemoteRazorProjectFileSystem : RazorProjectFileSystem
{
    private readonly string _root;

    public RemoteRazorProjectFileSystem(string root)
    {
        if (root is null)
        {
            throw new ArgumentNullException(nameof(root));
        }

        _root = FilePathNormalizer.NormalizeDirectory(root);
    }

    public override IEnumerable<RazorProjectItem> EnumerateItems(string basePath)
    {
        throw new NotImplementedException();
    }

    public override RazorProjectItem GetItem(string path)
    {
        return GetItem(path, fileKind: null);
    }

    public override RazorProjectItem GetItem(string path, string? fileKind)
    {
        if (path is null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        var physicalPath = NormalizeAndEnsureValidPath(path);
        if (FilePathRootedBy(physicalPath, _root))
        {
            var filePath = physicalPath[_root.Length..];
            return new RemoteProjectItem(filePath, physicalPath, fileKind);
        }
        else
        {
            // File does not belong to this file system.
            // In practice this should never happen, the systems above this should have routed the
            // file request to the appropriate file system. Return something reasonable so a higher
            // layer falls over to provide a better error.
            return new RemoteProjectItem(physicalPath, physicalPath, fileKind);
        }
    }

    protected override string NormalizeAndEnsureValidPath(string path)
    {
        var absolutePath = path;
        if (!FilePathRootedBy(absolutePath, _root))
        {
            if (IsPathRootedForPlatform(absolutePath))
            {
                // Existing path is already rooted, can't translate from relative to absolute.
                return absolutePath;
            }

            if (path[0] == '/' || path[0] == '\\')
            {
                path = path[1..];
            }

            absolutePath = _root + path;
        }

        absolutePath = FilePathNormalizer.Normalize(absolutePath);
        return absolutePath;

        static bool IsPathRootedForPlatform(string path)
        {
            if (PlatformInformation.IsWindows && path == "/")
            {
                // We have to special case windows and "/" because for some reason Path.IsPathRooted returns true on windows for a single "/" path.
                return false;
            }

            if (!Path.IsPathRooted(path))
            {
                return false;
            }

            return true;
        }
    }

    internal bool FilePathRootedBy(string path, string root)
    {
        if (path.Length < root.Length)
        {
            return false;
        }

        var potentialRoot = path[..root.Length];
        if (FilePathComparer.Instance.Equals(potentialRoot, root))
        {
            return true;
        }

        return false;
    }
}
