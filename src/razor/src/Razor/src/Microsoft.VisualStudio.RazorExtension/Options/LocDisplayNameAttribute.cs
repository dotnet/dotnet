// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
