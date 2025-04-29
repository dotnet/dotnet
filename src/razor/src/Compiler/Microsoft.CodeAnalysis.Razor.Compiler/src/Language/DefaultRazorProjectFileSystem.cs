// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Microsoft.AspNetCore.Razor.Language;

internal class DefaultRazorProjectFileSystem : RazorProjectFileSystem
{
    public DefaultRazorProjectFileSystem(string root)
    {
        ArgHelper.ThrowIfNullOrEmpty(root);

        // If "/" is passed in, we want that to be the value of root. We don't want root to end up
        // as an empty string.
        if (root == DefaultBasePath)
        {
            Root = DefaultBasePath;
        }
        else
        {
            root = root.Replace('\\', '/').TrimEnd('/');

            // Was the entire string just repeated '\' and '/' characters? If so, that's an invalid path.
            // Just throw instead of setting Root to an empty string.
            if (root.Length == 0)
            {
                ThrowHelper.ThrowArgumentException(nameof(root), $"Invalid path provided.");
            }

            Root = root;
        }
    }

    public string Root { get; }

    public override IEnumerable<RazorProjectItem> EnumerateItems(string basePath)
    {
        var absoluteBasePath = NormalizeAndEnsureValidPath(basePath);

        if (!Directory.Exists(absoluteBasePath))
        {
            yield break;
        }

        foreach (var filePath in Directory.EnumerateFiles(absoluteBasePath, "*.cshtml", SearchOption.AllDirectories))
        {
            yield return CreateItem(filePath, fileKind: null, basePath, absoluteBasePath);
        }

        foreach (var filePath in Directory.EnumerateFiles(absoluteBasePath, "*.razor", SearchOption.AllDirectories))
        {
            yield return CreateItem(filePath, fileKind: null, basePath, absoluteBasePath);
        }
    }

    public override RazorProjectItem GetItem(string path, string? fileKind)
    {
        var absoluteBasePath = Root;
        var absolutePath = NormalizeAndEnsureValidPath(path);

        if (!absolutePath.StartsWith(absoluteBasePath, StringComparison.OrdinalIgnoreCase))
        {
            return ThrowHelper.ThrowInvalidOperationException<RazorProjectItem>($"The file '{absolutePath}' is not a descendent of the base path '{absoluteBasePath}'.");
        }

        return CreateItem(absolutePath, fileKind, DefaultBasePath, absoluteBasePath);
    }

    private static DefaultRazorProjectItem CreateItem(string path, string? fileKind, string basePath, string absoluteBasePath)
    {
        var physicalPath = Path.GetFullPath(path);
        var relativePhysicalPath = physicalPath[(absoluteBasePath.Length + 1)..]; // Don't include leading separator

        var filePath = "/" + relativePhysicalPath.Replace(Path.DirectorySeparatorChar, '/');

        return new DefaultRazorProjectItem(basePath, filePath, physicalPath, relativePhysicalPath, fileKind, cssScope: null);
    }

    protected override string NormalizeAndEnsureValidPath(string path)
    {
        // PERF: If we're asked to normalize "/", there's no need to compare and manipulate strings to
        // ultimately return the value of Root.
        if (path == DefaultBasePath)
        {
            return Root;
        }

        ArgHelper.ThrowIfNullOrEmpty(path);

        var normalizedPath = path.Replace('\\', '/');

        // Check if the given path is an absolute path. It is absolute if...
        //
        // 1. It is a network share path and starts with a '//' (e.g. //server/some/network/folder) or...
        // 2. It starts with Root
        if (normalizedPath is ['/', '/', ..] ||
            normalizedPath.StartsWith(Root, StringComparison.OrdinalIgnoreCase))
        {
            return normalizedPath;
        }

        // This is not an absolute path, so we combine it with Root to produce the final path.

        // If the root doesn't end in a '/', and the path doesn't start with a '/', we'll need to add one.
        var needsSlash = Root[^1] is not '/' && normalizedPath[0] is not '/';
        var length = Root.Length + normalizedPath.Length + (needsSlash ? 1 : 0);

        return StringExtensions.CreateString(
            length,
            state: (Root, normalizedPath, needsSlash),
            static (span, state) =>
            {
                var (root, normalizedPath, needsSlash) = state;

                var rootSpan = root.AsSpan();
                var pathSpan = normalizedPath.AsSpan();

                // Copy the root first.
                rootSpan.CopyTo(span);
                span = span[rootSpan.Length..];

                // Add a slash if we need one.
                if (needsSlash)
                {
                    span[0] = '/';
                    span = span[1..];
                }

                // Finally, add the path.
                Debug.Assert(span.Length == pathSpan.Length, "The span should be the same length as the path.");
                pathSpan.CopyTo(span);
            });
    }
}
