// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
#if !NETCOREAPP
using System.Collections.Generic;
#endif
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Utilities;

internal static class ChecksumExtensions
{
    public static Checksum GetChecksum(this TagHelperDescriptor value)
        => ChecksumCache.GetOrCreate(value, static o => CreateChecksum((TagHelperDescriptor)o));

    // Public for benchmarks
    public static Checksum CreateChecksum(this TagHelperDescriptor value)
    {
        var builder = new Checksum.Builder();

        builder.AppendData(value.Kind);
        builder.AppendData(value.Name);
        builder.AppendData(value.AssemblyName);
        builder.AppendData(value.DisplayName);
        builder.AppendData(value.TagOutputHint);

        AppendDocumentation(value.DocumentationObject, builder);

        builder.AppendData(value.CaseSensitive);

        foreach (var descriptor in (AllowedChildTagDescriptor[])value.AllowedChildTags)
        {
            builder.AppendData(GetChecksum(descriptor));
        }

        foreach (var descriptor in (BoundAttributeDescriptor[])value.BoundAttributes)
        {
            builder.AppendData(GetChecksum(descriptor));
        }

        foreach (var descriptor in (TagMatchingRuleDescriptor[])value.TagMatchingRules)
        {
            builder.AppendData(GetChecksum(descriptor));
        }

        builder.AppendData(GetChecksum((MetadataCollection)value.Metadata));

        foreach (var diagnostic in (RazorDiagnostic[])value.Diagnostics)
        {
            builder.AppendData(GetChecksum(diagnostic));
        }

        return builder.FreeAndGetChecksum();
    }

    public static Checksum GetChecksum(this AllowedChildTagDescriptor value)
    {
        return ChecksumCache.GetOrCreate(value, Create);

        static object Create(object obj)
        {
            var builder = new Checksum.Builder();

            var value = (AllowedChildTagDescriptor)obj;

            builder.AppendData(value.Name);
            builder.AppendData(value.DisplayName);

            foreach (var diagnostic in (RazorDiagnostic[])value.Diagnostics)
            {
                builder.AppendData(GetChecksum(diagnostic));
            }

            return builder.FreeAndGetChecksum();
        }
    }

    public static Checksum GetChecksum(this TagMatchingRuleDescriptor value)
    {
        return ChecksumCache.GetOrCreate(value, Create);

        static object Create(object obj)
        {
            var builder = new Checksum.Builder();

            var value = (TagMatchingRuleDescriptor)obj;

            builder.AppendData(value.TagName);
            builder.AppendData(value.ParentTag);
            builder.AppendData((int)value.TagStructure);

            builder.AppendData(value.CaseSensitive);

            foreach (var descriptor in (RequiredAttributeDescriptor[])value.Attributes)
            {
                builder.AppendData(GetChecksum(descriptor));
            }

            foreach (var diagnostic in (RazorDiagnostic[])value.Diagnostics)
            {
                builder.AppendData(GetChecksum(diagnostic));
            }

            return builder.FreeAndGetChecksum();
        }
    }

    public static Checksum GetChecksum(this RequiredAttributeDescriptor value)
    {
        return ChecksumCache.GetOrCreate(value, Create);

        static object Create(object obj)
        {
            var builder = new Checksum.Builder();

            var value = (RequiredAttributeDescriptor)obj;

            builder.AppendData(value.Name);
            builder.AppendData((int)value.NameComparison);
            builder.AppendData(value.Value);
            builder.AppendData((int)value.ValueComparison);
            builder.AppendData(value.DisplayName);

            builder.AppendData(value.CaseSensitive);

            builder.AppendData(GetChecksum((MetadataCollection)value.Metadata));

            foreach (var diagnostic in (RazorDiagnostic[])value.Diagnostics)
            {
                builder.AppendData(GetChecksum(diagnostic));
            }

            return builder.FreeAndGetChecksum();
        }
    }

    public static Checksum GetChecksum(this BoundAttributeDescriptor value)
    {
        return ChecksumCache.GetOrCreate(value, Create);

        static object Create(object obj)
        {
            var builder = new Checksum.Builder();

            var value = (BoundAttributeDescriptor)obj;

            builder.AppendData(value.Kind);
            builder.AppendData(value.Name);
            builder.AppendData(value.TypeName);
            builder.AppendData(value.IndexerNamePrefix);
            builder.AppendData(value.IndexerTypeName);
            builder.AppendData(value.DisplayName);

            AppendDocumentation(value.DocumentationObject, builder);

            builder.AppendData(value.CaseSensitive);
            builder.AppendData(value.IsEditorRequired);
            builder.AppendData(value.IsEnum);
            builder.AppendData(value.HasIndexer);
            builder.AppendData(value.IsBooleanProperty);
            builder.AppendData(value.IsStringProperty);
            builder.AppendData(value.IsIndexerBooleanProperty);
            builder.AppendData(value.IsIndexerStringProperty);

            foreach (var descriptor in (BoundAttributeParameterDescriptor[])value.BoundAttributeParameters)
            {
                builder.AppendData(GetChecksum(descriptor));
            }

            builder.AppendData(GetChecksum((MetadataCollection)value.Metadata));

            foreach (var diagnostic in (RazorDiagnostic[])value.Diagnostics)
            {
                builder.AppendData(GetChecksum(diagnostic));
            }

            return builder.FreeAndGetChecksum();
        }
    }

    public static Checksum GetChecksum(this BoundAttributeParameterDescriptor value)
    {
        return ChecksumCache.GetOrCreate(value, Create);

        static object Create(object obj)
        {
            var builder = new Checksum.Builder();

            var value = (BoundAttributeParameterDescriptor)obj;

            builder.AppendData(value.Kind);
            builder.AppendData(value.Name);
            builder.AppendData(value.TypeName);
            builder.AppendData(value.DisplayName);

            AppendDocumentation(value.DocumentationObject, builder);

            builder.AppendData(value.CaseSensitive);
            builder.AppendData(value.IsEnum);
            builder.AppendData(value.IsBooleanProperty);
            builder.AppendData(value.IsStringProperty);
            builder.AppendData(GetChecksum((MetadataCollection)value.Metadata));

            foreach (var diagnostic in (RazorDiagnostic[])value.Diagnostics)
            {
                builder.AppendData(GetChecksum(diagnostic));
            }

            return builder.FreeAndGetChecksum();
        }
    }

    private static void AppendDocumentation(DocumentationObject documentationObject, Checksum.Builder builder)
    {
        switch (documentationObject.Object)
        {
            case DocumentationDescriptor descriptor:
                builder.AppendData(GetChecksum(descriptor));
                break;

            case string s:
                builder.AppendData(s);
                break;

            case null:
                builder.AppendNull();
                break;
        }
    }

    public static Checksum GetChecksum(this DocumentationDescriptor value)
    {
        return ChecksumCache.GetOrCreate(value, Create);

        static object Create(object obj)
        {
            var builder = new Checksum.Builder();

            var value = (DocumentationDescriptor)obj;
            builder.AppendData((int)value.Id);

            foreach (var arg in value.Args)
            {
                switch (arg)
                {
                    case string s:
                        builder.AppendData(s);
                        break;

                    case int i:
                        builder.AppendData(i);
                        break;

                    case bool b:
                        builder.AppendData(b);
                        break;

                    case null:
                        builder.AppendNull();
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }

            return builder.FreeAndGetChecksum();
        }
    }

    public static Checksum GetChecksum(this RazorDiagnostic value)
    {
        return ChecksumCache.GetOrCreate(value, Create);

        static object Create(object obj)
        {
            var builder = new Checksum.Builder();

            var diagnostic = (RazorDiagnostic)obj;

            builder.AppendData(diagnostic.Id);
            builder.AppendData((int)diagnostic.Severity);
            builder.AppendData(diagnostic.GetMessage());

            var span = diagnostic.Span;
            builder.AppendData(span.FilePath);
            builder.AppendData(span.AbsoluteIndex);
            builder.AppendData(span.LineIndex);
            builder.AppendData(span.CharacterIndex);
            builder.AppendData(span.Length);
            builder.AppendData(span.LineCount);
            builder.AppendData(span.EndCharacterIndex);

            return builder.FreeAndGetChecksum();
        }
    }

    public static Checksum GetChecksum(this MetadataCollection value)
    {
        return ChecksumCache.GetOrCreate(value, Create);

        static object Create(object obj)
        {
            var builder = new Checksum.Builder();

            foreach (var (key, value) in (MetadataCollection)obj)
            {
                builder.AppendData(key);
                builder.AppendData(value);
            }

            return builder.FreeAndGetChecksum();
        }
    }
}
