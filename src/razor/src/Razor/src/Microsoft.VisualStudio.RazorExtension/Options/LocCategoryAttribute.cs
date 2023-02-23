// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
