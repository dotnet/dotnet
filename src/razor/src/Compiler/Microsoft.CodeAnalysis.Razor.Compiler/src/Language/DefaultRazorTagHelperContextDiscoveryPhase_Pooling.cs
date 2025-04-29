// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Language;

internal partial class DefaultRazorTagHelperContextDiscoveryPhase
{
    private static readonly ObjectPool<TagHelperDirectiveVisitor> s_tagHelperDirectiveVisitorPool = DefaultPool.Create(DirectiveVisitorPolicy<TagHelperDirectiveVisitor>.Instance);
    private static readonly ObjectPool<ComponentDirectiveVisitor> s_componentDirectiveVisitorPool = DefaultPool.Create(DirectiveVisitorPolicy<ComponentDirectiveVisitor>.Instance);

    private sealed class DirectiveVisitorPolicy<T> : IPooledObjectPolicy<T>
        where T : DirectiveVisitor, new()
    {
        public static readonly DirectiveVisitorPolicy<T> Instance = new();

        private DirectiveVisitorPolicy()
        {
        }

        public T Create() => new();

        public bool Return(T visitor)
        {
            visitor.Reset();

            return true;
        }
    }

    internal readonly ref struct PooledDirectiveVisitor(DirectiveVisitor visitor, bool isComponentDirectiveVisitor)
    {
        public void Dispose()
        {
            if (isComponentDirectiveVisitor)
            {
                s_componentDirectiveVisitorPool.Return((ComponentDirectiveVisitor)visitor);
            }
            else
            {
                s_tagHelperDirectiveVisitorPool.Return((TagHelperDirectiveVisitor)visitor);
            }
        }
    }

    internal static PooledDirectiveVisitor GetPooledVisitor(
        RazorCodeDocument codeDocument,
        IReadOnlyList<TagHelperDescriptor> tagHelpers,
        out DirectiveVisitor visitor)
    {
        var useComponentDirectiveVisitor = codeDocument.ParserOptions.AllowComponentFileKind &&
                                           FileKinds.IsComponent(codeDocument.FileKind);

        if (useComponentDirectiveVisitor)
        {
            var componentDirectiveVisitor = s_componentDirectiveVisitorPool.Get();

            codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var currentNamespace);
            var filePath = codeDocument.Source.FilePath.AssumeNotNull();
            componentDirectiveVisitor.Initialize(filePath, tagHelpers, currentNamespace);

            visitor = componentDirectiveVisitor;
        }
        else
        {
            var tagHelperDirectiveVisitor = s_tagHelperDirectiveVisitorPool.Get();

            tagHelperDirectiveVisitor.Initialize(tagHelpers);

            visitor = tagHelperDirectiveVisitor;
        }

        return new(visitor, useComponentDirectiveVisitor);
    }
}
