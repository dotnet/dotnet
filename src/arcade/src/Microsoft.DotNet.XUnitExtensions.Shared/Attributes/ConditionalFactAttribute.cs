// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Not adding the support for xunit.v3.
// Still keeping the logic inside compatible with xunit.v3 in case we decided to add it.
// For cases that used to do [ConditionalFact] in xunit.v2, they can now call Assert.Skip instead of throwing SkipTestException
// In this case, [Fact] will just work because Assert.Skip is natively supported in xunit.v3
// TODO: Evaluate whether or not we want to still expose this attribute in xunit.v3 for usages of CalleeType and ConditionMemberNames?
#if !USES_XUNIT_3

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.DotNet.XUnitExtensions;
using Xunit.Sdk;

namespace Xunit
{
#if USES_XUNIT_3
    [XunitTestCaseDiscoverer(typeof(ConditionalFactDiscoverer))]
#else
    [XunitTestCaseDiscoverer("Microsoft.DotNet.XUnitExtensions.ConditionalFactDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
#endif
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ConditionalFactAttribute : FactAttribute
    {
        [DynamicallyAccessedMembers(StaticReflectionConstants.ConditionalMemberKinds)]
        public Type CalleeType { get; private set; }
        public string[] ConditionMemberNames { get; private set; }

        public ConditionalFactAttribute(
            [DynamicallyAccessedMembers(StaticReflectionConstants.ConditionalMemberKinds)]
            Type calleeType,
            params string[] conditionMemberNames)
        {
            CalleeType = calleeType;
            ConditionMemberNames = conditionMemberNames;
        }

        public ConditionalFactAttribute(params string[] conditionMemberNames)
        {
            ConditionMemberNames = conditionMemberNames;
        }
    }
}
#endif
