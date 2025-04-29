// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel;

namespace Microsoft.VisualStudio.RazorExtension.Options;
internal class LocDisplayNameAttribute : DisplayNameAttribute
{
    private string _resourceKey;

    public LocDisplayNameAttribute(string resourceKey) : base(resourceKey)
    {
        _resourceKey = resourceKey;
    }

    public override string DisplayName => VSPackage.GetResourceString(_resourceKey);
}
