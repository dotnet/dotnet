// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed class RazorEngine
{
    public ImmutableArray<IRazorEngineFeature> Features { get; }
    public ImmutableArray<IRazorEnginePhase> Phases { get; }

    internal RazorEngine(ImmutableArray<IRazorEngineFeature> features, ImmutableArray<IRazorEnginePhase> phases)
    {
        Features = features;
        Phases = phases;

        foreach (var feature in features)
        {
            feature.Engine = this;
        }

        foreach (var phase in phases)
        {
            phase.Engine = this;
        }
    }

    public void Process(RazorCodeDocument document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        foreach (var phase in Phases)
        {
            phase.Execute(document);
        }
    }

    internal bool TryGetFeature<TFeature>([NotNullWhen(true)] out TFeature? feature)
        where TFeature : class, IRazorEngineFeature
    {
        foreach (var item in Features)
        {
            if (item is TFeature result)
            {
                feature = result;
                return true;
            }
        }

        feature = null;
        return false;
    }
}
