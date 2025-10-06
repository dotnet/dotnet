// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators
{
    internal sealed class StaticCompilationTagHelperFeature(Compilation compilation)
        : RazorEngineFeatureBase, ITagHelperFeature
    {
        private ImmutableArray<ITagHelperDescriptorProvider> _providers;

        public void CollectDescriptors(
            IAssemblySymbol? targetAssembly,
            List<TagHelperDescriptor> results,
            CancellationToken cancellationToken)
        {
            if (_providers.IsDefaultOrEmpty)
            {
                return;
            }

            var context = new TagHelperDescriptorProviderContext(compilation, targetAssembly, results);

            foreach (var provider in _providers)
            {
                provider.Execute(context, cancellationToken);
            }
        }

        IReadOnlyList<TagHelperDescriptor> ITagHelperFeature.GetDescriptors(CancellationToken cancellationToken)
        {
            var results = new List<TagHelperDescriptor>();
            CollectDescriptors(targetAssembly: null, results, cancellationToken);

            return results;
        }

        protected override void OnInitialized()
        {
            _providers = Engine.GetFeatures<ITagHelperDescriptorProvider>().OrderByAsArray(static x => x.Order);
        }
    }
}
