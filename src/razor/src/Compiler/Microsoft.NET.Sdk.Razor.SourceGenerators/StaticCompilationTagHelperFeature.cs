// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators
{
    internal sealed class StaticCompilationTagHelperFeature : RazorEngineFeatureBase, ITagHelperFeature
    {
        private ITagHelperDescriptorProvider[]? _providers;

        public ImmutableArray<TagHelperDescriptor> GetDescriptors()
        {
            if (Compilation is null)
            {
                return ImmutableArray<TagHelperDescriptor>.Empty;
            }

            using var pool = ArrayBuilderPool<TagHelperDescriptor>.GetPooledObject(out var results);
            var context = TagHelperDescriptorProviderContext.Create(results);
            context.SetCompilation(Compilation);
            if (TargetSymbol is not null)
            {
                context.Items.SetTargetSymbol(TargetSymbol);
            }

            for (var i = 0; i < _providers?.Length; i++)
            {
                _providers[i].Execute(context);
            }

            return results.ToImmutable();
        }

        IReadOnlyList<TagHelperDescriptor> ITagHelperFeature.GetDescriptors() => GetDescriptors();

        public Compilation? Compilation { get; set; }

        public ISymbol? TargetSymbol { get; set; }

        protected override void OnInitialized()
        {
            _providers = Engine.Features.OfType<ITagHelperDescriptorProvider>().OrderBy(f => f.Order).ToArray();
        }
    }
}
