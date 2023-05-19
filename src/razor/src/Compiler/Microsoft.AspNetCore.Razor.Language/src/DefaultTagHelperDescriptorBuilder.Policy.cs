// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.Language;

internal partial class DefaultTagHelperDescriptorBuilder
{
    private sealed class Policy : TagHelperPooledObjectPolicy<DefaultTagHelperDescriptorBuilder>
    {
        public static readonly Policy Instance = new();

        public override DefaultTagHelperDescriptorBuilder Create() => new();

        public override bool Return(DefaultTagHelperDescriptorBuilder builder)
        {
            builder._kind = null;
            builder._name = null;
            builder._assemblyName = null;
            builder._documentationObject = default;

            builder.DisplayName = null;
            builder.TagOutputHint = null;
            builder.CaseSensitive = false;

            if (builder._allowedChildTags is { } allowedChildTagBuilders)
            {
                // Make sure that we return all allowed child tag builders to their pool.
                foreach (var allowedChildTagBuilder in allowedChildTagBuilders)
                {
                    DefaultAllowedChildTagDescriptorBuilder.ReturnInstance(allowedChildTagBuilder);
                }

                ClearList(allowedChildTagBuilders);
            }

            if (builder._attributeBuilders is { } attributeBuilders)
            {
                // Make sure that we return all allowed bound attribute builders to their pool.
                foreach (var attributeBuilder in attributeBuilders)
                {
                    DefaultBoundAttributeDescriptorBuilder.ReturnInstance(attributeBuilder);
                }

                ClearList(attributeBuilders);
            }

            if (builder._tagMatchingRuleBuilders is { } tagMatchingRuleBuilders)
            {
                // Make sure that we return all allowed tag matching rule builders to their pool.
                foreach (var tagMatchingRuleBuilder in tagMatchingRuleBuilders)
                {
                    DefaultTagMatchingRuleDescriptorBuilder.ReturnInstance(tagMatchingRuleBuilder);
                }

                ClearList(tagMatchingRuleBuilders);
            }

            ClearDiagnostics(builder._diagnostics);

            builder._metadata.Clear();

            return true;
        }
    }
}
