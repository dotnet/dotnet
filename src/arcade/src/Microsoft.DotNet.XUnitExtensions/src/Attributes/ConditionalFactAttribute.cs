// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using Xunit.Sdk;

namespace Xunit
{
    [XunitTestCaseDiscoverer("Microsoft.DotNet.XUnitExtensions.ConditionalFactDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
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
