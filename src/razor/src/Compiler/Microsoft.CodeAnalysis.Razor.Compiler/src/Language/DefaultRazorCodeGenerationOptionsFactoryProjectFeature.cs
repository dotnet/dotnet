// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Linq;

namespace Microsoft.AspNetCore.Razor.Language;

internal class DefaultRazorCodeGenerationOptionsFactoryProjectFeature : RazorProjectEngineFeatureBase, IRazorCodeGenerationOptionsFactoryProjectFeature
{
    private IConfigureRazorCodeGenerationOptionsFeature[] _configureOptions;

    protected override void OnInitialized()
    {
        _configureOptions = ProjectEngine.EngineFeatures.OfType<IConfigureRazorCodeGenerationOptionsFeature>().ToArray();
    }

    public RazorCodeGenerationOptions Create(Action<RazorCodeGenerationOptionsBuilder> configure)
    {
        var builder = new RazorCodeGenerationOptionsBuilder(ProjectEngine.Configuration);
        configure?.Invoke(builder);

        for (var i = 0; i < _configureOptions.Length; i++)
        {
            _configureOptions[i].Configure(builder);
        }

        var options = builder.Build();
        return options;
    }
}
