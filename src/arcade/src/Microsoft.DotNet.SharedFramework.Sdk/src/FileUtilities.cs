// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Microsoft.DotNet.SharedFramework.Sdk;

internal static partial class FileUtilities
{
    private static readonly HashSet<string> s_assemblyExtensions = new HashSet<string>(
        new[] { ".dll", ".exe" },
        StringComparer.OrdinalIgnoreCase);

    public static Version GetFileVersion(FileInfo sourceFile)
    {
        var fvi = FileVersionInfo.GetVersionInfo(sourceFile.FullName);

        if (fvi != null)
        {
            return new Version(fvi.FileMajorPart, fvi.FileMinorPart, fvi.FileBuildPart, fvi.FilePrivatePart);
        }

        return null;
    }

    public static AssemblyName GetAssemblyName(FileInfo file)
    {
        if (!s_assemblyExtensions.Contains(file.Extension))
        {
            return null;
        }

        try
        {
            using (var stream = File.OpenRead(file.FullName))
            using (var peReader = new PEReader(stream))
            {
                if (peReader.HasMetadata)
                {
                    return peReader.GetMetadataReader().GetAssemblyDefinition().GetAssemblyName();
                }
            }
        }
        catch (BadImageFormatException)
        {
            // Not a valid assembly.
            return null;
        }
        return null;
    }
}

internal struct AssemblyInformation
{
    public string SimpleName { get; set; }

    public Version Version { get; set; }

    public string PublicKeyToken { get; set; }
}
