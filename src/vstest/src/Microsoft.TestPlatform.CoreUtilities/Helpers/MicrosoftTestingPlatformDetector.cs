// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;

/// <summary>
/// Detects whether an assembly is a Microsoft.Testing.Platform application by reading the
/// <c>[assembly: AssemblyMetadata("Microsoft.Testing.Platform.Application", "true")]</c> attribute
/// that the Microsoft.Testing.Platform MSBuild targets stamp onto the entry assembly at build time.
/// </summary>
/// <remarks>
/// This lives in CoreUtilities so both the up-front detection in vstest.console and the routing
/// decision in the CrossPlatEngine TestEngine can share the exact same logic instead of duplicating
/// the (subtle) custom-attribute blob parsing.
/// </remarks>
internal static class MicrosoftTestingPlatformDetector
{
    private const string MicrosoftTestingPlatformApplicationMetadataKey = "Microsoft.Testing.Platform.Application";

    // Detection reads the assembly's PE metadata from disk, and a single run can ask about the same source
    // several times (grouping, provider resolution, and per-source proxy creation). Memoize per source path so
    // we read each assembly at most once. Concurrent because resolution can happen on parallel proxy threads.
    private static readonly ConcurrentDictionary<string, bool> Cache = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Returns <see langword="true"/> if the assembly at <paramref name="filePath"/> is a
    /// Microsoft.Testing.Platform application. Never throws; returns <see langword="false"/> on any error.
    /// </summary>
    public static bool IsMicrosoftTestingPlatformApp(string filePath)
    {
        if (filePath is null)
        {
            return false;
        }

        return Cache.GetOrAdd(filePath, static path =>
        {
            try
            {
                using var assemblyStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                var result = IsMicrosoftTestingPlatformApp(assemblyStream);
                EqtTrace.Info("MicrosoftTestingPlatformDetector.IsMicrosoftTestingPlatformApp: '{0}' for source: '{1}'", result, path);
                return result;
            }
            catch (Exception ex)
            {
                EqtTrace.Warning("MicrosoftTestingPlatformDetector.IsMicrosoftTestingPlatformApp: failed to read assembly metadata, exception: {0} for assembly: {1}", ex, path);
                return false;
            }
        });
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="assemblyStream"/> is a Microsoft.Testing.Platform
    /// application. The caller owns the stream lifetime.
    /// </summary>
    public static bool IsMicrosoftTestingPlatformApp(Stream assemblyStream)
    {
        using var peReader = new PEReader(assemblyStream);
        if (!peReader.HasMetadata)
        {
            return false;
        }

        var metadataReader = peReader.GetMetadataReader();

        // Microsoft.Testing.Platform applications are marked at build time with
        // [assembly: AssemblyMetadata("Microsoft.Testing.Platform.Application", "true")] by the
        // Microsoft.Testing.Platform MSBuild targets. We only look at assembly-level attributes.
        foreach (var handle in metadataReader.GetAssemblyDefinition().GetCustomAttributes())
        {
            var attribute = metadataReader.GetCustomAttribute(handle);
            if (!IsAssemblyMetadataAttribute(metadataReader, attribute))
            {
                continue;
            }

            try
            {
                // AssemblyMetadataAttribute has a (string key, string value) constructor. The custom attribute
                // blob is: 2-byte prolog (0x0001), then the two serialized strings, then the named-argument count.
                var blob = metadataReader.GetBlobReader(attribute.Value);
                if (blob.ReadUInt16() != 1)
                {
                    continue;
                }

                var key = blob.ReadSerializedString();
                var value = blob.ReadSerializedString();
                if (string.Equals(key, MicrosoftTestingPlatformApplicationMetadataKey, StringComparison.Ordinal)
                    && string.Equals(value, "true", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                EqtTrace.Verbose("MicrosoftTestingPlatformDetector.IsMicrosoftTestingPlatformApp: could not decode AssemblyMetadata attribute: {0}", ex);
            }
        }

        return false;
    }

    private static bool IsAssemblyMetadataAttribute(MetadataReader metadataReader, CustomAttribute attribute)
    {
        StringHandle typeNameHandle;
        StringHandle typeNamespaceHandle;
        switch (attribute.Constructor.Kind)
        {
            case HandleKind.MemberReference:
                var memberReference = metadataReader.GetMemberReference((MemberReferenceHandle)attribute.Constructor);
                switch (memberReference.Parent.Kind)
                {
                    case HandleKind.TypeReference:
                        var typeReference = metadataReader.GetTypeReference((TypeReferenceHandle)memberReference.Parent);
                        typeNameHandle = typeReference.Name;
                        typeNamespaceHandle = typeReference.Namespace;
                        break;
                    case HandleKind.TypeDefinition:
                        var typeDefinition = metadataReader.GetTypeDefinition((TypeDefinitionHandle)memberReference.Parent);
                        typeNameHandle = typeDefinition.Name;
                        typeNamespaceHandle = typeDefinition.Namespace;
                        break;
                    default:
                        return false;
                }

                break;

            case HandleKind.MethodDefinition:
                var methodDefinition = metadataReader.GetMethodDefinition((MethodDefinitionHandle)attribute.Constructor);
                var declaringType = metadataReader.GetTypeDefinition(methodDefinition.GetDeclaringType());
                typeNameHandle = declaringType.Name;
                typeNamespaceHandle = declaringType.Namespace;
                break;

            default:
                return false;
        }

        return string.Equals(metadataReader.GetString(typeNameHandle), "AssemblyMetadataAttribute", StringComparison.Ordinal)
            && string.Equals(metadataReader.GetString(typeNamespaceHandle), "System.Reflection", StringComparison.Ordinal);
    }
}
