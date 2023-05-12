// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

internal static partial class ObjectReaders
{
    private static readonly StringCache s_stringCache = new();

    [return: NotNullIfNotNull(nameof(str))]
    private static string? Cached(string? str)
    {
        if (str is null)
        {
            return null;
        }

        // Some of the strings used in TagHelperDescriptors are interned by other processes,
        // so we should avoid duplicating those.
        var interned = string.IsInterned(str);
        if (interned != null)
        {
            return interned;
        }

        // We cache all our stings here to prevent them from balooning memory in our Descriptors.
        return s_stringCache.GetOrAddValue(str);
    }

    public static RazorExtension ReadExtensionFromProperties(JsonDataReader reader)
    {
        var extensionName = reader.ReadNonNullString(nameof(RazorExtension.ExtensionName));

        return new SerializedRazorExtension(extensionName);
    }

    public static RazorConfiguration ReadConfigurationFromProperties(JsonDataReader reader)
    {
        ConfigurationData data = default;
        reader.ReadProperties(ref data, ConfigurationData.PropertyMap);

        return RazorConfiguration.Create(data.LanguageVersion, data.ConfigurationName, data.Extensions);
    }

    public static RazorDiagnostic ReadDiagnosticFromProperties(JsonDataReader reader)
    {
        DiagnosticData data = default;
        reader.ReadProperties(ref data, DiagnosticData.PropertyMap);

        var descriptor = new RazorDiagnosticDescriptor(data.Id, MessageFormat(data.Message), data.Severity);

        return RazorDiagnostic.Create(descriptor, data.Span);

        static Func<string> MessageFormat(string message)
        {
            return () => message;
        }
    }

    public static DocumentSnapshotHandle ReadDocumentSnapshotHandleFromProperties(JsonDataReader reader)
    {
        DocumentSnapshotHandleData data = default;
        reader.ReadProperties(ref data, DocumentSnapshotHandleData.PropertyMap);

        return new DocumentSnapshotHandle(data.FilePath, data.TargetPath, data.FileKind);
    }

    public static ProjectWorkspaceState ReadProjectWorkspaceStateFromProperties(JsonDataReader reader)
    {
        ProjectWorkspaceStateData data = default;
        reader.ReadProperties(ref data, ProjectWorkspaceStateData.PropertyMap);

        return new ProjectWorkspaceState(data.TagHelpers, data.CSharpLanguageVersion);
    }

    public static TagHelperDescriptor ReadTagHelperFromProperties(JsonDataReader reader, bool useCache)
    {
        // Try reading the optional hashcode
        var hashWasRead = reader.TryReadInt32(RazorSerializationConstants.HashCodePropertyName, out var hash);
        if (useCache && hashWasRead &&
            TagHelperDescriptorCache.TryGetDescriptor(hash, out var descriptor))
        {
            reader.ReadToEndOfCurrentObject();
            return descriptor;
        }

        // Required tokens (order matters)
        if (!reader.TryReadString(nameof(TagHelperDescriptor.Kind), out var descriptorKind))
        {
            return null!;
        }

        if (!reader.TryReadString(nameof(TagHelperDescriptor.Name), out var typeName))
        {
            return null!;
        }

        if (!reader.TryReadString(nameof(TagHelperDescriptor.AssemblyName), out var assemblyName))
        {
            return null!;
        }

        using var _ = TagHelperDescriptorBuilder.GetPooledInstance(
            Cached(descriptorKind), Cached(typeName), Cached(assemblyName),
            out var builder);

        reader.ProcessProperties(new TagHelperReader(builder), TagHelperReader.PropertyMap);

        descriptor = builder.Build();

        if (useCache && hashWasRead)
        {
            TagHelperDescriptorCache.Set(hash, descriptor);
        }

        return descriptor;
    }

    private static void ProcessDiagnostic(JsonDataReader reader, RazorDiagnosticCollection collection)
    {
        DiagnosticData data = default;
        reader.ReadObjectData(ref data, DiagnosticData.PropertyMap);

        var descriptor = new RazorDiagnosticDescriptor(Cached(data.Id), MessageFormat(data.Message), data.Severity);
        var diagnostic = RazorDiagnostic.Create(descriptor, data.Span);

        collection.Add(diagnostic);

        static Func<string> MessageFormat(string message)
        {
            return () => Cached(message);
        }
    }

    private static void ProcessMetadata(JsonDataReader reader, IDictionary<string, string?> dictionary)
    {
        while (reader.TryReadNextPropertyName(out var key))
        {
            var value = reader.ReadString();
            dictionary[key] = value;
        }
    }

    public static ProjectRazorJson ReadProjectRazorJsonFromProperties(JsonDataReader reader)
    {
        ProjectRazorJsonData data = default;
        reader.ReadProperties(ref data, ProjectRazorJsonData.PropertyMap);

        // We need to add a serialization format to the project response to indicate that this version
        // of the code is compatible with what's being serialized. This scenario typically happens when
        // a user has an incompatible serialized project snapshot but is using the latest Razor bits.

        if (string.IsNullOrEmpty(data.SerializationFormat) || data.SerializationFormat != ProjectSerializationFormat.Version)
        {
            // Unknown serialization format.
            return null!;
        }

        return new ProjectRazorJson(
            data.SerializedFilePath, data.FilePath, data.Configuration, data.RootNamespace, data.ProjectWorkspaceState, data.Documents);
    }
}
