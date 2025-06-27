// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;

namespace Microsoft.VisualStudio.RazorExtension.Options;

internal class LocDescriptionAttribute : DescriptionAttribute
{
    private readonly string _resourceKey;

    public LocDescriptionAttribute(string resourceKey) : base(resourceKey)
    {
        _resourceKey = resourceKey;
    }

    public override string Description => VSPackage.GetResourceString(_resourceKey);
}
