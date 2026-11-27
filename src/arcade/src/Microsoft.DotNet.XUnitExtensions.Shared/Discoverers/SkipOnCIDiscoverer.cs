// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !USES_XUNIT_3

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions;

/// <summary>
/// This class discovers all of the tests and test classes that have
/// applied the SkipOnCIAttribute
/// </summary>
public class SkipOnCIDiscoverer : ITraitDiscoverer
{
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
        TestPlatforms platforms = TestPlatforms.Any;

        // The optional second constructor argument restricts skipping to the given platforms.
        if (traitAttribute.GetConstructorArguments().Skip(1).FirstOrDefault() is TestPlatforms tp)
        {
            platforms = tp;
        }

        if (DiscovererHelpers.IsRunningInCI && DiscovererHelpers.TestPlatformApplies(platforms))
        {
            yield return new KeyValuePair<string, string>(XunitConstants.Category, XunitConstants.Failing);
        }
    }
}
#endif
