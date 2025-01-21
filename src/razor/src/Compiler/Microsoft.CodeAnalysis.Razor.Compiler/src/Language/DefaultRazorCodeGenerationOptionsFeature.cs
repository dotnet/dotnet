// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.Language;

#pragma warning disable CS0618 // Type or member is obsolete
internal class DefaultRazorCodeGenerationOptionsFeature : RazorEngineFeatureBase, IRazorCodeGenerationOptionsFeature
#pragma warning restore CS0618 // Type or member is obsolete
{
    private readonly bool _designTime;
    private ImmutableArray<IConfigureRazorCodeGenerationOptionsFeature> _configureOptions;

    public DefaultRazorCodeGenerationOptionsFeature(bool designTime)
    {
        _designTime = designTime;
    }

    protected override void OnInitialized()
    {
        _configureOptions = Engine.GetFeatures<IConfigureRazorCodeGenerationOptionsFeature>();
    }

    public RazorCodeGenerationOptions GetOptions()
    {
        return _designTime ? RazorCodeGenerationOptions.CreateDesignTime(ConfigureOptions) : RazorCodeGenerationOptions.Create(ConfigureOptions);
    }

    private void ConfigureOptions(RazorCodeGenerationOptionsBuilder builder)
    {
        foreach (var options in _configureOptions)
        {
            options.Configure(builder);
        }
    }
}
