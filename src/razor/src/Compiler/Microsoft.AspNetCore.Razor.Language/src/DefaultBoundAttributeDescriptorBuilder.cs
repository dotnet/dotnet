// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Language;

internal class DefaultBoundAttributeDescriptorBuilder : BoundAttributeDescriptorBuilder, IBuilder<BoundAttributeDescriptor>
{
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

    private readonly DefaultTagHelperDescriptorBuilder _parent;
    private readonly string _kind;
    private readonly ImmutableDictionary<string, string>.Builder _metadata;
    private List<DefaultBoundAttributeParameterDescriptorBuilder>? _attributeParameterBuilders;

    private RazorDiagnosticCollection? _diagnostics;

    public DefaultBoundAttributeDescriptorBuilder(DefaultTagHelperDescriptorBuilder parent, string kind)
    {
        _parent = parent;
        _kind = kind;

        _metadata = ImmutableDictionary.CreateBuilder<string, string>();
    }

    public override string? Name { get; set; }

    public override string? TypeName { get; set; }

    public override bool IsEnum { get; set; }

    public override bool IsDictionary { get; set; }

    public override string? IndexerAttributeNamePrefix { get; set; }

    public override string? IndexerValueTypeName { get; set; }

    public override string? Documentation { get; set; }

    public override string? DisplayName { get; set; }

    public override IDictionary<string, string> Metadata => _metadata;

    public override RazorDiagnosticCollection Diagnostics => _diagnostics ??= new RazorDiagnosticCollection();

    internal bool CaseSensitive => _parent.CaseSensitive;

    public override void BindAttributeParameter(Action<BoundAttributeParameterDescriptorBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        EnsureAttributeParameterBuilders();

        var builder = new DefaultBoundAttributeParameterDescriptorBuilder(this, _kind);
        configure(builder);
        _attributeParameterBuilders.Add(builder);
    }

    public BoundAttributeDescriptor Build()
    {
        var diagnostics = new PooledHashSet<RazorDiagnostic>();
        try
        {
            Validate(ref diagnostics);

            diagnostics.UnionWith(_diagnostics);

            var parameters = _attributeParameterBuilders.BuildAllOrEmpty(s_boundAttributeParameterSetPool);

            var descriptor = new DefaultBoundAttributeDescriptor(
                _kind,
                Name,
                TypeName,
                IsEnum,
                IsDictionary,
                IndexerAttributeNamePrefix,
                IndexerValueTypeName,
                Documentation,
                GetDisplayName(),
                CaseSensitive,
                parameters,
                _metadata.ToImmutable(),
                diagnostics.ToArray())
            {
                IsEditorRequired = IsEditorRequired,
            };

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

        var parentTypeName = _parent.GetTypeName();
        var propertyName = this.GetPropertyName();

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

            StringSegment name = Name;
            if (isDirectiveAttribute && name.StartsWith("@", StringComparison.Ordinal))
            {
                name = name.Subsegment(1);
            }
            else if (isDirectiveAttribute)
            {
                var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidBoundDirectiveAttributeName(
                        _parent.GetDisplayName(),
                        GetDisplayName(),
                        Name);

                diagnostics.Add(diagnostic);
            }

            for (var i = 0; i < name.Length; i++)
            {
                var character = name[i];
                if (char.IsWhiteSpace(character) || HtmlConventions.IsInvalidNonWhitespaceHtmlCharacters(character))
                {
                    var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidBoundAttributeName(
                        _parent.GetDisplayName(),
                        GetDisplayName(),
                        name.Value,
                        character);

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
                StringSegment indexerPrefix = IndexerAttributeNamePrefix;
                if (isDirectiveAttribute && indexerPrefix.StartsWith("@", StringComparison.Ordinal))
                {
                    indexerPrefix = indexerPrefix.Subsegment(1);
                }
                else if (isDirectiveAttribute)
                {
                    var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidBoundDirectiveAttributePrefix(
                        _parent.GetDisplayName(),
                        GetDisplayName(),
                        indexerPrefix.Value);

                    diagnostics.Add(diagnostic);
                }

                for (var i = 0; i < indexerPrefix.Length; i++)
                {
                    var character = indexerPrefix[i];
                    if (char.IsWhiteSpace(character) || HtmlConventions.IsInvalidNonWhitespaceHtmlCharacters(character))
                    {
                        var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidBoundAttributePrefix(
                            _parent.GetDisplayName(),
                            GetDisplayName(),
                            indexerPrefix.Value,
                            character);

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
