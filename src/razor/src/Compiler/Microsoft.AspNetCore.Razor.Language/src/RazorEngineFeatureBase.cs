// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;

namespace Microsoft.AspNetCore.Razor.Language;

public abstract class RazorEngineFeatureBase : IRazorEngineFeature
{
    private RazorEngine _engine;

    public RazorEngine Engine
    {
        get { return _engine; }
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _engine = value;
            OnInitialized();
        }
    }

    protected TFeature GetRequiredFeature<TFeature>()
        where TFeature : class, IRazorEngineFeature
    {
        if (Engine == null)
        {
            throw new InvalidOperationException(Resources.FormatFeatureMustBeInitialized(nameof(Engine)));
        }

        if (Engine.TryGetFeature(out TFeature feature))
        {
            return feature;
        }

        throw new InvalidOperationException(
            Resources.FormatFeatureDependencyMissing(
                GetType().Name,
                typeof(TFeature).Name,
                typeof(RazorEngine).Name));
    }

    protected void ThrowForMissingDocumentDependency<TDocumentDependency>(TDocumentDependency value)
    {
        if (value == null)
        {
            throw new InvalidOperationException(
                Resources.FormatFeatureDependencyMissing(
                    GetType().Name,
                    typeof(TDocumentDependency).Name,
                    typeof(RazorCodeDocument).Name));
        }
    }

    protected virtual void OnInitialized()
    {
    }
}
