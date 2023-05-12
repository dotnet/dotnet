// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class ProjectSystemRazorConfiguration : RazorConfiguration
{
    public override string ConfigurationName { get; }
    public override IReadOnlyList<RazorExtension> Extensions { get; }
    public override RazorLanguageVersion LanguageVersion { get; }
    public override bool UseConsolidatedMvcViews { get; }

    public ProjectSystemRazorConfiguration(
        RazorLanguageVersion languageVersion,
        string configurationName,
        RazorExtension[] extensions,
        bool useConsolidatedMvcViews = false)
    {
        LanguageVersion = languageVersion ?? throw new ArgumentNullException(nameof(languageVersion));
        ConfigurationName = configurationName ?? throw new ArgumentNullException(nameof(configurationName));
        Extensions = extensions ?? throw new ArgumentNullException(nameof(extensions));
        UseConsolidatedMvcViews = useConsolidatedMvcViews;
    }
}
