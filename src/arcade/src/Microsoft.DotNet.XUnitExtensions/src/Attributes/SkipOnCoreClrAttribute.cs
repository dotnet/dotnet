// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit.Sdk;

namespace Xunit
{
    [TraitDiscoverer("Microsoft.DotNet.XUnitExtensions.SkipOnCoreClrDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SkipOnCoreClrAttribute : Attribute, ITraitAttribute
    {
        internal SkipOnCoreClrAttribute() { }

        public SkipOnCoreClrAttribute(string reason, TestPlatforms testPlatforms) { }
        public SkipOnCoreClrAttribute(string reason, RuntimeTestModes testMode) { }
        public SkipOnCoreClrAttribute(string reason, RuntimeConfiguration runtimeConfigurations) { }
        public SkipOnCoreClrAttribute(string reason, RuntimeConfiguration runtimeConfigurations, RuntimeTestModes testModes) { }
        public SkipOnCoreClrAttribute(string reason, TestPlatforms testPlatforms, RuntimeConfiguration runtimeConfigurations) { }
        public SkipOnCoreClrAttribute(string reason, TestPlatforms testPlatforms, RuntimeTestModes testMode) { }
        public SkipOnCoreClrAttribute(string reason, TestPlatforms testPlatforms, RuntimeConfiguration runtimeConfigurations, RuntimeTestModes testModes) { }
        public SkipOnCoreClrAttribute(string reason) { }
    }
}
