// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Language;

public abstract partial class TagHelperDescriptorBuilder
{
    public static TagHelperDescriptorBuilder Create(string name, string assemblyName)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (assemblyName == null)
        {
            throw new ArgumentNullException(nameof(assemblyName));
        }

        return new DefaultTagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, name, assemblyName);
    }

    public static TagHelperDescriptorBuilder Create(string kind, string name, string assemblyName)
    {
        if (kind == null)
        {
            throw new ArgumentNullException(nameof(kind));
        }

        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (assemblyName == null)
        {
            throw new ArgumentNullException(nameof(assemblyName));
        }

        return new DefaultTagHelperDescriptorBuilder(kind, name, assemblyName);
    }

    /// <summary>
    ///  Retrieves a pooled <see cref="TagHelperDescriptorBuilder"/> instance.
    /// </summary>
    /// <remarks>
    ///  The <see cref="PooledBuilder"/> returned by this method should be disposed
    ///  to return the <see cref="TagHelperDescriptorBuilder"/> to its pool.
    ///  The correct way to achieve this is with a using statement:
    ///
    /// <code>
    ///  using var _ = TagHelperDescriptorBuilder.GetPooledInstance(..., out var builder);
    /// </code>
    /// 
    ///  Once disposed, the builder can no longer be used.
    /// </remarks>
    public static PooledBuilder GetPooledInstance(
        string kind, string name, string assemblyName,
        out TagHelperDescriptorBuilder builder)
    {
        var defaultBuilder = DefaultTagHelperDescriptorBuilder.GetInstance(kind, name, assemblyName);
        builder = defaultBuilder;
        return new(defaultBuilder);
    }

    /// <summary>
    ///  Retrieves a pooled <see cref="TagHelperDescriptorBuilder"/> instance.
    /// </summary>
    /// <remarks>
    ///  The <see cref="PooledBuilder"/> returned by this method should be disposed
    ///  to return the <see cref="TagHelperDescriptorBuilder"/> to its pool.
    ///  The correct way to achieve this is with a using statement:
    ///
    /// <code>
    ///  using var _ = TagHelperDescriptorBuilder.GetPooledInstance(..., out var builder);
    /// </code>
    /// 
    ///  Once disposed, the builder can no longer be used.
    /// </remarks>
    public static PooledBuilder GetPooledInstance(
        string name, string assemblyName,
        out TagHelperDescriptorBuilder builder)
    {
        var defaultBuilder = DefaultTagHelperDescriptorBuilder.GetInstance(name, assemblyName);
        builder = defaultBuilder;
        return new(defaultBuilder);
    }

    public abstract string Name { get; }

    public abstract string AssemblyName { get; }

    public abstract string Kind { get; }

    public abstract string DisplayName { get; set; }

    public abstract string TagOutputHint { get; set; }

    public virtual bool CaseSensitive { get; set; }

    public abstract string Documentation { get; set; }

    public abstract IDictionary<string, string> Metadata { get; }

    public abstract RazorDiagnosticCollection Diagnostics { get; }

    public abstract IReadOnlyList<AllowedChildTagDescriptorBuilder> AllowedChildTags { get; }

    public abstract IReadOnlyList<BoundAttributeDescriptorBuilder> BoundAttributes { get; }

    public abstract IReadOnlyList<TagMatchingRuleDescriptorBuilder> TagMatchingRules { get; }

    public abstract void AllowChildTag(Action<AllowedChildTagDescriptorBuilder> configure);

    public abstract void BindAttribute(Action<BoundAttributeDescriptorBuilder> configure);

    public abstract void TagMatchingRule(Action<TagMatchingRuleDescriptorBuilder> configure);

    public abstract TagHelperDescriptor Build();

    public abstract void Reset();

#nullable enable

    internal virtual void SetDocumentation(string? text)
    {
        throw new NotImplementedException();
    }

    internal virtual void SetDocumentation(DocumentationDescriptor? documentation)
    {
        throw new NotImplementedException();
    }

    public virtual void SetMetadata(MetadataCollection metadata)
    {
        throw new NotImplementedException();
    }

    public virtual bool TryGetMetadataValue(string key, [NotNullWhen(true)] out string? value)
    {
        throw new NotImplementedException();
    }

    internal virtual MetadataBuilder GetMetadataBuilder(string? runtimeName = null)
    {
        throw new NotImplementedException();
    }
}
