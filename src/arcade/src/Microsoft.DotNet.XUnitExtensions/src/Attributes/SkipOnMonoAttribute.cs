// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit.Sdk;

namespace Xunit
{
    [TraitDiscoverer("Microsoft.DotNet.XUnitExtensions.SkipOnMonoDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SkipOnMonoAttribute : Attribute, ITraitAttribute
    {
        internal SkipOnMonoAttribute() { }
        public SkipOnMonoAttribute(string reason, TestPlatforms testPlatforms = TestPlatforms.Any) { }
    }
}
