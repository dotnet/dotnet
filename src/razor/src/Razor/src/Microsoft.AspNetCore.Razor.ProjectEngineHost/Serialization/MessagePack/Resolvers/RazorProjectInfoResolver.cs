// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;
using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Resolvers;

internal sealed class RazorProjectInfoResolver : IFormatterResolver
{
    public static readonly RazorProjectInfoResolver Instance = new();

    private RazorProjectInfoResolver()
    {
    }

    public IMessagePackFormatter<T>? GetFormatter<T>()
    {
        return Cache<T>.Formatter;
    }

    private static class Cache<T>
    {
        public static readonly IMessagePackFormatter<T>? Formatter;

        static Cache()
        {
            Formatter = (IMessagePackFormatter<T>?)TypeToFormatterMap.GetFormatter(typeof(T));
        }
    }

    private static class TypeToFormatterMap
    {
        private static readonly Dictionary<Type, object> s_map = new()
        {
            RazorProjectInfoFormatter.Instance,

            ChecksumFormatter.Instance,
            DocumentSnapshotHandleFormatter.Instance,
            ProjectWorkspaceStateFormatter.Instance,
            RazorConfigurationFormatter.Instance,

            // tag helpers
            AllowedChildTagFormatter.Instance,
            BoundAttributeFormatter.Instance,
            BoundAttributeParameterFormatter.Instance,
            DocumentationObjectFormatter.Instance,
            MetadataCollectionFormatter.Instance,
            RazorDiagnosticFormatter.Instance,
            RequiredAttributeFormatter.Instance,
            TagHelperFormatter.Instance,
            TagMatchingRuleFormatter.Instance,
        };

        public static object? GetFormatter(Type t)
        {
            if (s_map.TryGetValue(t, out var formatter))
            {
                return formatter;
            }

            return null;
        }
    }
}
