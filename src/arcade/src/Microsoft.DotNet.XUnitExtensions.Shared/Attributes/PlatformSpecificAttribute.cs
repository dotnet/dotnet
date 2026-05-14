// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.DotNet.XUnitExtensions;
using Xunit.Sdk;

namespace Xunit
{
    /// <summary>
    /// Apply this attribute to your test method to specify this is a platform specific test.
    /// </summary>
#if !USES_XUNIT_3
    [TraitDiscoverer("Microsoft.DotNet.XUnitExtensions.PlatformSpecificDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
#endif
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class PlatformSpecificAttribute : Attribute, ITraitAttribute
    {
#if USES_XUNIT_3
        private readonly TestPlatforms _platforms;
#endif

        public PlatformSpecificAttribute(TestPlatforms platforms)
        {
#if USES_XUNIT_3
            _platforms = platforms;
#endif
        }

#if USES_XUNIT_3
        public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits()
        {
            return DiscovererHelpers.TestPlatformApplies(_platforms) ?
                Array.Empty<KeyValuePair<string, string>>() :
                new[] { new KeyValuePair<string, string>(XunitConstants.Category, XunitConstants.Failing) };

        }
#endif
    }
}
