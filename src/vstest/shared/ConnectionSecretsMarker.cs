// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

#pragma warning disable IDE0161 // Linked into servicing branches whose compilers predate file-scoped namespaces.
#pragma warning disable IDE0063 // A using statement cannot be simplified to a using declaration on C# 7.3 lines.
namespace Microsoft.VisualStudio.TestPlatform.Utilities
{
    /// <summary>
    /// Detects whether the process started from an assembly performs the connection identity check, by
    /// reading the <c>[assembly: AssemblyMetadata("Microsoft.TestPlatform.ConnectionSecrets", "true")]</c>
    /// marker compiled into it.
    ///
    /// The marker is a fact about that build. Deciding from the product version instead would mean
    /// predicting the patch number each servicing build ships as, and every package carrying that
    /// prediction would be wrong if the prediction did not come true.
    ///
    /// This is the same shape Microsoft.Testing.Platform uses to mark its applications, read the same
    /// way, so the subtle custom-attribute blob parsing is not invented twice.
    /// </summary>
    internal static class ConnectionSecretsMarker
    {
        private const string ConnectionSecretsMetadataKey = "Microsoft.TestPlatform.ConnectionSecrets";

        // A single run asks about the same file more than once (the capability check, then the launch
        // decision), so read each assembly at most once. Concurrent because proxies are created in
        // parallel.
        // ORDINAL, NOT OrdinalIgnoreCase. The key is the path exactly as the caller gave it, and nothing
        // else here folds case. On a case-sensitive filesystem two distinct binaries can differ only by
        // case, and a case-insensitive key would hand one of them the other's answer - an answer that
        // decides whether a peer is trusted. On Windows the cost is at most a duplicate entry for the
        // same file reached through two spellings, which is read twice and gives the same result.
        private static readonly ConcurrentDictionary<string, bool> Cache = new ConcurrentDictionary<string, bool>(StringComparer.Ordinal);

        /// <summary>
        /// Returns true when the assembly at the given path carries the marker. Never throws: anything
        /// that cannot be read is reported as unmarked, which refuses the peer rather than launching one
        /// whose identity could not be established.
        /// </summary>
        internal static bool IsMarked(string assemblyPath)
        {
            if (assemblyPath == null || assemblyPath.Length == 0)
            {
                return false;
            }

            return Cache.GetOrAdd(assemblyPath, ReadMarker);
        }

        private static bool ReadMarker(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                using (var peReader = new PEReader(stream))
                {
                    if (!peReader.HasMetadata)
                    {
                        return false;
                    }

                    var metadataReader = peReader.GetMetadataReader();
                    foreach (var handle in metadataReader.GetAssemblyDefinition().GetCustomAttributes())
                    {
                        var attribute = metadataReader.GetCustomAttribute(handle);
                        if (!IsAssemblyMetadataAttribute(metadataReader, attribute))
                        {
                            continue;
                        }

                        // AssemblyMetadataAttribute has a (string key, string value) constructor. The blob
                        // is a two byte prolog, then the two serialized strings.
                        var blob = metadataReader.GetBlobReader(attribute.Value);
                        if (blob.ReadUInt16() != 1)
                        {
                            continue;
                        }

                        var key = blob.ReadSerializedString();
                        var value = blob.ReadSerializedString();
                        if (string.Equals(key, ConnectionSecretsMetadataKey, StringComparison.Ordinal)
                            && string.Equals(value, "true", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EqtTrace.Verbose("ConnectionSecretsMarker.IsMarked: could not read '{0}', treating it as unmarked. {1}", path, ex);
                return false;
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
                            var referencedDefinition = metadataReader.GetTypeDefinition((TypeDefinitionHandle)memberReference.Parent);
                            typeNameHandle = referencedDefinition.Name;
                            typeNamespaceHandle = referencedDefinition.Namespace;
                            break;

                        default:
                            return false;
                    }

                    break;

                case HandleKind.MethodDefinition:
                    // The attribute can also be compiled into the assembly that carries it.
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
}
#pragma warning restore IDE0063
#pragma warning restore IDE0161