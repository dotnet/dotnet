// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ProjectSystem;

internal static class UnsupportedRazorConfiguration
{
    public static readonly RazorConfiguration Instance = new(
        RazorLanguageVersion.Version_1_0,
        "UnsupportedRazor",
        [new("UnsupportedRazorExtension")]);
}
