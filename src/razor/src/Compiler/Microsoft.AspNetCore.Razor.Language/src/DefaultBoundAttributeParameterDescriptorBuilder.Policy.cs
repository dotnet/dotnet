// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.Language;

internal partial class DefaultBoundAttributeParameterDescriptorBuilder
{
    private sealed class Policy : TagHelperPooledObjectPolicy<DefaultBoundAttributeParameterDescriptorBuilder>
    {
        public static readonly Policy Instance = new();

        public override DefaultBoundAttributeParameterDescriptorBuilder Create() => new();

        public override bool Return(DefaultBoundAttributeParameterDescriptorBuilder builder)
        {
            builder._parent = null;
            builder._kind = null;
            builder._documentationObject = default;

            builder.Name = null;
            builder.TypeName = null;
            builder.IsEnum = false;
            builder.DisplayName = null;

            ClearDiagnostics(builder._diagnostics);

            builder._metadata.Clear();

            return true;
        }
    }
}
