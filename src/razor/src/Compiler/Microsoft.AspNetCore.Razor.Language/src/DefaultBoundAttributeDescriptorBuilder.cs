// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Language;

internal partial class DefaultBoundAttributeDescriptorBuilder : BoundAttributeDescriptorBuilder, IBuilder<BoundAttributeDescriptor>
{
    private static readonly ObjectPool<DefaultBoundAttributeDescriptorBuilder> s_pool = DefaultPool.Create(Policy.Instance);

    public static DefaultBoundAttributeDescriptorBuilder GetInstance(DefaultTagHelperDescriptorBuilder parent, string kind)
    {
        var builder = s_pool.Get();

        builder._parent = parent;
        builder._kind = kind;

        return builder;
    }

    public static void ReturnInstance(DefaultBoundAttributeDescriptorBuilder builder)
        => s_pool.Return(builder);

    private static readonly ObjectPool<HashSet<BoundAttributeParameterDescriptor>> s_boundAttributeParameterSetPool
        = HashSetPool<BoundAttributeParameterDescriptor>.Create(BoundAttributeParameterDescriptorComparer.Default);

    // PERF: A Dictionary<string, string> is used intentionally here for faster lookup over ImmutableDictionary<string, string>.
    // This should never be mutated.
    private static readonly Dictionary<string, string> s_primitiveDisplayTypeNameLookups = new(StringComparer.Ordinal)
    {
        { typeof(byte).FullName, "byte" },
        { typeof(sbyte).FullName, "sbyte" },
        { typeof(int).FullName, "int" },
        { typeof(uint).FullName, "uint" },
        { typeof(short).FullName, "short" },
        { typeof(ushort).FullName, "ushort" },
        { typeof(long).FullName, "long" },
        { typeof(ulong).FullName, "ulong" },
        { typeof(float).FullName, "float" },
        { typeof(double).FullName, "double" },
        { typeof(char).FullName, "char" },
        { typeof(bool).FullName, "bool" },
        { typeof(object).FullName, "object" },
        { typeof(string).FullName, "string" },
        { typeof(decimal).FullName, "decimal" }
    };

    [AllowNull]
    private DefaultTagHelperDescriptorBuilder _parent;
    [AllowNull]
    private string _kind;
    private List<DefaultBoundAttributeParameterDescriptorBuilder>? _attributeParameterBuilders;
    private DocumentationObject _documentationObject;
    private MetadataHolder _metadata;
    private RazorDiagnosticCollection? _diagnostics;

    private DefaultBoundAttributeDescriptorBuilder()
    {
    }

    public DefaultBoundAttributeDescriptorBuilder(DefaultTagHelperDescriptorBuilder parent, string kind)
    {
        _parent = parent;
        _kind = kind;
    }

    public override string? Name { get; set; }
    public override string? TypeName { get; set; }
    public override bool IsEnum { get; set; }
    public override bool IsDictionary { get; set; }
    public override string? IndexerAttributeNamePrefix { get; set; }
    public override string? IndexerValueTypeName { get; set; }

    public override string? Documentation
    {
        get => _documentationObject.GetText();
        set => _documentationObject = new(value);
    }

    public override string? DisplayName { get; set; }

    public override IDictionary<string, string?> Metadata => _metadata.MetadataDictionary;

    public override void SetMetadata(MetadataCollection metadata) => _metadata.SetMetadataCollection(metadata);

    public override bool TryGetMetadataValue(string key, [NotNullWhen(true)] out string? value)
        => _metadata.TryGetMetadataValue(key, out value);

    public override RazorDiagnosticCollection Diagnostics => _diagnostics ??= new RazorDiagnosticCollection();

    internal bool CaseSensitive => _parent.CaseSensitive;

    public override void BindAttributeParameter(Action<BoundAttributeParameterDescriptorBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        EnsureAttributeParameterBuilders();

        var builder = DefaultBoundAttributeParameterDescriptorBuilder.GetInstance(this, _kind);
        configure(builder);
        _attributeParameterBuilders.Add(builder);
    }

    internal override void SetDocumentation(string? text)
    {
        _documentationObject = new(text);
    }

    internal override void SetDocumentation(DocumentationDescriptor? documentation)
    {
        _documentationObject = new(documentation);
    }

    public BoundAttributeDescriptor Build()
    {
        var diagnostics = new PooledHashSet<RazorDiagnostic>();
        try
        {
            Validate(ref diagnostics);

            diagnostics.UnionWith(_diagnostics);

            var parameters = _attributeParameterBuilders.BuildAllOrEmpty(s_boundAttributeParameterSetPool);

            var metadata = _metadata.GetMetadataCollection();

            var descriptor = new DefaultBoundAttributeDescriptor(
                _kind,
                Name,
                TypeName,
                IsEnum,
                IsDictionary,
                IndexerAttributeNamePrefix,
                IndexerValueTypeName,
                _documentationObject,
                GetDisplayName(),
                CaseSensitive,
                IsEditorRequired,
                parameters,
                metadata,
                diagnostics.ToArray());

            return descriptor;
        }
        finally
        {
            diagnostics.ClearAndFree();
        }
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

    private void Validate(ref PooledHashSet<RazorDiagnostic> diagnostics)
    {
        // data-* attributes are explicitly not implemented by user agents and are not intended for use on
        // the server; therefore it's invalid for TagHelpers to bind to them.
        const string DataDashPrefix = "data-";
        var isDirectiveAttribute = this.IsDirectiveAttribute();

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

    [MemberNotNull(nameof(_attributeParameterBuilders))]
    private void EnsureAttributeParameterBuilders()
    {
        _attributeParameterBuilders ??= new List<DefaultBoundAttributeParameterDescriptorBuilder>();
    }
}
