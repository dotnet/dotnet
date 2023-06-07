// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Language;

internal partial class DefaultBoundAttributeParameterDescriptorBuilder : BoundAttributeParameterDescriptorBuilder, IBuilder<BoundAttributeParameterDescriptor>
{
    private static readonly ObjectPool<DefaultBoundAttributeParameterDescriptorBuilder> s_pool = DefaultPool.Create(Policy.Instance);

    public static DefaultBoundAttributeParameterDescriptorBuilder GetInstance(DefaultBoundAttributeDescriptorBuilder parent, string kind)
    {
        var builder = s_pool.Get();

        builder._parent = parent;
        builder._kind = kind;

        return builder;
    }

    public static void ReturnInstance(DefaultBoundAttributeParameterDescriptorBuilder builder)
        => s_pool.Return(builder);

    [AllowNull]
    private DefaultBoundAttributeDescriptorBuilder _parent;
    [AllowNull]
    private string _kind;
    private DocumentationObject _documentationObject;
    private MetadataHolder _metadata;

    private RazorDiagnosticCollection? _diagnostics;

    private DefaultBoundAttributeParameterDescriptorBuilder()
    {
    }

    public DefaultBoundAttributeParameterDescriptorBuilder(DefaultBoundAttributeDescriptorBuilder parent, string kind)
    {
        _parent = parent;
        _kind = kind;
    }

    public override string? Name { get; set; }
    public override string? TypeName { get; set; }
    public override bool IsEnum { get; set; }

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

    internal override void SetDocumentation(string? text)
    {
        _documentationObject = new(text);
    }

    internal override void SetDocumentation(DocumentationDescriptor? documentation)
    {
        _documentationObject = new(documentation);
    }

    public BoundAttributeParameterDescriptor Build()
    {
        var diagnostics = new PooledHashSet<RazorDiagnostic>();
        try
        {
            Validate(ref diagnostics);

            diagnostics.UnionWith(_diagnostics);

            var metadata = _metadata.GetMetadataCollection();

            var descriptor = new DefaultBoundAttributeParameterDescriptor(
                _kind,
                Name,
                TypeName,
                IsEnum,
                _documentationObject,
                GetDisplayName(),
                CaseSensitive,
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
        => DisplayName ?? $":{Name}";

    private void Validate(ref PooledHashSet<RazorDiagnostic> diagnostics)
    {
        if (Name.IsNullOrWhiteSpace())
        {
            var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidBoundAttributeParameterNullOrWhitespace(_parent.Name);

            diagnostics.Add(diagnostic);
        }
        else
        {
            foreach (var character in Name)
            {
                if (char.IsWhiteSpace(character) || HtmlConventions.IsInvalidNonWhitespaceHtmlCharacters(character))
                {
                    var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidBoundAttributeParameterName(
                        _parent.Name,
                        Name,
                        character);

                    diagnostics.Add(diagnostic);
                }
            }
        }
    }
}
