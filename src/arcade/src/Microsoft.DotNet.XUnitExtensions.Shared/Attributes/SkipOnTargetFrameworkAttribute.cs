// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.DotNet.XUnitExtensions;
using Xunit.Sdk;

namespace Xunit
{
    /// <summary>
    /// Apply this attribute to your test method to specify this is a platform specific test.
    /// </summary>
#if !USES_XUNIT_3
    [TraitDiscoverer("Microsoft.DotNet.XUnitExtensions.SkipOnTargetFrameworkDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
#endif
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class SkipOnTargetFrameworkAttribute : Attribute, ITraitAttribute
    {
#if USES_XUNIT_3
        private readonly TargetFrameworkMonikers _frameworks;
#endif

        public SkipOnTargetFrameworkAttribute(TargetFrameworkMonikers platform, string reason = null)
        {
#if USES_XUNIT_3
            _frameworks = platform;
#endif
        }

#if USES_XUNIT_3
        public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits()
        {
            return DiscovererHelpers.TestFrameworkApplies(_frameworks) ?
                new[] { new KeyValuePair<string, string>(XunitConstants.Category, XunitConstants.Failing) } :
                Array.Empty<KeyValuePair<string, string>>();
        }
#endif
    }
}
