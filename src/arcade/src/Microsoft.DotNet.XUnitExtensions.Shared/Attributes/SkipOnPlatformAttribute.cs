// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.DotNet.XUnitExtensions;
using Xunit.Sdk;

namespace Xunit
{
#if !USES_XUNIT_3
    [TraitDiscoverer("Microsoft.DotNet.XUnitExtensions.SkipOnPlatformDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
#endif
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SkipOnPlatformAttribute : Attribute, ITraitAttribute
    {
#if USES_XUNIT_3
        private readonly TestPlatforms _testPlatforms = (TestPlatforms)0;
#endif

        internal SkipOnPlatformAttribute() { }
        public SkipOnPlatformAttribute(TestPlatforms testPlatforms, string reason)
        {
#if USES_XUNIT_3
            _testPlatforms = testPlatforms;
#endif
        }

#if USES_XUNIT_3
        public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits()
        {
            if (DiscovererHelpers.TestPlatformApplies(_testPlatforms))
            {
                return new[] { new KeyValuePair<string, string>(XunitConstants.Category, XunitConstants.Failing) };
            }

            return Array.Empty<KeyValuePair<string, string>>();
        }
#endif
    }
}
