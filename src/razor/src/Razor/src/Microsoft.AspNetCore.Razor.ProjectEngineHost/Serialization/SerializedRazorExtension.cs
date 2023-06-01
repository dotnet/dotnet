// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization;

internal class SerializedRazorExtension : RazorExtension
{
    public SerializedRazorExtension(string extensionName)
    {
        if (extensionName is null)
        {
            throw new ArgumentNullException(nameof(extensionName));
        }

        ExtensionName = extensionName;
    }

    public override string ExtensionName { get; }
}
