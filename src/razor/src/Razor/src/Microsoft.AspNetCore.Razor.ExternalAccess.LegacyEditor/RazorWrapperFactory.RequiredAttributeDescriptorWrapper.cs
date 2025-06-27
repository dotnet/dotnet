// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class RequiredAttributeDescriptorWrapper(RequiredAttributeDescriptor obj) : Wrapper<RequiredAttributeDescriptor>(obj), IRazorRequiredAttributeDescriptor
    {
        public string Name => Object.Name;
        public string DisplayName => Object.DisplayName;
    }
}
