// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class BoundAttributeDescriptorWrapper(BoundAttributeDescriptor obj) : Wrapper<BoundAttributeDescriptor>(obj), IRazorBoundAttributeDescriptor
    {
        private ImmutableArray<IRazorBoundAttributeParameterDescriptor> _boundAttributeParameters;

        public string Name => Object.Name;
        public string DisplayName => Object.DisplayName;
        public string? Documentation => Object.Documentation;
        public bool CaseSensitive => Object.CaseSensitive;
        public string? IndexerNamePrefix => Object.IndexerNamePrefix;

        public ImmutableArray<IRazorBoundAttributeParameterDescriptor> BoundAttributeParameters
            => InitializeArrayWithWrappedItems(ref _boundAttributeParameters, Object.Parameters, Wrap);
    }
}
