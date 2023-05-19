// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.AspNetCore.Razor.Language;

/// <summary>
///  Helper struct that wraps a <see cref="DocumentationDescriptor"/>, <see cref="string"/>, or <see langword="null"/>.
/// </summary>
internal readonly record struct DocumentationObject
{
    public readonly object? Object;

    public DocumentationObject(object? obj)
    {
        if (obj is not (DocumentationDescriptor or string or null))
        {
            throw new ArgumentException(
                Resources.FormatA_documentation_object_can_only_be_a_0_instance_string_or_null(nameof(DocumentationDescriptor)),
                paramName: nameof(obj));
        }

        Object = obj;
    }

    public readonly string? GetText()
        => Object switch
        {
            DocumentationDescriptor d => d.GetText(),
            string s => s,
            null => null,
            _ => Assumed.Unreachable<string>()
        };

    public override int GetHashCode()
        => Object switch
        {
            DocumentationDescriptor d => d.GetHashCode(),
            string s => s.GetHashCode(),
            null => 0,
            _ => Assumed.Unreachable<int>()
        };

    public bool Equals(DocumentationObject other)
        => (Object, other.Object) switch
        {
            (DocumentationDescriptor d1, DocumentationDescriptor d2) => d1.Equals(d2),
            (string s1, string s2) => s1 == s2,
            (null, null) => true,
            (DocumentationDescriptor or string or null, DocumentationDescriptor or string or null) => false,
            _ => Assumed.Unreachable<bool>()
        };

    public static implicit operator DocumentationObject(string text)
        => new(text);

    public static implicit operator DocumentationObject(DocumentationDescriptor descriptor)
        => new(descriptor);
}
