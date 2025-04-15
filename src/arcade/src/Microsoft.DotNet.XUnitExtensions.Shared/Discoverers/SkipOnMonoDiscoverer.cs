// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !USES_XUNIT_3
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions
{
    public class SkipOnMonoDiscoverer : ITraitDiscoverer
    {
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            if (DiscovererHelpers.IsMonoRuntime)
            {
                TestPlatforms testPlatforms = TestPlatforms.Any;

                // Last argument is either the TestPlatform or the test platform to skip the test on.
                if (traitAttribute.GetConstructorArguments().LastOrDefault() is TestPlatforms tp)
                {
                    testPlatforms = tp;
                }

                if (DiscovererHelpers.TestPlatformApplies(testPlatforms))
                {
                    return new[] { new KeyValuePair<string, string>(XunitConstants.Category, XunitConstants.Failing) };
                }
            }

            return Array.Empty<KeyValuePair<string, string>>();
        }
    }
}
#endif
