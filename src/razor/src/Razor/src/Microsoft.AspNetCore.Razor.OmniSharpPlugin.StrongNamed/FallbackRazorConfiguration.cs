// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

public class FallbackRazorConfiguration
{
    public static RazorConfiguration SelectConfiguration(Version version) => CodeAnalysis.Razor.ProjectSystem.FallbackRazorConfiguration.SelectConfiguration(version);
}
