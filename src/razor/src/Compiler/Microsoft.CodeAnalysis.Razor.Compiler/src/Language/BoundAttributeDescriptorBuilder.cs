// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed partial class BoundAttributeDescriptorBuilder : TagHelperObjectBuilder<BoundAttributeDescriptor>
{
    // PERF: A Dictionary<string, string> is used intentionally here for faster lookup over ImmutableDictionary<string, string>.
    // This should never be mutated.
    private static readonly Dictionary<string, string> s_primitiveDisplayTypeNameLookups = new(StringComparer.Ordinal)
    {
        { typeof(byte).FullName!, "byte" },
        { typeof(sbyte).FullName!, "sbyte" },
        { typeof(int).FullName!, "int" },
        { typeof(uint).FullName!, "uint" },
        { typeof(short).FullName!, "short" },
        { typeof(ushort).FullName!, "ushort" },
        { typeof(long).FullName!, "long" },
        { typeof(ulong).FullName!, "ulong" },
        { typeof(float).FullName!, "float" },
        { typeof(double).FullName!, "double" },
        { typeof(char).FullName!, "char" },
        { typeof(bool).FullName!, "bool" },
        { typeof(object).FullName!, "object" },
        { typeof(string).FullName!, "string" },
        { typeof(decimal).FullName!, "decimal" }
    };

    [AllowNull]
    private TagHelperDescriptorBuilder _parent;
    [AllowNull]
    private string _kind;
    private DocumentationObject _documentationObject;
    private MetadataHolder _metadata;
    private bool? _caseSensitive;

    private BoundAttributeDescriptorBuilder()
    {
    }

    internal BoundAttributeDescriptorBuilder(TagHelperDescriptorBuilder parent, string kind)
    {
        _parent = parent;
        _kind = kind;
    }

    [AllowNull]
    public string Name { get; set; }
    public string? TypeName { get; set; }
    public bool IsEnum { get; set; }
    public bool IsDictionary { get; set; }
    public string? IndexerAttributeNamePrefix { get; set; }
    public string? IndexerValueTypeName { get; set; }
    internal bool IsEditorRequired { get; set; }

    public string? Documentation
    {
        get => _documentationObject.GetText();
        set => _documentationObject = new(value);
    }

    public string? DisplayName { get; set; }

    public string? ContainingType { get; set; }

    public IDictionary<string, string?> Metadata => _metadata.MetadataDictionary;

    public void SetMetadata(MetadataCollection metadata) => _metadata.SetMetadataCollection(metadata);

    public bool TryGetMetadataValue(string key, [NotNullWhen(true)] out string? value)
        => _metadata.TryGetMetadataValue(key, out value);

    internal bool CaseSensitive
    {
        get => _caseSensitive ?? _parent.CaseSensitive;
        set => _caseSensitive = value;
    }

    private TagHelperObjectBuilderCollection<BoundAttributeParameterDescriptor, BoundAttributeParameterDescriptorBuilder> Parameters { get; }
        = new(BoundAttributeParameterDescriptorBuilder.Pool);

    public void BindAttributeParameter(Action<BoundAttributeParameterDescriptorBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var builder = BoundAttributeParameterDescriptorBuilder.GetInstance(this, _kind);
        configure(builder);
        Parameters.Add(builder);
    }

    internal void SetDocumentation(string? text)
    {
        _documentationObject = new(text);
    }

    internal void SetDocumentation(DocumentationDescriptor? documentation)
    {
        _documentationObject = new(documentation);
    }

    private protected override BoundAttributeDescriptor BuildCore(ImmutableArray<RazorDiagnostic> diagnostics)
    {
        return new BoundAttributeDescriptor(
            _kind,
            Name ?? string.Empty,
            TypeName ?? string.Empty,
            IsEnum,
            IsDictionary,
            IndexerAttributeNamePrefix,
            IndexerValueTypeName,
            _documentationObject,
            GetDisplayName(),
            ContainingType,
            CaseSensitive,
            IsEditorRequired,
            Parameters.ToImmutable(),
            _metadata.GetMetadataCollection(),
            diagnostics);
    }

    private string GetDisplayName()
    {
        if (DisplayName != null)
        {
            return DisplayName;
        }

        if (!_parent.TryGetMetadataValue(TagHelperMetadata.Common.TypeName, out var parentTypeName))
        {
            parentTypeName = null;
        }

        if (!TryGetMetadataValue(TagHelperMetadata.Common.PropertyName, out var propertyName))
        {
            propertyName = null;
        }

        if (TypeName != null &&
            propertyName != null &&
            parentTypeName != null)
        {
            // This looks like a normal c# property, so lets compute a display name based on that.
            if (!s_primitiveDisplayTypeNameLookups.TryGetValue(TypeName, out var simpleTypeName))
            {
                simpleTypeName = TypeName;
            }

            return $"{simpleTypeName} {parentTypeName}.{propertyName}";
        }

        return Name ?? string.Empty;
    }

    private bool IsDirectiveAttribute()
        => TryGetMetadataValue(ComponentMetadata.Common.DirectiveAttribute, out var value) &&
           value == bool.TrueString;

    private protected override void CollectDiagnostics(ref PooledHashSet<RazorDiagnostic> diagnostics)
    {
        // data-* attributes are explicitly not implemented by user agents and are not intended for use on
        // the server; therefore it's invalid for TagHelpers to bind to them.
        const string DataDashPrefix = "data-";
        var isDirectiveAttribute = IsDirectiveAttribute();

        if (Name.IsNullOrWhiteSpace())
        {
            if (IndexerAttributeNamePrefix == null)
            {
                var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidBoundAttributeNullOrWhitespace(
                    _parent.GetDisplayName(),
                    GetDisplayName());

                diagnostics.Add(diagnostic);
            }
        }
        else
        {
            if (Name.StartsWith(DataDashPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidBoundAttributeNameStartsWith(
                    _parent.GetDisplayName(),
                    GetDisplayName(),
                    Name);

                diagnostics.Add(diagnostic);
            }

            var name = Name.AsSpan();
            if (isDirectiveAttribute && name[0] == '@')
            {
                name = name[1..];
            }
            else if (isDirectiveAttribute)
            {
                var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidBoundDirectiveAttributeName(
                    _parent.GetDisplayName(),
                    GetDisplayName(),
                    Name);

                diagnostics.Add(diagnostic);
            }

            foreach (var ch in name)
            {
                if (char.IsWhiteSpace(ch) || HtmlConventions.IsInvalidNonWhitespaceHtmlCharacters(ch))
                {
                    var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidBoundAttributeName(
                        _parent.GetDisplayName(),
                        GetDisplayName(),
                        name.ToString(),
                        ch);

                    diagnostics.Add(diagnostic);
                }
            }
        }

        if (IndexerAttributeNamePrefix != null)
        {
            if (IndexerAttributeNamePrefix.StartsWith(DataDashPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidBoundAttributePrefixStartsWith(
                    _parent.GetDisplayName(),
                    GetDisplayName(),
                    IndexerAttributeNamePrefix);

                diagnostics.Add(diagnostic);
            }
            else if (IndexerAttributeNamePrefix.Length > 0 && string.IsNullOrWhiteSpace(IndexerAttributeNamePrefix))
            {
                var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidBoundAttributeNullOrWhitespace(
                    _parent.GetDisplayName(),
                    GetDisplayName());

                diagnostics.Add(diagnostic);
            }
            else
            {
                var indexerPrefix = IndexerAttributeNamePrefix.AsSpan();
                if (isDirectiveAttribute && indexerPrefix[0] == '@')
                {
                    indexerPrefix = indexerPrefix[1..];
                }
                else if (isDirectiveAttribute)
                {
                    var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidBoundDirectiveAttributePrefix(
                        _parent.GetDisplayName(),
                        GetDisplayName(),
                        indexerPrefix.ToString());

                    diagnostics.Add(diagnostic);
                }

                foreach (var ch in indexerPrefix)
                {
                    if (char.IsWhiteSpace(ch) || HtmlConventions.IsInvalidNonWhitespaceHtmlCharacters(ch))
                    {
                        var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidBoundAttributePrefix(
                            _parent.GetDisplayName(),
                            GetDisplayName(),
                            indexerPrefix.ToString(),
                            ch);

                        diagnostics.Add(diagnostic);
                    }
                }
            }
        }
    }
}
