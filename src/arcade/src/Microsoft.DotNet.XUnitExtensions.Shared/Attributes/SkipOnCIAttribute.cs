// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.DotNet.XUnitExtensions;
using Xunit.Sdk;

namespace Xunit;

/// <summary>
/// Skips the test when running in CI, optionally only on the given platforms.
/// </summary>
#if !USES_XUNIT_3
[TraitDiscoverer("Microsoft.DotNet.XUnitExtensions.SkipOnCIDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
#endif
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false)]
public sealed class SkipOnCIAttribute : Attribute, ITraitAttribute
{
    public string Reason { get; private set; }

    public TestPlatforms Platforms { get; private set; }

    public SkipOnCIAttribute(string reason)
        : this(reason, TestPlatforms.Any)
    {
    }

    public SkipOnCIAttribute(string reason, TestPlatforms platforms)
    {
        Reason = reason;
        Platforms = platforms;
    }

#if USES_XUNIT_3
    public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits()
    {
        if (DiscovererHelpers.IsRunningInCI && DiscovererHelpers.TestPlatformApplies(Platforms))
        {
            return [new KeyValuePair<string, string>(XunitConstants.Category, XunitConstants.Failing)];
        }

        return [];
    }
#endif
}
