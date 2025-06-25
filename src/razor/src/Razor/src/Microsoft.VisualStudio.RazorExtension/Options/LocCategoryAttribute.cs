// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;

namespace Microsoft.VisualStudio.RazorExtension.Options;

[AttributeUsage(AttributeTargets.All)]
internal class LocCategoryAttribute : CategoryAttribute
{
    public LocCategoryAttribute(string category)
        : base(category)
    {
    }

    protected override string GetLocalizedString(string value)
    {
        return VSPackage.GetResourceString(Category);
    }
}
