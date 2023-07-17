// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
