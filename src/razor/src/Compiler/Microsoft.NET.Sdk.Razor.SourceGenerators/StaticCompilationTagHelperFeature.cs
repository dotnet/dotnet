// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators
{
    internal sealed class StaticCompilationTagHelperFeature(Compilation compilation)
        : RazorEngineFeatureBase, ITagHelperFeature
    {
        private ITagHelperDescriptorProvider[]? _providers;

        public List<TagHelperDescriptor> GetDescriptors(ISymbol? targetSymbol)
        {
            var results = new List<TagHelperDescriptor>();
            var context = TagHelperDescriptorProviderContext.Create(results);
            context.SetCompilation(compilation);
            if (targetSymbol is not null)
            {
                context.Items.SetTargetSymbol(targetSymbol);
            }

            for (var i = 0; i < _providers?.Length; i++)
            {
                _providers[i].Execute(context);
            }

            return results;
        }

        IReadOnlyList<TagHelperDescriptor> ITagHelperFeature.GetDescriptors() => GetDescriptors(targetSymbol: null);

        protected override void OnInitialized()
        {
            _providers = Engine.Features.OfType<ITagHelperDescriptorProvider>().OrderBy(f => f.Order).ToArray();
        }
    }
}
