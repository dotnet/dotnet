// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost;

internal static class UnsupportedRazorConfiguration
{
    public static readonly RazorConfiguration Instance = RazorConfiguration.Create(
        RazorLanguageVersion.Version_1_0,
        "UnsupportedRazor",
        new[] { new UnsupportedRazorExtension("UnsupportedRazorExtension"), });

    private class UnsupportedRazorExtension(string extensionName) : RazorExtension
    {
        public override string ExtensionName { get; } = extensionName ?? throw new ArgumentNullException(nameof(extensionName));
    }
}
