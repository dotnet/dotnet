// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.Language;

internal class DefaultRazorCodeGenerationOptionsFactoryProjectFeature : RazorProjectEngineFeatureBase, IRazorCodeGenerationOptionsFactoryProjectFeature
{
    private ImmutableArray<IConfigureRazorCodeGenerationOptionsFeature> _configureOptions;

    protected override void OnInitialized()
    {
        _configureOptions = ProjectEngine.Engine.GetFeatures<IConfigureRazorCodeGenerationOptionsFeature>();
    }

    public RazorCodeGenerationOptions Create(Action<RazorCodeGenerationOptionsBuilder> configure)
    {
        var builder = new RazorCodeGenerationOptionsBuilder(ProjectEngine.Configuration);
        configure?.Invoke(builder);

        foreach (var options in _configureOptions)
        {
            options.Configure(builder);
        }

        return builder.Build();
    }
}
