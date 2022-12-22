// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit.Sdk;

namespace Xunit
{
    /// <summary>
    /// Apply this attribute to your test method to specify a outer-loop category.
    /// </summary>
    [TraitDiscoverer("Microsoft.DotNet.XUnitExtensions.OuterLoopTestsDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class OuterLoopAttribute : Attribute, ITraitAttribute
    {
        public Type CalleeType { get; private set; }
        public string[] ConditionMemberNames { get; private set; }

        public OuterLoopAttribute() { }
        public OuterLoopAttribute(string reason) { }
        public OuterLoopAttribute(string reason, TestPlatforms platforms) { }
        public OuterLoopAttribute(string reason, TargetFrameworkMonikers framework) { }
        public OuterLoopAttribute(string reason, TestRuntimes runtimes) { }
        public OuterLoopAttribute(string reason, TestPlatforms platforms = TestPlatforms.Any, TargetFrameworkMonikers framework = TargetFrameworkMonikers.Any, TestRuntimes runtimes = TestRuntimes.Any) { }
        public OuterLoopAttribute(string reason, Type calleeType, params string[] conditionMemberNames)
        {
            CalleeType = calleeType;
            ConditionMemberNames = conditionMemberNames;
        }
    }
}
